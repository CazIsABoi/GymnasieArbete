namespace GymnasieArbete
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelVisualizer = new DoubleBufferedPanel();
            btnStart = new Button();
            panelVisualizer2 = new DoubleBufferedPanel();
            panelVisualizer3 = new DoubleBufferedPanel();
            panelVisualizer4 = new DoubleBufferedPanel();
            listViewResults = new ListView();
            Algorithm = new ColumnHeader();
            Time = new ColumnHeader();
            checkedListAlgorithms = new CheckedListBox();
            numericSampleSize = new NumericUpDown();
            btnReset = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            label2 = new Label();
            Algorithm3 = new Label();
            Algorithm4 = new Label();
            Algorithm1 = new Label();
            Algorithm2 = new Label();
            label3 = new Label();
            comboBoxSpeed = new ComboBox();
            label4 = new Label();
            labelStatus1 = new Label();
            labelStatus2 = new Label();
            labelStatus3 = new Label();
            labelStatus4 = new Label();
            checkBoxWordMode = new CheckBox();
            txtMinValue = new TextBox();
            txtMaxValue = new TextBox();
            lblMinValue = new Label();
            lblMaxValue = new Label();
            ((System.ComponentModel.ISupportInitialize)numericSampleSize).BeginInit();
            SuspendLayout();
            // 
            // panelVisualizer
            // 
            panelVisualizer.Location = new Point(13, 23);
            panelVisualizer.Name = "panelVisualizer";
            panelVisualizer.Size = new Size(323, 203);
            panelVisualizer.TabIndex = 0;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(525, 5);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // panelVisualizer2
            // 
            panelVisualizer2.Location = new Point(795, 24);
            panelVisualizer2.Name = "panelVisualizer2";
            panelVisualizer2.Size = new Size(323, 203);
            panelVisualizer2.TabIndex = 1;
            // 
            // panelVisualizer3
            // 
            panelVisualizer3.Location = new Point(13, 247);
            panelVisualizer3.Name = "panelVisualizer3";
            panelVisualizer3.Size = new Size(323, 203);
            panelVisualizer3.TabIndex = 1;
            // 
            // panelVisualizer4
            // 
            panelVisualizer4.Location = new Point(795, 245);
            panelVisualizer4.Name = "panelVisualizer4";
            panelVisualizer4.Size = new Size(323, 203);
            panelVisualizer4.TabIndex = 1;
            // 
            // listViewResults
            // 
            listViewResults.Columns.AddRange(new ColumnHeader[] { Algorithm, Time });
            listViewResults.Location = new Point(342, 291);
            listViewResults.Name = "listViewResults";
            listViewResults.Size = new Size(447, 134);
            listViewResults.TabIndex = 2;
            listViewResults.UseCompatibleStateImageBehavior = false;
            // 
            // Algorithm
            // 
            Algorithm.Text = "Algorithm";
            Algorithm.Width = 200;
            // 
            // Time
            // 
            Time.Text = "Time";
            Time.Width = 250;
            // 
            // checkedListAlgorithms
            // 
            checkedListAlgorithms.FormattingEnabled = true;
            checkedListAlgorithms.Items.AddRange(new object[] { "Bubble Sort", "Quick Sort", "Merge Sort", "Insertion Sort", "Heap Sort", "Selection Sort", "Bogo Sort" });
            checkedListAlgorithms.Location = new Point(506, 49);
            checkedListAlgorithms.Name = "checkedListAlgorithms";
            checkedListAlgorithms.Size = new Size(120, 112);
            checkedListAlgorithms.TabIndex = 3;
            // 
            // numericSampleSize
            // 
            numericSampleSize.Location = new Point(506, 208);
            numericSampleSize.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericSampleSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericSampleSize.Name = "numericSampleSize";
            numericSampleSize.Size = new Size(120, 23);
            numericSampleSize.TabIndex = 4;
            numericSampleSize.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // btnReset
            // 
            btnReset.Location = new Point(525, 431);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 5;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 50;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 272);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 6;
            label1.Text = "Results";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(511, 31);
            label2.Name = "label2";
            label2.Size = new Size(107, 15);
            label2.TabIndex = 7;
            label2.Text = "Sorting Algorithms";
            // 
            // Algorithm3
            // 
            Algorithm3.AutoSize = true;
            Algorithm3.Location = new Point(13, 229);
            Algorithm3.Name = "Algorithm3";
            Algorithm3.Size = new Size(61, 15);
            Algorithm3.TabIndex = 8;
            Algorithm3.Text = "Algorithm";
            // 
            // Algorithm4
            // 
            Algorithm4.AutoSize = true;
            Algorithm4.Location = new Point(798, 230);
            Algorithm4.Name = "Algorithm4";
            Algorithm4.Size = new Size(61, 15);
            Algorithm4.TabIndex = 9;
            Algorithm4.Text = "Algorithm";
            // 
            // Algorithm1
            // 
            Algorithm1.AutoSize = true;
            Algorithm1.Location = new Point(13, 5);
            Algorithm1.Name = "Algorithm1";
            Algorithm1.Size = new Size(61, 15);
            Algorithm1.TabIndex = 10;
            Algorithm1.Text = "Algorithm";
            // 
            // Algorithm2
            // 
            Algorithm2.AutoSize = true;
            Algorithm2.Location = new Point(798, 5);
            Algorithm2.Name = "Algorithm2";
            Algorithm2.Size = new Size(61, 15);
            Algorithm2.TabIndex = 9;
            Algorithm2.Text = "Algorithm";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(536, 190);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 11;
            label3.Text = "Amount";
            // 
            // comboBoxSpeed
            // 
            comboBoxSpeed.FormattingEnabled = true;
            comboBoxSpeed.Items.AddRange(new object[] { "1x", "2x", "4x", "8x", "16x", "32x", "64x", "128x", "256x", "512x", "1024x", "2048x", "4096x", "8192x" });
            comboBoxSpeed.Location = new Point(505, 251);
            comboBoxSpeed.Margin = new Padding(2);
            comboBoxSpeed.Name = "comboBoxSpeed";
            comboBoxSpeed.Size = new Size(121, 23);
            comboBoxSpeed.TabIndex = 12;
            comboBoxSpeed.Text = "1x";
            comboBoxSpeed.DropDownClosed += comboBoxSpeed_DropDownClosed;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(518, 235);
            label4.Name = "label4";
            label4.Size = new Size(89, 15);
            label4.TabIndex = 13;
            label4.Text = "Playback Speed";
            // 
            // labelStatus1
            // 
            labelStatus1.AutoSize = true;
            labelStatus1.Location = new Point(190, 5);
            labelStatus1.Margin = new Padding(2, 0, 2, 0);
            labelStatus1.Name = "labelStatus1";
            labelStatus1.Size = new Size(39, 15);
            labelStatus1.TabIndex = 14;
            labelStatus1.Text = "Status";
            // 
            // labelStatus2
            // 
            labelStatus2.AutoSize = true;
            labelStatus2.Location = new Point(935, 5);
            labelStatus2.Margin = new Padding(2, 0, 2, 0);
            labelStatus2.Name = "labelStatus2";
            labelStatus2.Size = new Size(39, 15);
            labelStatus2.TabIndex = 15;
            labelStatus2.Text = "Status";
            // 
            // labelStatus3
            // 
            labelStatus3.AutoSize = true;
            labelStatus3.Location = new Point(190, 229);
            labelStatus3.Margin = new Padding(2, 0, 2, 0);
            labelStatus3.Name = "labelStatus3";
            labelStatus3.Size = new Size(39, 15);
            labelStatus3.TabIndex = 15;
            labelStatus3.Text = "Status";
            labelStatus3.Click += labelStatus3_Click;
            // 
            // labelStatus4
            // 
            labelStatus4.AutoSize = true;
            labelStatus4.Location = new Point(935, 230);
            labelStatus4.Margin = new Padding(2, 0, 2, 0);
            labelStatus4.Name = "labelStatus4";
            labelStatus4.Size = new Size(39, 15);
            labelStatus4.TabIndex = 15;
            labelStatus4.Text = "Status";
            // 
            // checkBoxWordMode
            // 
            checkBoxWordMode.AutoSize = true;
            checkBoxWordMode.Location = new Point(518, 173);
            checkBoxWordMode.Margin = new Padding(2);
            checkBoxWordMode.Name = "checkBoxWordMode";
            checkBoxWordMode.Size = new Size(89, 19);
            checkBoxWordMode.TabIndex = 17;
            checkBoxWordMode.Text = "Word mode";
            checkBoxWordMode.UseVisualStyleBackColor = true;
            checkBoxWordMode.CheckedChanged += checkBoxWordMode_CheckedChanged;
            // 
            // txtMinValue
            // 
            txtMinValue.Location = new Point(352, 208);
            txtMinValue.Name = "txtMinValue";
            txtMinValue.Size = new Size(72, 23);
            txtMinValue.TabIndex = 18;
            txtMinValue.Text = "1";
            // 
            // txtMaxValue
            // 
            txtMaxValue.Location = new Point(430, 208);
            txtMaxValue.Name = "txtMaxValue";
            txtMaxValue.Size = new Size(70, 23);
            txtMaxValue.TabIndex = 18;
            txtMaxValue.Text = "10000";
            // 
            // lblMinValue
            // 
            lblMinValue.AutoSize = true;
            lblMinValue.Location = new Point(352, 190);
            lblMinValue.Name = "lblMinValue";
            lblMinValue.Size = new Size(59, 15);
            lblMinValue.TabIndex = 11;
            lblMinValue.Text = "Min Value";
            // 
            // lblMaxValue
            // 
            lblMaxValue.AutoSize = true;
            lblMaxValue.Location = new Point(430, 190);
            lblMaxValue.Name = "lblMaxValue";
            lblMaxValue.Size = new Size(60, 15);
            lblMaxValue.TabIndex = 11;
            lblMaxValue.Text = "Max Value";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1127, 460);
            Controls.Add(txtMaxValue);
            Controls.Add(txtMinValue);
            Controls.Add(checkBoxWordMode);
            Controls.Add(labelStatus4);
            Controls.Add(labelStatus3);
            Controls.Add(labelStatus2);
            Controls.Add(labelStatus1);
            Controls.Add(label4);
            Controls.Add(comboBoxSpeed);
            Controls.Add(lblMaxValue);
            Controls.Add(lblMinValue);
            Controls.Add(label3);
            Controls.Add(Algorithm2);
            Controls.Add(Algorithm1);
            Controls.Add(Algorithm4);
            Controls.Add(Algorithm3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnReset);
            Controls.Add(numericSampleSize);
            Controls.Add(checkedListAlgorithms);
            Controls.Add(listViewResults);
            Controls.Add(panelVisualizer4);
            Controls.Add(panelVisualizer3);
            Controls.Add(panelVisualizer2);
            Controls.Add(btnStart);
            Controls.Add(panelVisualizer);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericSampleSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DoubleBufferedPanel panelVisualizer;
        private Button btnStart;
        private DoubleBufferedPanel panelVisualizer2;
        private DoubleBufferedPanel panelVisualizer3;
        private DoubleBufferedPanel panelVisualizer4;
        private ListView listViewResults;
        private CheckedListBox checkedListAlgorithms;
        private NumericUpDown numericSampleSize;
        private Button btnReset;
        private ColumnHeader Algorithm;
        private ColumnHeader Time;
        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private Label label2;
        private Label Algorithm3;
        private Label Algorithm4;
        private Label Algorithm1;
        private Label Algorithm2;
        private Label label3;
        private ComboBox comboBoxSpeed;
        private Label label4;
        private Label labelStatus1;
        private Label labelStatus2;
        private Label labelStatus3;
        private Label labelStatus4;
        private CheckBox checkBoxWordMode;
        private TextBox txtMinValue;
        private TextBox txtMaxValue;
        private Label lblMinValue;
        private Label lblMaxValue;
    }
}
