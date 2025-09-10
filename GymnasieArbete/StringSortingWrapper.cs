using System;
using System.Collections.Generic;
using System.Linq;
using static GymnasieArbete.SortingAlgorithms;

namespace GymnasieArbete
{
    public class StringSortingWrapper : ISortingAlgorithm<string>
    {
        private ISortingAlgorithm<int> inner;
        private string[] words;
        private int[] indices;

        public StringSortingWrapper(string[] words, string algorithmName)
        {
            this.words = (string[])words.Clone();
            this.indices = Enumerable.Range(0, words.Length).ToArray();
            this.inner = CreateIndexSorter(algorithmName, this.words);
            this.inner.Reset(indices);
        }

        public string[] Items => indices.Select(i => words[i]).ToArray();
        public int CurrentI => inner.CurrentI;
        public int CurrentJ => inner.CurrentJ;
        public string StatusMessage => inner.StatusMessage;
        public int[] Indices => inner.Items;
        public string[] Words => words;

        public bool Step() => inner.Step();

        public void Reset(string[] items)
        {
            words = (string[])items.Clone();
            indices = Enumerable.Range(0, words.Length).ToArray();
            inner = CreateIndexSorter(inner.GetType().Name.Replace("Index", "").Replace("Sort", " Sort"), words);
            inner.Reset(indices);
        }

        // Factory for index-based sorters
        public static ISortingAlgorithm<int> CreateIndexSorter(string name, string[] words)
        {
            switch (name)
            {
                case "Bubble Sort": return new IndexBubbleSort(words);
                case "Quick Sort": return new IndexQuickSort(words);
                case "Merge Sort": return new IndexMergeSort(words);
                case "Insertion Sort": return new IndexInsertionSort(words);
                case "Selection Sort": return new IndexSelectionSort(words);
                case "Heap Sort": return new IndexHeapSort(words);
                case "Bogo Sort": return new IndexBogoSort(words);
                default: throw new NotImplementedException();
            }
        }

        public static int GetWordScore(string word)
        {
            int score = 0;
            foreach (char c in word.ToLowerInvariant())
            {
                if (c >= 'a' && c <= 'z')
                    score += (c - 'a' + 1);
            }
            return score;
        }
    }

    // Bubble Sort
    public class IndexBubbleSort : ISortingAlgorithm<int>
    {
        private int[] indices;
        private int currentI, currentJ;
        private bool swapped;
        private string statusMessage;
        private string[] words;

        public int[] Items => indices;
        public int CurrentI => currentI;
        public int CurrentJ => currentJ;
        public string StatusMessage => statusMessage;

        public IndexBubbleSort(string[] words) { this.words = words; }

        public void Reset(int[] items)
        {
            indices = (int[])items.Clone();
            currentI = 0;
            currentJ = 0;
            swapped = false;
            statusMessage = "Ready to start";
        }

        public bool Step()
        {
            if (currentI < indices.Length)
            {
                if (currentJ == 0) swapped = false;

                if (currentJ < indices.Length - currentI - 1)
                {
                    statusMessage = $"Comparing {words[indices[currentJ]]} and {words[indices[currentJ + 1]]}";
                    if (StringSortingWrapper.GetWordScore(words[indices[currentJ]]) > StringSortingWrapper.GetWordScore(words[indices[currentJ + 1]]))
                    {
                        int temp = indices[currentJ];
                        indices[currentJ] = indices[currentJ + 1];
                        indices[currentJ + 1] = temp;
                        swapped = true;
                        statusMessage = $"Swapped {words[indices[currentJ + 1]]} and {words[indices[currentJ]]}";
                    }
                    currentJ++;
                }
                else
                {
                    if (!swapped)
                    {
                        statusMessage = "Finished (no swaps in pass)";
                        currentI = indices.Length;
                        return false;
                    }
                    currentJ = 0;
                    currentI++;
                    statusMessage = $"Completed pass {currentI}";
                }
                return true;
            }
            statusMessage = "Finished";
            return false;
        }
    }

    // Insertion Sort
    public class IndexInsertionSort : ISortingAlgorithm<int>
    {
        private int[] indices;
        private int currentI, currentJ;
        private int key;
        private bool keyPicked;
        private string statusMessage;
        private string[] words;

        public int[] Items => indices;
        public int CurrentI => currentI;
        public int CurrentJ => currentJ;
        public string StatusMessage => statusMessage;

