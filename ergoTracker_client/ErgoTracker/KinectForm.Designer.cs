namespace ErgoTracker
{
    partial class KinectForm
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
            myKinect.AllFramesReady -= myKinect_AllFramesReady;
            requestHandler.ReceivedScoreData -= HandleScoreReceived;

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
            this.kinectVideoBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.scorelabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.kinectVideoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // kinectVideoBox
            // 
            this.kinectVideoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kinectVideoBox.Location = new System.Drawing.Point(0, 0);
            this.kinectVideoBox.Name = "kinectVideoBox";
            this.kinectVideoBox.Size = new System.Drawing.Size(1278, 737);
            this.kinectVideoBox.TabIndex = 0;
            this.kinectVideoBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label1.Location = new System.Drawing.Point(991, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "Posture Score";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.label2.Location = new System.Drawing.Point(1043, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "*Updated once every 10 seconds.";
            // 
            // scorelabel
            // 
            this.scorelabel.AutoSize = true;
            this.scorelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F);
            this.scorelabel.Location = new System.Drawing.Point(1082, 55);
            this.scorelabel.Name = "scorelabel";
            this.scorelabel.Size = new System.Drawing.Size(98, 108);
            this.scorelabel.TabIndex = 3;
            this.scorelabel.Text = "0";
            this.scorelabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(999, 214);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(253, 68);
            this.button1.TabIndex = 4;
            this.button1.Text = "Send Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // KinectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 737);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.scorelabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kinectVideoBox);
            this.Name = "KinectForm";
            this.Text = "KinectForm";
            this.Load += new System.EventHandler(this.KinectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kinectVideoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox kinectVideoBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label scorelabel;
        private System.Windows.Forms.Button button1;
    }
}