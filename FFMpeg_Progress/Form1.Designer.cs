namespace FFMpeg_Progress
{
    partial class Form1
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
            this.btnStart = new System.Windows.Forms.Button();
            this.lblProgress1 = new System.Windows.Forms.Label();
            this.lblProgress2 = new System.Windows.Forms.Label();
            this.lblProgress3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progress1 = new ProgressLabel.Progress();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(361, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblProgress1
            // 
            this.lblProgress1.Location = new System.Drawing.Point(236, 51);
            this.lblProgress1.Name = "lblProgress1";
            this.lblProgress1.Size = new System.Drawing.Size(314, 16);
            this.lblProgress1.TabIndex = 1;
            this.lblProgress1.Text = "progress";
            this.lblProgress1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgress2
            // 
            this.lblProgress2.Location = new System.Drawing.Point(236, 67);
            this.lblProgress2.Name = "lblProgress2";
            this.lblProgress2.Size = new System.Drawing.Size(314, 16);
            this.lblProgress2.TabIndex = 2;
            this.lblProgress2.Text = "progress1";
            this.lblProgress2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgress3
            // 
            this.lblProgress3.Location = new System.Drawing.Point(236, 83);
            this.lblProgress3.Name = "lblProgress3";
            this.lblProgress3.Size = new System.Drawing.Size(314, 16);
            this.lblProgress3.TabIndex = 3;
            this.lblProgress3.Text = "progress2";
            this.lblProgress3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(239, 131);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(311, 23);
            this.progressBar.TabIndex = 4;
            // 
            // progress1
            // 
            this.progress1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.progress1.Location = new System.Drawing.Point(239, 185);
            this.progress1.Name = "progress1";
            this.progress1.Size = new System.Drawing.Size(311, 10);
            this.progress1.TabIndex = 5;
            this.progress1.Value = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progress1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblProgress3);
            this.Controls.Add(this.lblProgress2);
            this.Controls.Add(this.lblProgress1);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblProgress1;
        private System.Windows.Forms.Label lblProgress2;
        private System.Windows.Forms.Label lblProgress3;
        private System.Windows.Forms.ProgressBar progressBar;
        private ProgressLabel.Progress progress1;
    }
}

