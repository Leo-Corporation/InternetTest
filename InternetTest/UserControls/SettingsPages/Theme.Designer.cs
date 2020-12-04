
namespace InternetTest.UserControls.SettingsPages
{
    partial class Theme
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Theme));
            this.gunaWinSwitch1 = new Guna.UI.WinForms.GunaWinSwitch();
            this.gunaLabel4 = new Guna.UI.WinForms.GunaLabel();
            this.gunaLabel3 = new Guna.UI.WinForms.GunaLabel();
            this.gunaLabel2 = new Guna.UI.WinForms.GunaLabel();
            this.gunaGradientButton1 = new Guna.UI.WinForms.GunaGradientButton();
            this.SuspendLayout();
            // 
            // gunaWinSwitch1
            // 
            resources.ApplyResources(this.gunaWinSwitch1, "gunaWinSwitch1");
            this.gunaWinSwitch1.BackColor = System.Drawing.Color.Transparent;
            this.gunaWinSwitch1.BaseColor = System.Drawing.SystemColors.Control;
            this.gunaWinSwitch1.CheckedOffColor = System.Drawing.Color.DarkGray;
            this.gunaWinSwitch1.CheckedOnColor = System.Drawing.Color.RoyalBlue;
            this.gunaWinSwitch1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gunaWinSwitch1.FillColor = System.Drawing.Color.White;
            this.gunaWinSwitch1.Name = "gunaWinSwitch1";
            this.gunaWinSwitch1.CheckedChanged += new System.EventHandler(this.gunaWinSwitch1_CheckedChanged);
            // 
            // gunaLabel4
            // 
            resources.ApplyResources(this.gunaLabel4, "gunaLabel4");
            this.gunaLabel4.BackColor = System.Drawing.Color.Transparent;
            this.gunaLabel4.Name = "gunaLabel4";
            // 
            // gunaLabel3
            // 
            resources.ApplyResources(this.gunaLabel3, "gunaLabel3");
            this.gunaLabel3.BackColor = System.Drawing.Color.Transparent;
            this.gunaLabel3.Name = "gunaLabel3";
            // 
            // gunaLabel2
            // 
            resources.ApplyResources(this.gunaLabel2, "gunaLabel2");
            this.gunaLabel2.BackColor = System.Drawing.Color.Transparent;
            this.gunaLabel2.Name = "gunaLabel2";
            // 
            // gunaGradientButton1
            // 
            resources.ApplyResources(this.gunaGradientButton1, "gunaGradientButton1");
            this.gunaGradientButton1.Animated = true;
            this.gunaGradientButton1.AnimationHoverSpeed = 0.07F;
            this.gunaGradientButton1.AnimationSpeed = 0.03F;
            this.gunaGradientButton1.BackColor = System.Drawing.Color.Transparent;
            this.gunaGradientButton1.BaseColor1 = System.Drawing.Color.DeepSkyBlue;
            this.gunaGradientButton1.BaseColor2 = System.Drawing.Color.RoyalBlue;
            this.gunaGradientButton1.BorderColor = System.Drawing.Color.Black;
            this.gunaGradientButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gunaGradientButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaGradientButton1.FocusedColor = System.Drawing.Color.Empty;
            this.gunaGradientButton1.ForeColor = System.Drawing.Color.White;
            this.gunaGradientButton1.Image = global::InternetTest.Properties.Resources.checkmark;
            this.gunaGradientButton1.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaGradientButton1.Name = "gunaGradientButton1";
            this.gunaGradientButton1.OnHoverBaseColor1 = System.Drawing.Color.DodgerBlue;
            this.gunaGradientButton1.OnHoverBaseColor2 = System.Drawing.Color.MediumBlue;
            this.gunaGradientButton1.OnHoverBorderColor = System.Drawing.Color.Black;
            this.gunaGradientButton1.OnHoverForeColor = System.Drawing.Color.White;
            this.gunaGradientButton1.OnHoverImage = null;
            this.gunaGradientButton1.OnPressedColor = System.Drawing.Color.Black;
            this.gunaGradientButton1.Radius = 6;
            this.gunaGradientButton1.Click += new System.EventHandler(this.gunaGradientButton1_Click);
            // 
            // Theme
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gunaGradientButton1);
            this.Controls.Add(this.gunaWinSwitch1);
            this.Controls.Add(this.gunaLabel4);
            this.Controls.Add(this.gunaLabel3);
            this.Controls.Add(this.gunaLabel2);
            this.Name = "Theme";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI.WinForms.GunaWinSwitch gunaWinSwitch1;
        private Guna.UI.WinForms.GunaLabel gunaLabel4;
        private Guna.UI.WinForms.GunaLabel gunaLabel3;
        private Guna.UI.WinForms.GunaLabel gunaLabel2;
        private Guna.UI.WinForms.GunaGradientButton gunaGradientButton1;
    }
}
