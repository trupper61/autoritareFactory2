namespace factordictatorship
{
    partial class home
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
            this.HomeImage_picbox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.HomeImage_picbox)).BeginInit();
            this.SuspendLayout();
            // 
            // HomeImage_picbox
            // 
            this.HomeImage_picbox.Location = new System.Drawing.Point(100, 56);
            this.HomeImage_picbox.Name = "HomeImage_picbox";
            this.HomeImage_picbox.Size = new System.Drawing.Size(100, 50);
            this.HomeImage_picbox.TabIndex = 0;
            this.HomeImage_picbox.TabStop = false;
            // 
            // home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.HomeImage_picbox);
            this.Name = "home";
            this.Text = "home";
            ((System.ComponentModel.ISupportInitialize)(this.HomeImage_picbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox HomeImage_picbox;
    }
}