        public IndexInsertionSort(string[] words) { this.words = words; }

        public void Reset(int[] items)
        {
            indices = (int[])items.Clone();
            currentI = 1;
            currentJ = 0;
            key = -1;
            keyPicked = false;
            statusMessage = "Ready to start";
        }

        public bool Step()
        {
            if (currentI < indices.Length)
            {
                if (!keyPicked)
                {
                    key = indices[currentI];
                    currentJ = currentI - 1;
                    keyPicked = true;
                    statusMessage = $"Picked key {words[key]}";
                }
                if (currentJ >= 0 && StringSortingWrapper.GetWordScore(words[indices[currentJ]]) > StringSortingWrapper.GetWordScore(words[key]))
                {
                    statusMessage = $"Shifting {words[indices[currentJ]]} right";
                    indices[currentJ + 1] = indices[currentJ];
                    currentJ--;
                    return true;
                }
                indices[currentJ + 1] = key;
                statusMessage = $"Inserted key {words[key]} at position {currentJ + 1}";
                currentI++;
                keyPicked = false;
                return true;
            }
            statusMessage = "Finished";
            return false;
        }
    }

    // Selection Sort
    public class IndexSelectionSort : ISortingAlgorithm<int>
    {
        private int[] indices;
        private int currentI, currentJ;
        private int minIndex;
        private string statusMessage;
        private string[] words;

        public int[] Items => indices;
        public int CurrentI => currentI;
        public int CurrentJ => currentJ;
        public string StatusMessage => statusMessage;

        public IndexSelectionSort(string[] words) { this.words = words; }

        public void Reset(int[] items)
        {
            indices = (int[])items.Clone();
            currentI = 0;
            currentJ = 1;
            minIndex = 0;
            statusMessage = "Ready to start";
        }

        public bool Step()
        {
            if (currentI >= indices.Length - 1)
            {
                statusMessage = "Finished";
                return false;
            }

            if (currentJ < indices.Length)
            {
                statusMessage = $"Comparing {words[indices[currentJ]]} with current min {words[indices[minIndex]]}";
                if (StringSortingWrapper.GetWordScore(words[indices[currentJ]]) < StringSortingWrapper.GetWordScore(words[indices[minIndex]]))
                {
                    minIndex = currentJ;
                    statusMessage = $"New min found: {words[indices[minIndex]]} at {minIndex}";
                }
                currentJ++;
                return true;
            }
            else
            {
                int temp = indices[currentI];
                indices[currentI] = indices[minIndex];
                indices[minIndex] = temp;
                statusMessage = $"Swapped {words[indices[currentI]]} and {words[indices[minIndex]]}";
                currentI++;
                minIndex = currentI;
                currentJ = currentI + 1;
                return true;
            }
        }
    }

    // Quick Sort
    public class IndexQuickSort : ISortingAlgorithm<int>
    {
        private int[] indices;
        private int currentI, currentJ;
        private string statusMessage;
        private string[] words;

        private Stack<(int left, int right)> stack;
        private int left, right, i, j, pivotIndex;
        private bool partitioning;

        public int[] Items => indices;
        public int CurrentI => currentI;
        public int CurrentJ => currentJ;
        public string StatusMessage => statusMessage;

        public IndexQuickSort(string[] words) { this.words = words; }

        public void Reset(int[] items)
        {
            indices = (int[])items.Clone();
            stack = new Stack<(int, int)>();
            stack.Push((0, indices.Length - 1));
            partitioning = false;
            left = right = i = j = pivotIndex = -1;
            currentI = currentJ = -1;
            statusMessage = "Ready to start";
        }

        private int MedianOfThree(int a, int b, int c)
        {
            int wa = StringSortingWrapper.GetWordScore(words[indices[a]]);
            int wb = StringSortingWrapper.GetWordScore(words[indices[b]]);
            int wc = StringSortingWrapper.GetWordScore(words[indices[c]]);
            if ((wa > wb) == (wa < wc)) return a;
            if ((wb > wa) == (wb < wc)) return b;
            return c;
        }

