namespace 发卡模块
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using 发卡模块.Properties;

    public class Form6 : Form
    {
        private IContainer components = null;

        public Form6()
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
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            this.BackgroundImage = Resources.门禁管理渐变1;
            base.ClientSize = new Size(0x1ee, 0xef);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "Form6";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Form6";
            base.ResumeLayout(false);
        }
    }
}

