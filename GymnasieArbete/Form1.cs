using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GymnasieArbete.SortingAlgorithms;

namespace GymnasieArbete
{
    public partial class Form1 : Form
    {
        private ISortingAlgorithm<int>[] algorithmsInt = new ISortingAlgorithm<int>[4];
        private ISortingAlgorithm<string>[] algorithmsString = new ISortingAlgorithm<string>[4];
        private Panel[] panels;
        private Label[] labels;
        private int[] sampleArray;
        private System.Diagnostics.Stopwatch[] stopwatches = new System.Diagnostics.Stopwatch[4];
        private long[] elapsedTimes = new long[4];
        private string[] algorithmNames = new string[4];
        private Task[] sortingTasks = new Task[4];
        private CancellationTokenSource[] cancellationTokens = new CancellationTokenSource[4];
        private Label[] statusLabels;
        private bool wordMode = false;
        private string[] sampleWords;

        public Form1()
        {
            InitializeComponent();
            timer1.Tick += Timer_Tick;
            timer1.Interval = 25;
            panelVisualizer.Paint += PanelVisualizer_Paint;

            panels = new Panel[] { panelVisualizer, panelVisualizer2, panelVisualizer3, panelVisualizer4 };
            labels = new Label[] { Algorithm1, Algorithm2, Algorithm3, Algorithm4 };
            statusLabels = new Label[] { labelStatus1, labelStatus2, labelStatus3, labelStatus4 };
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].Visible = false;
                labels[i].Visible = false;
            }
        }

        private int[] GenerateRandomArray(int size, int minValue, int maxValue)
        {
            var rand = new Random();
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
                arr[i] = rand.Next(minValue, maxValue + 1); // Inclusive of maxValue
            return arr;
        }
        private string[] GenerateRandomWords(int size)
        {
            string[] wordBank = { "apple", "banana", "cherry", "date", "fig", "grape", "kiwi", "lemon", "mango", "orange", "peach", "pear", "plum", "quince", "raspberry", "strawberry", "tomato", "watermelon" };
            var rand = new Random();
            string[] arr = new string[size];
            for (int i = 0; i < size; i++)
                arr[i] = wordBank[rand.Next(wordBank.Length)];
            return arr;
        }
        private string[] GenerateRandomStrings(int size, int stringLength = 5)
        {
            var rand = new Random();
            var arr = new string[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = new string(Enumerable.Range(0, stringLength)
                    .Select(_ => (char)rand.Next('a', 'z' + 1)).ToArray());
            }
            return arr;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int selectedCount = checkedListAlgorithms.CheckedItems.Count;
            if (selectedCount == 0)
            {
                MessageBox.Show("Select at least one algorithm.");
                return;
            }
            if (selectedCount > 4)
            {
                MessageBox.Show("You can select up to 4 algorithms.");
                return;
            }

            int sampleSize = (int)numericSampleSize.Value;
            if (wordMode)
            {
                sampleWords = GenerateRandomStrings(sampleSize);
            }
            else
            {
                int minValue = 1;
                int maxValue = 10000;
                if (int.TryParse(txtMinValue.Text, out int min))
                    minValue = min;
                if (int.TryParse(txtMaxValue.Text, out int max))
                    maxValue = max;
                sampleArray = GenerateRandomArray(sampleSize, minValue, maxValue);
            }

            for (int i = 0; i < 4; i++)
            {
                if (i < selectedCount)
                {
                    string selectedAlg = checkedListAlgorithms.CheckedItems[i].ToString();
                    if (wordMode)
                    {
                        algorithmsString[i] = new StringSortingWrapper((string[])sampleWords.Clone(), selectedAlg);
                        int minValue = 1;
                        int maxValue = 10000;

                        if (int.TryParse(txtMinValue.Text, out int min))
                            minValue = min;
                        if (int.TryParse(txtMaxValue.Text, out int max))
                            maxValue = max;

                        // Use minValue and maxValue in your number generation logic
                        Random rnd = new Random();
                        int generatedNumber = rnd.Next(minValue, maxValue + 1); // Inclusive of maxValue
                        algorithmsString[i].Reset((string[])sampleWords.Clone());
                    }
                    else
                    {
                        algorithmsInt[i] = CreateAlgorithmInstance<int>(selectedAlg);
                        algorithmsInt[i].Reset((int[])sampleArray.Clone());
                    }
                    panels[i].Invalidate();
                    labels[i].Text = selectedAlg;
                    panels[i].Paint -= PanelVisualizer_Paint;
                    panels[i].Paint += PanelVisualizer_Paint;
                    panels[i].Visible = true;
                    labels[i].Visible = true;
                    stopwatches[i] = System.Diagnostics.Stopwatch.StartNew();
                    elapsedTimes[i] = 0;
                    algorithmNames[i] = selectedAlg;
                }
                else
                {
                    algorithmsInt[i] = null;
                    algorithmsString[i] = null;
                    panels[i].Invalidate();
                    labels[i].Text = "";
                    panels[i].Visible = false;
                    labels[i].Visible = false;
                }
            }

            btnStart.Enabled = false;

            // Cancel any previous runs
            for (int i = 0; i < 4; i++)
            {
                cancellationTokens[i]?.Cancel();
                cancellationTokens[i] = null;
                sortingTasks[i] = null;
            }

            // Start new background tasks
            for (int i = 0; i < 4; i++)
            {
                if (wordMode)
                {
                    if (algorithmsString[i] != null)
                    {
                        int idx = i;
                        cancellationTokens[idx] = new CancellationTokenSource();
                        sortingTasks[idx] = Task.Run(() => RunSortingString(idx, cancellationTokens[idx].Token));
                    }
                }
                else
                {
                    if (algorithmsInt[i] != null)
                    {
                        int idx = i;
                        cancellationTokens[idx] = new CancellationTokenSource();
                        sortingTasks[idx] = Task.Run(() => RunSortingInt(idx, cancellationTokens[idx].Token));
                    }
                }
            }
        }

        private ISortingAlgorithm<T> CreateAlgorithmInstance<T>(string name) where T : IComparable<T>
        {
            switch (name)
            {
                case "Bubble Sort":
                    return new SortingAlgorithms.BubbleSort<T>();
                case "Quick Sort":
                    return new SortingAlgorithms.QuickSort<T>();
                case "Merge Sort":
                    return new SortingAlgorithms.MergeSort<T>();
                case "Insertion Sort":
                    return new SortingAlgorithms.InsertionSort<T>();
                case "Heap Sort":
                    return new SortingAlgorithms.HeapSort<T>();
                case "Selection Sort":
                    return new SortingAlgorithms.SelectionSort<T>();
                case "Bogo Sort":
                    return new SortingAlgorithms.BogoSort<T>();
                default:
                    throw new NotImplementedException();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int speedMultiplier = GetSpeedMultiplier();
            bool anyRunning = false;

            for (int i = 0; i < 4; i++)
            {
                if (wordMode)
                {
                    if (algorithmsString[i] != null)
                    {
                        statusLabels[i].Text = algorithmsString[i].StatusMessage;
                        if (stopwatches[i] != null && stopwatches[i].IsRunning)
                            elapsedTimes[i] = stopwatches[i].ElapsedMilliseconds;
                        panels[i].Invalidate();
                        if (sortingTasks[i] != null && !sortingTasks[i].IsCompleted)
                            anyRunning = true;
                    }
                    else
                    {
                        statusLabels[i].Text = "";
                    }
                }
                else
                {
                    if (algorithmsInt[i] != null)
                    {
                        statusLabels[i].Text = algorithmsInt[i].StatusMessage;
                        if (stopwatches[i] != null && stopwatches[i].IsRunning)
                            elapsedTimes[i] = stopwatches[i].ElapsedMilliseconds;
                        panels[i].Invalidate();
                        if (sortingTasks[i] != null && !sortingTasks[i].IsCompleted)
                            anyRunning = true;
                    }
                    else
                    {
                        statusLabels[i].Text = "";
                    }
                }
            }

            if (!anyRunning)
            {
                btnStart.Enabled = true;
                ShowResults();
            }
        }

        private void PanelVisualizer_Paint(object sender, PaintEventArgs e)
        {
            int idx = Array.IndexOf(panels, sender as Panel);
            if (idx < 0) return;

            int n, panelWidth, panelHeight;
            int[] values = null;
            int currentI = -1, currentJ = -1;

            panelWidth = panels[idx].Width;
            panelHeight = panels[idx].Height;

            if (wordMode)
            {
                var alg = algorithmsString[idx] as StringSortingWrapper;
                if (alg == null || alg.Indices == null || alg.Words == null || alg.Indices.Length == 0) return;

                n = alg.Indices.Length;
                int[] indices = alg.Indices;
                string[] words = alg.Words;

                // Calculate scores for all words in current order
                int[] scores = new int[n];
                int minScore = int.MaxValue, maxScore = int.MinValue;
                for (int i = 0; i < n; i++)
                {
                    int score = StringSortingWrapper.GetWordScore(words[indices[i]]);
                    scores[i] = score;
                    if (score < minScore) minScore = score;
                    if (score > maxScore) maxScore = score;
                }

                // Normalize scores to fit panel height
                values = new int[n];
                int range = Math.Max(1, maxScore - minScore);
                for (int i = 0; i < n; i++)
                {
                    // Map score to [10, panelHeight-10]
                    values[i] = 10 + (panelHeight - 20) * (scores[i] - minScore) / range;
                }
                currentI = alg.CurrentI;
                currentJ = alg.CurrentJ;
            }
            else
            {
                var alg = algorithmsInt[idx];
                if (alg == null || alg.Items == null || alg.Items.Length == 0) return;

                n = alg.Items.Length;
                values = alg.Items;
                currentI = alg.CurrentI;
                currentJ = alg.CurrentJ;

                // Find min and max in the current array
                int minValue = values.Min();
                int maxValue = values.Max();
                int range = Math.Max(1, maxValue - minValue);

                // Normalize values to fit panel height
                int[] normalized = new int[n];
                for (int i = 0; i < n; i++)
                {
                    // Map value to [10, panelHeight-10]
                    normalized[i] = 10 + (panelHeight - 20) * (values[i] - minValue) / range;
                }

                // Draw lines for each value
                for (int x = 0; x < panelWidth; x++)
                {
                    int index = x * n / panelWidth;
                    int value = normalized[index];
                    e.Graphics.DrawLine(Pens.Green, x, panelHeight, x, panelHeight - value);
                }

                // Draw active indices
                DrawIndex(currentI, Pens.Blue, n, panelWidth, panelHeight, e);
                DrawIndex(currentJ, Pens.Red, n, panelWidth, panelHeight, e);
                return;
            }

            // Draw lines for each value (word mode)
            for (int x = 0; x < panelWidth; x++)
            {
                int index = x * n / panelWidth;
                int value = values[index];
                e.Graphics.DrawLine(Pens.Green, x, panelHeight, x, panelHeight - value);
            }

            // Draw active indices (word mode)
            DrawIndex(currentI, Pens.Blue, n, panelWidth, panelHeight, e);
            DrawIndex(currentJ, Pens.Red, n, panelWidth, panelHeight, e);
        }

        // Helper method to draw index lines
        private void DrawIndex(int index, Pen pen, int n, int panelWidth, int panelHeight, PaintEventArgs e)
        {
            if (index >= 0 && index < n)
            {
                int x = index * panelWidth / n;
                e.Graphics.DrawLine(pen, x, 0, x, panelHeight);
            }
        }



        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                cancellationTokens[i]?.Cancel();
                cancellationTokens[i] = null;
                sortingTasks[i] = null;
            }

            btnStart.Enabled = true;
            listViewResults.Items.Clear();

            for (int i = 0; i < 4; i++)
            {
                algorithmsInt[i] = null;
                algorithmsString[i] = null;
                panels[i].Invalidate();
                labels[i].Text = "";
                panels[i].Visible = false;
                labels[i].Visible = false;
                statusLabels[i].Text = "";
            }
        }

        private void CheckedListAlgorithms_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int checkedCount = checkedListAlgorithms.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1);
            if (checkedCount > 4)
            {
                e.NewValue = CheckState.Unchecked;
                MessageBox.Show("You can select up to 4 algorithms.");
            }
        }

        private void ShowResults()
        {
            for (int i = 0; i < 4; i++)
            {
                if (wordMode)
                {
                    if (algorithmsString[i] != null && stopwatches[i] != null && stopwatches[i].IsRunning)
                    {
                        stopwatches[i].Stop();
                        elapsedTimes[i] = stopwatches[i].ElapsedMilliseconds;
                    }
                }
                else
                {
                    if (algorithmsInt[i] != null && stopwatches[i] != null && stopwatches[i].IsRunning)
                    {
                        stopwatches[i].Stop();
                        elapsedTimes[i] = stopwatches[i].ElapsedMilliseconds;
                    }
                }
            }

            listViewResults.Items.Clear();

            var results = new List<(string Name, long Time)>();
            for (int i = 0; i < 4; i++)
            {
                if (wordMode)
                {
                    if (algorithmsString[i] != null && !string.IsNullOrEmpty(algorithmNames[i]))
                    {
                        results.Add((algorithmNames[i], elapsedTimes[i]));
                    }
                }
                else
                {
                    if (algorithmsInt[i] != null && !string.IsNullOrEmpty(algorithmNames[i]))
                    {
                        results.Add((algorithmNames[i], elapsedTimes[i]));
                    }
                }
            }

            var sorted = results.OrderBy(r => r.Time);

            int speedMultiplier = GetSpeedMultiplier();

            foreach (var result in sorted)
            {
                long actualTimeMs = result.Time * speedMultiplier;
                double seconds = actualTimeMs / 1000.0;
                string timeDisplay = $"{seconds:0.000} s ({actualTimeMs} ms)";
                var item = new ListViewItem(new[] { result.Name, timeDisplay });
                listViewResults.Items.Add(item);
            }

            listViewResults.View = View.Details;
        }

        private int GetSpeedMultiplier()
        {
            switch (comboBoxSpeed.SelectedItem?.ToString())
            {
                case "1x": return 1;
                case "2x": return 2;
                case "4x": return 4;
                case "8x": return 8;
                case "16x": return 16;
                case "32x": return 32;
                case "64x": return 64;
                case "128x": return 128;
                case "256x": return 256;
                case "512x": return 512;
                case "1024x": return 1024;
                case "2048x": return 2048;
                case "4096x": return 4096;
                case "8192x": return 8192;
                default: return 1;
            }
        }

        private void UpdateTimerInterval()
        {
            int baseInterval = 25;
            int speed = GetSpeedMultiplier();
            timer1.Interval = Math.Max(1, baseInterval / speed);
        }

        private void comboBoxSpeed_DropDownClosed(object sender, EventArgs e)
        {
            UpdateTimerInterval();
            if (!timer1.Enabled)
                ShowResults();
        }

        private void RunSortingInt(int idx, CancellationToken token)
        {
            int speedMultiplier;
            try
            {
                int minValue = sampleArray.Min();
                int maxValue = sampleArray.Max();
                int stepCounter = 0;
                int uiUpdateInterval = 1;

                while (algorithmsInt[idx] != null && !token.IsCancellationRequested)
                {
                    speedMultiplier = 1;
                    Invoke(new Action(() => speedMultiplier = GetSpeedMultiplier()));
                    uiUpdateInterval = Math.Max(1, speedMultiplier / 8);

                    bool running = false;
                    for (int s = 0; s < speedMultiplier; s++)
                    {
                        if (token.IsCancellationRequested) break;
                        if (algorithmsInt[idx].Step())
                        {
                            running = true;
                            stepCounter++;
                            if (stepCounter % uiUpdateInterval == 0)
                            {
                                Invoke(new Action(() =>
                                {
                                    statusLabels[idx].Text = algorithmsInt[idx].StatusMessage;
                                    panels[idx].Invalidate();
                                }));
                            }
                        }
                        else
                        {
                            Invoke(new Action(() => statusLabels[idx].Text = algorithmsInt[idx].StatusMessage));
                            break;
                        }
                    }

                    if (stopwatches[idx] != null && stopwatches[idx].IsRunning)
                    {
                        elapsedTimes[idx] = stopwatches[idx].ElapsedMilliseconds;
                    }

                    if (stepCounter % uiUpdateInterval != 0)
                    {
                        Invoke(new Action(() => panels[idx].Invalidate()));
                    }

                    if (!running)
                    {
                        if (stopwatches[idx] != null && stopwatches[idx].IsRunning)
                        {
                            stopwatches[idx].Stop();
                            elapsedTimes[idx] = stopwatches[idx].ElapsedMilliseconds;
                        }
                        break;
                    }

                    Thread.Sleep(1);
                }
            }
            catch (ObjectDisposedException)
            {
                // Form is closing, ignore
            }

            if (AllSortsDone())
            {
                Invoke(new Action(() =>
                {
                    btnStart.Enabled = true;
                    ShowResults();
                }));
            }
        }

        private void RunSortingString(int idx, CancellationToken token)
        {
            int speedMultiplier;
            try
            {
                int stepCounter = 0;
                int uiUpdateInterval = 1;

                while (algorithmsString[idx] != null && !token.IsCancellationRequested)
                {
                    speedMultiplier = 1;
                    Invoke(new Action(() => speedMultiplier = GetSpeedMultiplier()));
                    uiUpdateInterval = Math.Max(1, speedMultiplier / 8);

                    bool running = false;
                    for (int s = 0; s < speedMultiplier; s++)
                    {
                        if (token.IsCancellationRequested) break;
                        if (algorithmsString[idx].Step())
                        {
                            running = true;
                            stepCounter++;
                            if (stepCounter % uiUpdateInterval == 0)
                            {
                                Invoke(new Action(() =>
                                {
                                    statusLabels[idx].Text = algorithmsString[idx].StatusMessage;
                                    panels[idx].Invalidate();
                                }));
                            }
                        }
                        else
                        {
                            Invoke(new Action(() => statusLabels[idx].Text = algorithmsString[idx].StatusMessage));
                            break;
                        }
                    }

                    if (stopwatches[idx] != null && stopwatches[idx].IsRunning)
                    {
                        elapsedTimes[idx] = stopwatches[idx].ElapsedMilliseconds;
                    }

                    if (stepCounter % uiUpdateInterval != 0)
                    {
                        Invoke(new Action(() => panels[idx].Invalidate()));
                    }

                    if (!running)
                    {
                        if (stopwatches[idx] != null && stopwatches[idx].IsRunning)
                        {
                            stopwatches[idx].Stop();
                            elapsedTimes[idx] = stopwatches[idx].ElapsedMilliseconds;
                        }
                        break;
                    }

                    Thread.Sleep(1);
                }
            }
            catch (ObjectDisposedException)
            {
                // Form is closing, ignore
            }

            if (AllSortsDone())
            {
                Invoke(new Action(() =>
                {
                    btnStart.Enabled = true;
                    ShowResults();
                }));
            }
        }

        private bool AllSortsDone()
        {
            for (int i = 0; i < 4; i++)
            {
                if (wordMode)
                {
                    if (algorithmsString[i] != null && (sortingTasks[i] != null && !sortingTasks[i].IsCompleted))
                        return false;
                }
                else
                {
                    if (algorithmsInt[i] != null && (sortingTasks[i] != null && !sortingTasks[i].IsCompleted))
                        return false;
                }
            }
            return true;
        }

        private void labelStatus3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void checkBoxWordMode_CheckedChanged(object sender, EventArgs e)
        {
            wordMode = checkBoxWordMode.Checked;
            UpdateRangeControlsVisibility();
        }
        private void UpdateRangeControlsVisibility()
        {
            bool visible = !wordMode;
            txtMinValue.Visible = visible;
            txtMaxValue.Visible = visible;
            lblMinValue.Visible = visible;
            lblMaxValue.Visible = visible;
        }
    }
}
