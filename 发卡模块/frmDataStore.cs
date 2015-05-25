namespace 发卡模块
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class frmDataStore : Form
    {
        private Button btnClose;
        private Button btnDStore;
        private Button btnpart;
        private IContainer components = null;
        private FolderBrowserDialog fbDialogFile;
        private GroupBox groupBox1;
        private Label label1;
        private TextBox txtpath;

        public frmDataStore()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDStore_Click(object sender, EventArgs e)
        {
            Exception exception;
            string str = ((DateTime.Now.Hour.ToString() + "时") + DateTime.Now.Minute.ToString() + "分") + DateTime.Now.Second.ToString() + "秒";
            str = DateTime.Now.ToLongDateString() + str;
            string sourceFileName = Application.StartupPath + @"\SentCard.mdb";
            string destFileName = this.txtpath.Text.Trim() + str + " - 卡数据.bak";
            string str4 = this.txtpath.Text.Trim() + str + " - 设备列表.dak";
            try
            {
                if (File.Exists(this.txtpath.Text.Trim() + str + ".bak") && File.Exists(this.txtpath.Text.Trim() + str + ".dak"))
                {
                    MessageBox.Show("该文件已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.txtpath.Text = "";
                    this.txtpath.Focus();
                }
                else
                {
                    try
                    {
                        File.Copy(sourceFileName, destFileName, true);
                        File.Copy(Application.StartupPath + @"\address.xml", str4, true);
                        MessageBox.Show("数据备份成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        base.Close();
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void btnpart_Click(object sender, EventArgs e)
        {
            try
            {
                this.fbDialogFile.ShowDialog();
                this.txtpath.Text = this.fbDialogFile.SelectedPath.ToString().Trim() + @"\";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmDataStore_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmDataStore));
            this.groupBox1 = new GroupBox();
            this.btnpart = new Button();
            this.txtpath = new TextBox();
            this.label1 = new Label();
            this.btnDStore = new Button();
            this.btnClose = new Button();
            this.fbDialogFile = new FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.btnpart);
            this.groupBox1.Controls.Add(this.txtpath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x147, 0x57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据备份";
            this.btnpart.Location = new Point(0xf6, 0x31);
            this.btnpart.Name = "btnpart";
            this.btnpart.Size = new Size(0x4b, 0x17);
            this.btnpart.TabIndex = 2;
            this.btnpart.Text = "选择路径";
            this.btnpart.UseVisualStyleBackColor = true;
            this.btnpart.Click += new EventHandler(this.btnpart_Click);
            this.txtpath.Location = new Point(0x18, 0x31);
            this.txtpath.Name = "txtpath";
            this.txtpath.Size = new Size(0xd8, 0x15);
            this.txtpath.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x16, 0x1d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "备份文件路径";
            this.btnDStore.Location = new Point(0x51, 0x69);
            this.btnDStore.Name = "btnDStore";
            this.btnDStore.Size = new Size(0x4b, 0x17);
            this.btnDStore.TabIndex = 3;
            this.btnDStore.Text = "数据备份";
            this.btnDStore.UseVisualStyleBackColor = true;
            this.btnDStore.Click += new EventHandler(this.btnDStore_Click);
            this.btnClose.Location = new Point(210, 0x69);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x4b, 0x17);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "退  出";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.fbDialogFile.SelectedPath = @"D:\";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x15f, 0x93);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.btnDStore);
            base.Controls.Add(this.groupBox1);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "frmDataStore";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "数据库备份";
            base.Load += new EventHandler(this.frmDataStore_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

