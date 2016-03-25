namespace ErgoTracker
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
            this.Maxubi = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // Maxubi
            // 
            this.Maxubi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Maxubi.Location = new System.Drawing.Point(0, 0);
            this.Maxubi.MinimumSize = new System.Drawing.Size(20, 20);
            this.Maxubi.Name = "Maxubi";
            this.Maxubi.Size = new System.Drawing.Size(2048, 1398);
            this.Maxubi.TabIndex = 0;
            System.Uri uri = new System.Uri("http://maxubi.herokuapp.com/login", System.UriKind.Absolute);
            this.Maxubi.Navigate(uri);

            this.Maxubi.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(maxUbi_DocumentNavigating);
            this.Maxubi.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(maxUbi_DocumentCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2048, 1398);
            this.Controls.Add(this.Maxubi);
            this.Name = "Form1";
            this.Text = "ErgoTracker";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser Maxubi;
    }
}

