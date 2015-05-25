namespace 发卡模块
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ChangeTime : Form
    {
        private Button button1;
        private IContainer components = null;
        public DateTimePicker DeviceDateTime;
        private Label label1;

        public ChangeTime()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                base.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
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
            this.label1 = new Label();
            this.DeviceDateTime = new DateTimePicker();
            this.button1 = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "请选择时间";
            this.DeviceDateTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.DeviceDateTime.Format = DateTimePickerFormat.Custom;
            this.DeviceDateTime.Location = new Point(12, 0x1c);
            this.DeviceDateTime.Name = "DeviceDateTime";
            this.DeviceDateTime.Size = new Size(0x9d, 0x15);
            this.DeviceDateTime.TabIndex = 1;
            this.button1.Location = new Point(0x3a, 0x3d);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            //base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0xb6, 0x60);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.DeviceDateTime);
            base.Controls.Add(this.label1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ChangeTime";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "母卡校准时间";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

