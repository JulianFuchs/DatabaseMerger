namespace DatabaseMerger
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
            this.Title = new System.Windows.Forms.Label();
            this.StartMergingButton = new System.Windows.Forms.Button();
            this.TimeRunningLabel = new System.Windows.Forms.Label();
            this.StatsThisRunLabel = new System.Windows.Forms.Label();
            this.CurrentIdLabel = new System.Windows.Forms.Label();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.MergerProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Rockwell", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(213, 57);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(314, 42);
            this.Title.TabIndex = 0;
            this.Title.Text = "Database Merger";
            // 
            // StartMergingButton
            // 
            this.StartMergingButton.Location = new System.Drawing.Point(223, 198);
            this.StartMergingButton.Name = "StartMergingButton";
            this.StartMergingButton.Size = new System.Drawing.Size(280, 56);
            this.StartMergingButton.TabIndex = 1;
            this.StartMergingButton.Text = "Start Merging";
            this.StartMergingButton.UseVisualStyleBackColor = true;
            this.StartMergingButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.StartMergingButton_MouseClick);
            // 
            // TimeRunningLabel
            // 
            this.TimeRunningLabel.AutoSize = true;
            this.TimeRunningLabel.Location = new System.Drawing.Point(610, 198);
            this.TimeRunningLabel.Name = "TimeRunningLabel";
            this.TimeRunningLabel.Size = new System.Drawing.Size(80, 13);
            this.TimeRunningLabel.TabIndex = 2;
            this.TimeRunningLabel.Text = "Time running: 0";
            // 
            // StatsThisRunLabel
            // 
            this.StatsThisRunLabel.AutoSize = true;
            this.StatsThisRunLabel.Font = new System.Drawing.Font("Rockwell", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatsThisRunLabel.Location = new System.Drawing.Point(606, 130);
            this.StatsThisRunLabel.Name = "StatsThisRunLabel";
            this.StatsThisRunLabel.Size = new System.Drawing.Size(239, 39);
            this.StatsThisRunLabel.TabIndex = 3;
            this.StatsThisRunLabel.Text = "Stats this Run:";
            // 
            // CurrentIdLabel
            // 
            this.CurrentIdLabel.AutoSize = true;
            this.CurrentIdLabel.Location = new System.Drawing.Point(610, 220);
            this.CurrentIdLabel.Name = "CurrentIdLabel";
            this.CurrentIdLabel.Size = new System.Drawing.Size(120, 13);
            this.CurrentIdLabel.TabIndex = 4;
            this.CurrentIdLabel.Text = "Currently looking at id: 0";
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(610, 241);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(71, 13);
            this.ProgressLabel.TabIndex = 5;
            this.ProgressLabel.Text = "Progress: 0/0";
            // 
            // MergerProgressBar
            // 
            this.MergerProgressBar.Location = new System.Drawing.Point(88, 316);
            this.MergerProgressBar.Name = "MergerProgressBar";
            this.MergerProgressBar.Size = new System.Drawing.Size(526, 54);
            this.MergerProgressBar.Step = 1;
            this.MergerProgressBar.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(974, 472);
            this.Controls.Add(this.MergerProgressBar);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.CurrentIdLabel);
            this.Controls.Add(this.StatsThisRunLabel);
            this.Controls.Add(this.TimeRunningLabel);
            this.Controls.Add(this.StartMergingButton);
            this.Controls.Add(this.Title);
            this.Name = "MainForm";
            this.Text = "Database Merger";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button StartMergingButton;
        private System.Windows.Forms.Label TimeRunningLabel;
        private System.Windows.Forms.Label StatsThisRunLabel;
        private System.Windows.Forms.Label CurrentIdLabel;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.ProgressBar MergerProgressBar;
    }
}

