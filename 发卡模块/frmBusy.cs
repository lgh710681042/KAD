namespace 发卡模块
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmBusy : Form
    {
        private IContainer components = null;

        public frmBusy()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmBusy));
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            this.BackgroundImage = (Image) resources.GetObject("$this.BackgroundImage");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.ClientSize = new Size(0x214, 0x119);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "frmBusy";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "frmBusy";
            base.ResumeLayout(false);
        }
    }
}

