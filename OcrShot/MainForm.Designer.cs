namespace OcrShot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            layout = new TableLayoutPanel();
            btnCapture = new Button();
            tbText = new TextBox();
            cmbLangauges = new ComboBox();
            cbCopyToClipboard = new CheckBox();
            progressBar = new ProgressBar();
            layout.SuspendLayout();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnCount = 2;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.Controls.Add(btnCapture, 0, 0);
            layout.Controls.Add(tbText, 0, 3);
            layout.Controls.Add(cmbLangauges, 1, 1);
            layout.Controls.Add(cbCopyToClipboard, 0, 1);
            layout.Controls.Add(progressBar, 0, 2);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.RowCount = 4;
            layout.RowStyles.Add(new RowStyle());
            layout.RowStyles.Add(new RowStyle());
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.Size = new Size(496, 161);
            layout.TabIndex = 0;
            // 
            // btnCapture
            // 
            btnCapture.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            layout.SetColumnSpan(btnCapture, 2);
            btnCapture.Location = new Point(3, 3);
            btnCapture.Name = "btnCapture";
            btnCapture.Size = new Size(490, 30);
            btnCapture.TabIndex = 0;
            btnCapture.Text = "Capture";
            btnCapture.UseVisualStyleBackColor = true;
            btnCapture.Click += OnClickCapture;
            // 
            // tbText
            // 
            layout.SetColumnSpan(tbText, 2);
            tbText.Dock = DockStyle.Fill;
            tbText.Location = new Point(3, 99);
            tbText.Multiline = true;
            tbText.Name = "tbText";
            tbText.Size = new Size(490, 59);
            tbText.TabIndex = 3;
            // 
            // cmbLangauges
            // 
            cmbLangauges.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbLangauges.FormattingEnabled = true;
            cmbLangauges.Location = new Point(251, 40);
            cmbLangauges.Name = "cmbLangauges";
            cmbLangauges.Size = new Size(242, 23);
            cmbLangauges.TabIndex = 4;
            // 
            // cbCopyToClipboard
            // 
            cbCopyToClipboard.Appearance = Appearance.Button;
            cbCopyToClipboard.AutoSize = true;
            cbCopyToClipboard.Dock = DockStyle.Fill;
            cbCopyToClipboard.Location = new Point(3, 39);
            cbCopyToClipboard.Name = "cbCopyToClipboard";
            cbCopyToClipboard.Size = new Size(242, 25);
            cbCopyToClipboard.TabIndex = 5;
            cbCopyToClipboard.Text = "Copy to Clipboard";
            cbCopyToClipboard.TextAlign = ContentAlignment.MiddleCenter;
            cbCopyToClipboard.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            layout.SetColumnSpan(progressBar, 2);
            progressBar.Dock = DockStyle.Fill;
            progressBar.Enabled = false;
            progressBar.Location = new Point(3, 70);
            progressBar.MarqueeAnimationSpeed = 0;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(490, 23);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(496, 161);
            Controls.Add(layout);
            Name = "MainForm";
            Text = "MainForm";
            layout.ResumeLayout(false);
            layout.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layout;
        private Button btnCapture;
        private TextBox tbText;
        private ComboBox cmbLangauges;
        private CheckBox cbCopyToClipboard;
        private ProgressBar progressBar;
    }
}