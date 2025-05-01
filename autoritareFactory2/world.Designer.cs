using factordictatorship.formsElement;
using System.Windows.Forms;

namespace factordictatorship
{
    partial class world
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.moneyAmount = new System.Windows.Forms.Label();
            this.noFocusButton = new factordictatorship.formsElement.NoFocusButton();
            this.openWorldFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::factordictatorship.Properties.Resources.money;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(538, 67);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 42);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // moneyAmount
            // 
            this.moneyAmount.AutoSize = true;
            this.moneyAmount.BackColor = System.Drawing.Color.Transparent;
            this.moneyAmount.Location = new System.Drawing.Point(510, 82);
            this.moneyAmount.Name = "moneyAmount";
            this.moneyAmount.Size = new System.Drawing.Size(35, 13);
            this.moneyAmount.TabIndex = 2;
            this.moneyAmount.Text = "label1";
            this.moneyAmount.Visible = false;
            // 
            // noFocusButton
            // 
            this.noFocusButton.CausesValidation = false;
            this.noFocusButton.Location = new System.Drawing.Point(63, 319);
            this.noFocusButton.Name = "noFocusButton";
            this.noFocusButton.Size = new System.Drawing.Size(100, 35);
            this.noFocusButton.TabIndex = 0;
            this.noFocusButton.Text = "Not Focusable Button";
            this.noFocusButton.UseVisualStyleBackColor = true;
            this.noFocusButton.Visible = false;
            // 
            // openWorldFile
            // 
            this.openWorldFile.DefaultExt = "world";
            this.openWorldFile.FileName = "open World";
            // 
            // world
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.moneyAmount);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.noFocusButton);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "world";
            this.Text = "FactoriaAutoritaet";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NoFocusButton noFocusButton;
        private PictureBox pictureBox1;
        private Label moneyAmount;
        private OpenFileDialog openWorldFile;
    }
}