        public bool Step()
        {
            if (!partitioning)
            {
                if (stack.Count == 0)
                {
                    statusMessage = "Finished";
                    return false;
                }

                (left, right) = stack.Pop();
                if (left >= right)
                {
                    statusMessage = $"Subarray [{left}, {right}] already sorted";
                    return true;
                }

                int mid = left + (right - left) / 2;
                int median = MedianOfThree(left, mid, right);
                if (median == mid)
                    (indices[mid], indices[right]) = (indices[right], indices[mid]);
                else if (median == left)
                    (indices[left], indices[right]) = (indices[right], indices[left]);
                pivotIndex = right;

                i = left - 1;
                j = left;
                partitioning = true;
                statusMessage = $"Partitioning [{left}, {right}] with pivot {words[indices[pivotIndex]]}";
            }

            if (j < right)
            {
                currentI = j;
                currentJ = right;
                statusMessage = $"Comparing {words[indices[j]]} with pivot {words[indices[pivotIndex]]}";
                if (StringSortingWrapper.GetWordScore(words[indices[j]]) < StringSortingWrapper.GetWordScore(words[indices[pivotIndex]]))
                {
                    i++;
                    (indices[i], indices[j]) = (indices[j], indices[i]);
                    statusMessage = $"Swapped {words[indices[i]]} and {words[indices[j]]}";
                }
                j++;
                return true;
            }
            else
            {
                (indices[i + 1], indices[right]) = (indices[right], indices[i + 1]);
                statusMessage = $"Placed pivot {words[indices[i + 1]]} at position {i + 1}";
                stack.Push((left, i));
                stack.Push((i + 2, right));
                partitioning = false;
                return true;
            }
        }
    }

    // Merge Sort
    public class IndexMergeSort : ISortingAlgorithm<int>
    {
        private int[] indices;
        private int currentI, currentJ;
        private string statusMessage;
        private string[] words;

        private int[] aux;
        private int width;
        private Queue<(int left, int mid, int right)> mergeJobs;
        private int left, mid, right, i, j, k;
        private bool merging;
        private bool done;
        private bool copyBack;
        private int copyIdx;

        public int[] Items => indices;
        public int CurrentI => currentI;
        public int CurrentJ => currentJ;
        public string StatusMessage => statusMessage;

        public IndexMergeSort(string[] words) { this.words = words; }

        public void Reset(int[] items)
        {
            indices = (int[])items.Clone();
            aux = new int[indices.Length];
            width = 1;
            mergeJobs = new Queue<(int, int, int)>();
            merging = false;
            done = false;
            copyBack = false;
            copyIdx = 0;
            currentI = currentJ = -1;
            statusMessage = "Ready to start";
            i = j = k = 0;
        }

        public bool Step()
        {
            if (done)
            {
                statusMessage = "Finished";
                return false;
            }

            if (copyBack)
            {
                if (copyIdx < indices.Length)
                {
                    indices[copyIdx] = aux[copyIdx];
                    currentJ = copyIdx;
                    statusMessage = $"Copying back index {copyIdx}";
                    copyIdx++;
                    return true;
                }
                else
                {
                    width *= 2;
                    merging = false;
                    copyBack = false;
                    copyIdx = 0;
                    statusMessage = $"Increased merge width to {width}";
                    return true;
                }
            }

            if (!merging)
            {
                if (width >= indices.Length)
                {
                    for (int idx = 0; idx < indices.Length; idx++)
                        indices[idx] = aux[idx];
                    done = true;
                    statusMessage = "Finished";
                    return false;
                }
                mergeJobs.Clear();
                for (int l = 0; l < indices.Length - 1; l += 2 * width)
                {
                    int m = Math.Min(l + width - 1, indices.Length - 1);
                    int r = Math.Min(l + 2 * width - 1, indices.Length - 1);
                    mergeJobs.Enqueue((l, m, r));
                }
                merging = true;
                statusMessage = $"Prepared merge jobs for width {width}";
                i = j = k = 0;
            }

            if (merging && mergeJobs.Count > 0)
            {
                if (k == 0)
                {
                    (left, mid, right) = mergeJobs.Peek();
                    i = left;
                    j = mid + 1;
                    k = left;
                    statusMessage = $"Merging [{left}, {mid}] and [{mid + 1}, {right}]";
                }

                if (i <= mid && (j > right || StringSortingWrapper.GetWordScore(words[indices[i]]) <= StringSortingWrapper.GetWordScore(words[indices[j]])))
                {
                    aux[k] = indices[i];
                    currentJ = i;
                    statusMessage = $"Taking {words[indices[i]]} from left";
                    i++;
                }
                else if (j <= right)
                {
                    aux[k] = indices[j];
                    currentJ = j;
                    statusMessage = $"Taking {words[indices[j]]} from right";
                    j++;
                }
                k++;

                if (k > right)
                {
                    mergeJobs.Dequeue();
                    k = 0;
                    statusMessage = $"Finished merging [{left}, {right}]";
                }

                return true;
            }
            else if (merging && mergeJobs.Count == 0)
            {
                copyBack = true;
                copyIdx = 0;
                statusMessage = $"Copying merged array back";
                return true;
            }

            statusMessage = "Finished";
            return false;
        }
    }

