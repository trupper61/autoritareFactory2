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
            this.noFocusButton = new NoFocusButton();
            this.SuspendLayout();
            // 
            // noSelButton
            // 
            this.noFocusButton.CausesValidation = false;
            this.noFocusButton.Location = new System.Drawing.Point(271, 90);
            this.noFocusButton.Name = "noFocusButton";
            this.noFocusButton.Size = new System.Drawing.Size(100, 35);
            this.noFocusButton.TabIndex = 0;
            this.noFocusButton.Text = "Not Focusable Button";
            this.noFocusButton.UseVisualStyleBackColor = true;
            // 
            // world
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.noFocusButton);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "world";
            this.Text = "FactoriaAutoritaet";
            this.ResumeLayout(false);

        }

        #endregion

        private Button noFocusButton;
    }
}

