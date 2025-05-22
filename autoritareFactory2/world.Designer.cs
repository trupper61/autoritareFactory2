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
            this.openWorldFile = new System.Windows.Forms.OpenFileDialog();
            this.noFocusButton = new factordictatorship.formsElement.NoFocusButton();
            this.SuspendLayout();
            // 
            // openWorldFile
            // 
            this.openWorldFile.DefaultExt = "world";
            this.openWorldFile.FileName = "open World";
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
            this.Controls.Add(this.noFocusButton);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "world";
            this.Text = "FactoriaAutoritaet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NoFocusButton noFocusButton;

        private OpenFileDialog openWorldFile;
    }
}

