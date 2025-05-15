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
            this.moneyAmount = new System.Windows.Forms.Label();
            this.openWorldFile = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.noFocusButton = new factordictatorship.formsElement.NoFocusButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // moneyAmount
            // 
            this.moneyAmount.AutoSize = true;
            this.moneyAmount.BackColor = System.Drawing.Color.Transparent;
            this.moneyAmount.Location = new System.Drawing.Point(680, 101);
            this.moneyAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.moneyAmount.Name = "moneyAmount";
            this.moneyAmount.Size = new System.Drawing.Size(44, 16);
            this.moneyAmount.TabIndex = 2;
            this.moneyAmount.Text = "label1";
            // 
            // openWorldFile
            // 
            this.openWorldFile.DefaultExt = "world";
            this.openWorldFile.FileName = "open World";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::factordictatorship.Properties.Resources.money;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(717, 82);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(67, 52);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // noFocusButton
            // 
            this.noFocusButton.CausesValidation = false;
            this.noFocusButton.Location = new System.Drawing.Point(84, 393);
            this.noFocusButton.Margin = new System.Windows.Forms.Padding(4);
            this.noFocusButton.Name = "noFocusButton";
            this.noFocusButton.Size = new System.Drawing.Size(133, 43);
            this.noFocusButton.TabIndex = 0;
            this.noFocusButton.Text = "Not Focusable Button";
            this.noFocusButton.UseVisualStyleBackColor = true;
            this.noFocusButton.Visible = false;
            // 
            // world
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.moneyAmount);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.noFocusButton);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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

