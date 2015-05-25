namespace 发卡模块
{
    using DAL;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class Form8 : Form
    {
        private Button btnCancel;
        private Button btnlogin;
        private ComboBox cbxName;
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private TextBox txtpassword;
        private int userid;

        public Form8()
        {
            this.InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cbxName.Text.Trim().ToString().Equals(""))
                {
                    MessageBox.Show("请输入用户名");
                    base.DialogResult = DialogResult.None;
                }
                else if (this.txtpassword.Text.Trim().ToString().Equals(""))
                {
                    MessageBox.Show("请输入密码");
                    base.DialogResult = DialogResult.None;
                }
                else
                {
                    string sql = "select * from UserInfo where UserName = @UserName  ";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@UserName", this.cbxName.Text.Trim().ToString()), new OleDbParameter("@Password", this.txtpassword.Text.Trim().ToString()) };
                        OleDbDataReader reader = DBHelper.GetReader(sql, values);
                        if (reader.Read())
                        {
                            if (reader.GetString(2).Equals(this.txtpassword.Text))
                            {
                                this.userid = int.Parse(reader.GetValue(0).ToString());
                                base.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("密码错误");
                                base.DialogResult = DialogResult.None;
                            }
                        }
                        else
                        {
                            MessageBox.Show("用户名错误");
                            base.DialogResult = DialogResult.None;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        throw exception;
                    }
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
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

        private void Form8_Load(object sender, EventArgs e)
        {
            try
            {
                string safeSql = "select * from UserInfo";
                DataTable dataSet = DBHelper.GetDataSet(safeSql);
                this.cbxName.DataSource = dataSet;
                this.cbxName.DisplayMember = "UserName";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form8));
            this.label1 = new Label();
            this.label2 = new Label();
            this.txtpassword = new TextBox();
            this.btnlogin = new Button();
            this.btnCancel = new Button();
            this.cbxName = new ComboBox();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x51, 0x23);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2f, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x51, 0x42);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2f, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密  码;";
            this.txtpassword.Location = new Point(0x8d, 0x3f);
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.PasswordChar = '*';
            this.txtpassword.Size = new Size(100, 0x15);
            this.txtpassword.TabIndex = 3;
            this.btnlogin.Location = new Point(0x53, 0x6a);
            this.btnlogin.Name = "btnlogin";
            this.btnlogin.Size = new Size(0x4b, 0x17);
            this.btnlogin.TabIndex = 4;
            this.btnlogin.Text = "登   陆";
            this.btnlogin.UseVisualStyleBackColor = true;
            this.btnlogin.Click += new EventHandler(this.btnlogin_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xa6, 0x6a);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取   消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.cbxName.AutoCompleteMode = AutoCompleteMode.Append;
            this.cbxName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.cbxName.FormattingEnabled = true;
            this.cbxName.Location = new Point(0x8d, 0x20);
            this.cbxName.Name = "cbxName";
            this.cbxName.Size = new Size(100, 20);
            this.cbxName.TabIndex = 6;
            base.AcceptButton = this.btnlogin;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x14d, 0xa3);
            base.Controls.Add(this.cbxName);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnlogin);
            base.Controls.Add(this.txtpassword);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form8";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "智能门禁脱机管理系统-用户版";
            base.Load += new EventHandler(this.Form8_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public int Userid
        {
            get
            {
                return this.userid;
            }
        }
    }
}