    // Heap Sort
    public class IndexHeapSort : ISortingAlgorithm<int>
    {
        private int[] indices;
        private int currentI, currentJ;
        private string statusMessage;
        private string[] words;

        private int heapSize;
        private int phase;
        private int siftIndex;

        public int[] Items => indices;
        public int CurrentI => currentI;
        public int CurrentJ => currentJ;
        public string StatusMessage => statusMessage;

        public IndexHeapSort(string[] words) { this.words = words; }

        public void Reset(int[] items)
        {
            indices = (int[])items.Clone();
            heapSize = indices.Length;
            phase = 0;
            siftIndex = heapSize / 2 - 1;
            currentI = -1;
            currentJ = -1;
            statusMessage = "Ready to start";
        }

        public bool Step()
        {
            if (phase == 0)
            {
                if (siftIndex >= 0)
                {
                    statusMessage = $"Sifting down index {siftIndex}";
                    SiftDown(siftIndex, heapSize);
                    siftIndex--;
                    return true;
                }
                else
                {
                    phase = 1;
                    currentI = heapSize - 1;
                    statusMessage = "Heap built, starting sort";
                }
            }
            if (phase == 1)
            {
                if (heapSize > 1)
                {
                    int temp = indices[0];
                    indices[0] = indices[heapSize - 1];
                    indices[heapSize - 1] = temp;
                    statusMessage = $"Swapped root {words[indices[heapSize - 1]]} with {words[indices[0]]}";
                    heapSize--;
                    SiftDown(0, heapSize);
                    currentI = heapSize - 1;
                    return true;
                }
                else
                {
                    statusMessage = "Finished";
                    return false;
                }
            }
            statusMessage = "Finished";
            return false;
        }

        private void SiftDown(int i, int n)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && StringSortingWrapper.GetWordScore(words[indices[left]]) > StringSortingWrapper.GetWordScore(words[indices[largest]]))
                largest = left;
            if (right < n && StringSortingWrapper.GetWordScore(words[indices[right]]) > StringSortingWrapper.GetWordScore(words[indices[largest]]))
                largest = right;

            currentJ = largest;

            if (largest != i)
            {
                int temp = indices[i];
                indices[i] = indices[largest];
                indices[largest] = temp;
                statusMessage = $"Sifted down: swapped {words[indices[largest]]} and {words[indices[i]]}";
                SiftDown(largest, n);
            }
            else
            {
                statusMessage = $"No swap needed at index {i}";
            }
        }
    }

    // Bogo Sort
    public class IndexBogoSort : ISortingAlgorithm<int>
    {
        private int[] indices;
        private int currentI, currentJ;
        private string statusMessage;
        private string[] words;
        private Random rand = new Random();

        public int[] Items => indices;
        public int CurrentI => currentI;
        public int CurrentJ => currentJ;
        public string StatusMessage => statusMessage;

        public IndexBogoSort(string[] words) { this.words = words; }

        public void Reset(int[] items)
        {
            indices = (int[])items.Clone();
            currentI = 0;
            currentJ = 1;
            statusMessage = "Ready to start";
        }

        private bool IsSorted()
        {
            for (int i = 1; i < indices.Length; i++)
                if (StringSortingWrapper.GetWordScore(words[indices[i - 1]]) > StringSortingWrapper.GetWordScore(words[indices[i]]))
                    return false;
            return true;
        }

        public bool Step()
        {
            if (indices.Length <= 1)
            {
                statusMessage = "Finished";
                return false;
            }

            if (IsSorted())
            {
                statusMessage = "Finished";
                return false;
            }

            for (int i = indices.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = indices[i];
                indices[i] = indices[j];
                indices[j] = temp;
            }
            statusMessage = "Shuffled array";
            currentI = 0;
            currentJ = 1;
            return true;
        }
    }
}