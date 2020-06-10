namespace InternetTest.Forms
{
    partial class AboutForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.gunaElipse1 = new Guna.UI.WinForms.GunaElipse(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.gunaAdvenceTileButton1 = new Guna.UI.WinForms.GunaAdvenceTileButton();
            this.gunaPictureBox1 = new Guna.UI.WinForms.GunaPictureBox();
            this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
            this.gunaDragControl2 = new Guna.UI.WinForms.GunaDragControl(this.components);
            this.gunaDragControl1 = new Guna.UI.WinForms.GunaDragControl(this.components);
            this.gunaLabel2 = new Guna.UI.WinForms.GunaLabel();
            this.gunaLabel3 = new Guna.UI.WinForms.GunaLabel();
            this.gunaPictureBox2 = new Guna.UI.WinForms.GunaPictureBox();
            this.gunaGradientButton2 = new Guna.UI.WinForms.GunaGradientButton();
            this.gunaLinkLabel1 = new Guna.UI.WinForms.GunaLinkLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // gunaElipse1
            // 
            this.gunaElipse1.Radius = 6;
            this.gunaElipse1.TargetControl = this;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.gunaAdvenceTileButton1);
            this.panel1.Controls.Add(this.gunaPictureBox1);
            this.panel1.Controls.Add(this.gunaLabel1);
            this.panel1.Name = "panel1";
            // 
            // gunaAdvenceTileButton1
            // 
            resources.ApplyResources(this.gunaAdvenceTileButton1, "gunaAdvenceTileButton1");
            this.gunaAdvenceTileButton1.Animated = true;
            this.gunaAdvenceTileButton1.AnimationHoverSpeed = 0.07F;
            this.gunaAdvenceTileButton1.AnimationSpeed = 0.03F;
            this.gunaAdvenceTileButton1.BaseColor = System.Drawing.Color.Transparent;
            this.gunaAdvenceTileButton1.BorderColor = System.Drawing.Color.Black;
            this.gunaAdvenceTileButton1.CheckedBaseColor = System.Drawing.Color.Gray;
            this.gunaAdvenceTileButton1.CheckedBorderColor = System.Drawing.Color.Black;
            this.gunaAdvenceTileButton1.CheckedForeColor = System.Drawing.Color.White;
            this.gunaAdvenceTileButton1.CheckedImage = ((System.Drawing.Image)(resources.GetObject("gunaAdvenceTileButton1.CheckedImage")));
            this.gunaAdvenceTileButton1.CheckedLineColor = System.Drawing.Color.DimGray;
            this.gunaAdvenceTileButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaAdvenceTileButton1.FocusedColor = System.Drawing.Color.Empty;
            this.gunaAdvenceTileButton1.ForeColor = System.Drawing.Color.White;
            this.gunaAdvenceTileButton1.Image = global::InternetTest.Properties.Resources.icons8_delete_100px_1;
            this.gunaAdvenceTileButton1.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaAdvenceTileButton1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(58)))), ((int)(((byte)(170)))));
            this.gunaAdvenceTileButton1.Name = "gunaAdvenceTileButton1";
            this.gunaAdvenceTileButton1.OnHoverBaseColor = System.Drawing.Color.Red;
            this.gunaAdvenceTileButton1.OnHoverBorderColor = System.Drawing.Color.Black;
            this.gunaAdvenceTileButton1.OnHoverForeColor = System.Drawing.Color.White;
            this.gunaAdvenceTileButton1.OnHoverImage = global::InternetTest.Properties.Resources.icons8_delete_32px;
            this.gunaAdvenceTileButton1.OnHoverLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(58)))), ((int)(((byte)(170)))));
            this.gunaAdvenceTileButton1.OnPressedColor = System.Drawing.Color.Black;
            this.gunaAdvenceTileButton1.Click += new System.EventHandler(this.gunaAdvenceTileButton1_Click);
            // 
            // gunaPictureBox1
            // 
            resources.ApplyResources(this.gunaPictureBox1, "gunaPictureBox1");
            this.gunaPictureBox1.BaseColor = System.Drawing.Color.White;
            this.gunaPictureBox1.Image = global::InternetTest.Properties.Resources.InternetTestLogo1;
            this.gunaPictureBox1.Name = "gunaPictureBox1";
            this.gunaPictureBox1.TabStop = false;
            // 
            // gunaLabel1
            // 
            resources.ApplyResources(this.gunaLabel1, "gunaLabel1");
            this.gunaLabel1.Name = "gunaLabel1";
            // 
            // gunaDragControl2
            // 
            this.gunaDragControl2.TargetControl = this.gunaLabel1;
            // 
            // gunaDragControl1
            // 
            this.gunaDragControl1.TargetControl = this.panel1;
            // 
            // gunaLabel2
            // 
            resources.ApplyResources(this.gunaLabel2, "gunaLabel2");
            this.gunaLabel2.BackColor = System.Drawing.Color.Transparent;
            this.gunaLabel2.Name = "gunaLabel2";
            // 
            // gunaLabel3
            // 
            resources.ApplyResources(this.gunaLabel3, "gunaLabel3");
            this.gunaLabel3.BackColor = System.Drawing.Color.Transparent;
            this.gunaLabel3.Name = "gunaLabel3";
            // 
            // gunaPictureBox2
            // 
            resources.ApplyResources(this.gunaPictureBox2, "gunaPictureBox2");
            this.gunaPictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.gunaPictureBox2.BaseColor = System.Drawing.Color.White;
            this.gunaPictureBox2.Image = global::InternetTest.Properties.Resources.InternetTestLogo1;
            this.gunaPictureBox2.Name = "gunaPictureBox2";
            this.gunaPictureBox2.TabStop = false;
            // 
            // gunaGradientButton2
            // 
            resources.ApplyResources(this.gunaGradientButton2, "gunaGradientButton2");
            this.gunaGradientButton2.Animated = true;
            this.gunaGradientButton2.AnimationHoverSpeed = 0.07F;
            this.gunaGradientButton2.AnimationSpeed = 0.03F;
            this.gunaGradientButton2.BackColor = System.Drawing.Color.Transparent;
            this.gunaGradientButton2.BaseColor1 = System.Drawing.Color.DeepSkyBlue;
            this.gunaGradientButton2.BaseColor2 = System.Drawing.Color.RoyalBlue;
            this.gunaGradientButton2.BorderColor = System.Drawing.Color.Black;
            this.gunaGradientButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gunaGradientButton2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaGradientButton2.FocusedColor = System.Drawing.Color.Empty;
            this.gunaGradientButton2.ForeColor = System.Drawing.Color.White;
            this.gunaGradientButton2.Image = global::InternetTest.Properties.Resources.Update;
            this.gunaGradientButton2.ImageSize = new System.Drawing.Size(18, 18);
            this.gunaGradientButton2.Name = "gunaGradientButton2";
            this.gunaGradientButton2.OnHoverBaseColor1 = System.Drawing.Color.DodgerBlue;
            this.gunaGradientButton2.OnHoverBaseColor2 = System.Drawing.Color.MediumBlue;
            this.gunaGradientButton2.OnHoverBorderColor = System.Drawing.Color.Black;
            this.gunaGradientButton2.OnHoverForeColor = System.Drawing.Color.White;
            this.gunaGradientButton2.OnHoverImage = null;
            this.gunaGradientButton2.OnPressedColor = System.Drawing.Color.Black;
            this.gunaGradientButton2.Radius = 6;
            this.gunaGradientButton2.Click += new System.EventHandler(this.gunaGradientButton2_Click);
            // 
            // gunaLinkLabel1
            // 
            resources.ApplyResources(this.gunaLinkLabel1, "gunaLinkLabel1");
            this.gunaLinkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.gunaLinkLabel1.LinkColor = System.Drawing.Color.RoyalBlue;
            this.gunaLinkLabel1.Name = "gunaLinkLabel1";
            this.gunaLinkLabel1.TabStop = true;
            this.gunaLinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gunaLinkLabel1_LinkClicked);
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gunaLinkLabel1);
            this.Controls.Add(this.gunaGradientButton2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gunaLabel3);
            this.Controls.Add(this.gunaPictureBox2);
            this.Controls.Add(this.gunaLabel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI.WinForms.GunaElipse gunaElipse1;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI.WinForms.GunaAdvenceTileButton gunaAdvenceTileButton1;
        private Guna.UI.WinForms.GunaPictureBox gunaPictureBox1;
        private Guna.UI.WinForms.GunaLabel gunaLabel1;
        private Guna.UI.WinForms.GunaDragControl gunaDragControl2;
        private Guna.UI.WinForms.GunaDragControl gunaDragControl1;
        private Guna.UI.WinForms.GunaLabel gunaLabel3;
        private Guna.UI.WinForms.GunaPictureBox gunaPictureBox2;
        private Guna.UI.WinForms.GunaLabel gunaLabel2;
        private Guna.UI.WinForms.GunaGradientButton gunaGradientButton2;
        private Guna.UI.WinForms.GunaLinkLabel gunaLinkLabel1;
    }
}