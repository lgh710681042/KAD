namespace 发卡模块
{
    using DAL;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;
    using 发卡模块.Properties;

    public class OperatorUserManagement : Form
    {
        private Button btnAddCancel;
        private Button btnAddUser;
        private Button btnUpdateCancel;
        private Button btnUpdateUser;
        private ComboBox ComboBox_UserPermissions;
        private IContainer components = null;
        private DataGridView dataGridView1;
        private Label Label_UserPermissions;
        private Label label1;
        private Label label2;
        private Label label3;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox toolStripTextBox1;
        private TextBox txtName;
        private TextBox txtPassword;

        public OperatorUserManagement()
        {
            this.InitializeComponent();
        }

        private void btnAddCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.txtName.Visible = false;
                this.txtPassword.Visible = false;
                this.btnAddUser.Visible = false;
                this.btnAddCancel.Visible = false;
                this.Label_UserPermissions.Visible = false;
                this.ComboBox_UserPermissions.Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtName.Text.Equals("") || this.txtPassword.Text.Equals(""))
                {
                    MessageBox.Show("请输入用户名或密码");
                }
                else
                {
                    string sql = "select * from UserInfo where UserName = @UserName";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@UserName", this.txtName.Text) };
                        if (DBHelper.GetDataSet(sql, values).Rows.Count == 0)
                        {
                            string str2 = "insert into UserInfo (UserName,Passwrod,UserPermission)values (@UserName,@Passwrod,@UserPermission)";
                            try
                            {
                                OleDbParameter[] parameterArray2 = new OleDbParameter[] { new OleDbParameter("@UserName", this.txtName.Text), new OleDbParameter("@Passwrod", this.txtPassword.Text), new OleDbParameter("@UserPermission", this.ComboBox_UserPermissions.SelectedItem.ToString()) };
                                DBHelper.ExecuteCommand(str2, parameterArray2);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception.Message);
                                throw exception;
                            }
                        }
                        else
                        {
                            MessageBox.Show("此用户名已经存在,请用别的用户名");
                            return;
                        }
                    }
                    catch
                    {
                    }
                    MessageBox.Show("添加成功");
                    this.GetUserInfo();
                    this.label1.Visible = false;
                    this.label2.Visible = false;
                    this.txtName.Visible = false;
                    this.txtPassword.Visible = false;
                    this.btnAddUser.Visible = false;
                    this.btnAddCancel.Visible = false;
                    this.ComboBox_UserPermissions.Visible = false;
                    this.Label_UserPermissions.Visible = false;
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        private void btnUpdateCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.txtName.Visible = false;
                this.txtPassword.Visible = false;
                this.btnUpdateCancel.Visible = false;
                this.btnUpdateUser.Visible = false;
                this.Label_UserPermissions.Visible = false;
                this.ComboBox_UserPermissions.Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.label3.Text.Equals("") || this.txtName.Text.Equals(""))
                {
                    MessageBox.Show("请至少选择一条数据");
                }
                else
                {
                    string sql = "UPDATE UserInfo SET UserName = @UserName,Passwrod = @Passwrod, UserPermission = @UserPermission WHERE ID = @ID";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@UserName", this.txtName.Text), new OleDbParameter("@Passwrod", this.txtPassword.Text), new OleDbParameter("@UserPermission", this.ComboBox_UserPermissions.SelectedItem.ToString()), new OleDbParameter("@ID", int.Parse(this.label3.Text.ToString())) };
                        if (MessageBox.Show("确定要修改用户名为" + this.txtName.Text + "的这条数据", "修改", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                        {
                            DBHelper.ExecuteCommand(sql, values);
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        throw exception;
                    }
                    this.GetUserInfo();
                    this.label1.Visible = false;
                    this.label2.Visible = false;
                    this.txtName.Visible = false;
                    this.txtPassword.Visible = false;
                    this.btnUpdateCancel.Visible = false;
                    this.btnUpdateUser.Visible = false;
                    this.Label_UserPermissions.Visible = false;
                    this.ComboBox_UserPermissions.Visible = false;
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.txtName.Text = this.dataGridView1.CurrentRow.Cells["UserName"].Value.ToString();
                this.txtPassword.Text = this.dataGridView1.CurrentRow.Cells["Passwrod"].Value.ToString();
                this.label3.Text = this.dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                this.ComboBox_UserPermissions.SelectedIndex = this.ComboBox_UserPermissions.FindString(this.dataGridView1.CurrentRow.Cells["UserPermission"].Value.ToString());
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

        private void Form9_Load(object sender, EventArgs e)
        {
            this.GetUserInfo();
        }

        private void GetUserInfo()
        {
            try
            {
                string safeSql = "select * from UserInfo  ";
                this.dataGridView1.DataSource = DBHelper.GetDataSet(safeSql);
                this.dataGridView1.Columns["UserName"].HeaderText = "用户名";
                this.dataGridView1.Columns["Passwrod"].HeaderText = "密码";
                this.dataGridView1.Columns["UserPermission"].HeaderText = "用户权限";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(OperatorUserManagement));
            this.btnAddUser = new Button();
            this.txtName = new TextBox();
            this.dataGridView1 = new DataGridView();
            this.toolStrip1 = new ToolStrip();
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripButton2 = new ToolStripButton();
            this.toolStripButton3 = new ToolStripButton();
            this.toolStripLabel1 = new ToolStripLabel();
            this.toolStripTextBox1 = new ToolStripTextBox();
            this.toolStripButton4 = new ToolStripButton();
            this.txtPassword = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.btnUpdateUser = new Button();
            this.label3 = new Label();
            this.btnAddCancel = new Button();
            this.btnUpdateCancel = new Button();
            this.Label_UserPermissions = new Label();
            this.ComboBox_UserPermissions = new ComboBox();
            ((ISupportInitialize) this.dataGridView1).BeginInit();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.btnAddUser.Location = new Point(0x14f, 0xce);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new Size(0x4b, 0x17);
            this.btnAddUser.TabIndex = 0;
            this.btnAddUser.Text = "添  加";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Visible = false;
            this.btnAddUser.Click += new EventHandler(this.btnAddUser_Click);
            this.txtName.Location = new Point(0x41, 0xd0);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(100, 0x15);
            this.txtName.TabIndex = 1;
            this.txtName.Visible = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(0, 0x27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 0x17;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new Size(0x1ba, 0xa1);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripLabel1, this.toolStripTextBox1, this.toolStripButton4 });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x1ba, 40);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripButton1.Image = Resources._200934163052197;
            this.toolStripButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new Size(0x24, 0x25);
            this.toolStripButton1.Text = "增加";
            this.toolStripButton1.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton1.Click += new EventHandler(this.toolStripButton1_Click);
            this.toolStripButton2.Image = (Image) resources.GetObject("toolStripButton2.Image");
            this.toolStripButton2.ImageTransparentColor = Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new Size(0x24, 0x25);
            this.toolStripButton2.Text = "修改";
            this.toolStripButton2.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton2.Click += new EventHandler(this.toolStripButton2_Click);
            this.toolStripButton3.Image = (Image) resources.GetObject("toolStripButton3.Image");
            this.toolStripButton3.ImageTransparentColor = Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new Size(0x24, 0x25);
            this.toolStripButton3.Text = "删除";
            this.toolStripButton3.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton3.Click += new EventHandler(this.toolStripButton3_Click);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0x2c, 0x25);
            this.toolStripLabel1.Text = "查询值";
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new Size(100, 40);
            this.toolStripButton4.Image = Resources.Search1;
            this.toolStripButton4.ImageTransparentColor = Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new Size(0x24, 0x25);
            this.toolStripButton4.Text = "查询";
            this.toolStripButton4.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton4.Click += new EventHandler(this.toolStripButton4_Click);
            this.txtPassword.Location = new Point(0xe5, 0xd0);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new Size(100, 0x15);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Visible = false;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0xd3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2f, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "用户名:";
            this.label1.Visible = false;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xb0, 0xd3);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2f, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "密  码:";
            this.label2.Visible = false;
            this.btnUpdateUser.Location = new Point(0x14f, 0xcf);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new Size(0x4b, 0x17);
            this.btnUpdateUser.TabIndex = 3;
            this.btnUpdateUser.Text = "修  改";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Visible = false;
            this.btnUpdateUser.Click += new EventHandler(this.btnUpdateUser_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(12, 0xec);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x29, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "label3";
            this.label3.Visible = false;
            this.btnAddCancel.Location = new Point(0x14f, 0xeb);
            this.btnAddCancel.Name = "btnAddCancel";
            this.btnAddCancel.Size = new Size(0x4b, 0x17);
            this.btnAddCancel.TabIndex = 9;
            this.btnAddCancel.Text = "取  消";
            this.btnAddCancel.UseVisualStyleBackColor = true;
            this.btnAddCancel.Visible = false;
            this.btnAddCancel.Click += new EventHandler(this.btnAddCancel_Click);
            this.btnUpdateCancel.Location = new Point(0x14f, 0xec);
            this.btnUpdateCancel.Name = "btnUpdateCancel";
            this.btnUpdateCancel.Size = new Size(0x4b, 0x17);
            this.btnUpdateCancel.TabIndex = 10;
            this.btnUpdateCancel.Text = "取  消";
            this.btnUpdateCancel.UseVisualStyleBackColor = true;
            this.btnUpdateCancel.Visible = false;
            this.btnUpdateCancel.Click += new EventHandler(this.btnUpdateCancel_Click);
            this.Label_UserPermissions.AutoSize = true;
            this.Label_UserPermissions.Location = new Point(13, 0xeb);
            this.Label_UserPermissions.Name = "Label_UserPermissions";
            this.Label_UserPermissions.Size = new Size(0x23, 12);
            this.Label_UserPermissions.TabIndex = 11;
            this.Label_UserPermissions.Text = "权限:";
            this.Label_UserPermissions.Visible = false;
            this.ComboBox_UserPermissions.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ComboBox_UserPermissions.FormattingEnabled = true;
            this.ComboBox_UserPermissions.Items.AddRange(new object[] { "超级用户", "受限用户" });
            this.ComboBox_UserPermissions.Location = new Point(0x41, 0xe7);
            this.ComboBox_UserPermissions.Name = "ComboBox_UserPermissions";
            this.ComboBox_UserPermissions.Size = new Size(100, 20);
            this.ComboBox_UserPermissions.TabIndex = 12;
            this.ComboBox_UserPermissions.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x1ba, 0x107);
            base.Controls.Add(this.ComboBox_UserPermissions);
            base.Controls.Add(this.Label_UserPermissions);
            base.Controls.Add(this.btnUpdateCancel);
            base.Controls.Add(this.btnAddCancel);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.btnUpdateUser);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.txtPassword);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.dataGridView1);
            base.Controls.Add(this.txtName);
            base.Controls.Add(this.btnAddUser);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form9";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "用户管理";
            base.Load += new EventHandler(this.Form9_Load);
            ((ISupportInitialize) this.dataGridView1).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.txtName.Visible = true;
                this.txtPassword.Visible = true;
                this.btnAddUser.Visible = true;
                this.btnAddCancel.Visible = true;
                this.Label_UserPermissions.Visible = true;
                this.ComboBox_UserPermissions.Visible = true;
                this.ComboBox_UserPermissions.SelectedIndex = 0;
                this.txtName.Text = "";
                this.txtPassword.Text = "";
                this.btnUpdateUser.Visible = false;
                this.btnUpdateCancel.Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.txtName.Visible = true;
                this.txtPassword.Visible = true;
                this.btnUpdateCancel.Visible = true;
                this.btnUpdateUser.Visible = true;
                this.Label_UserPermissions.Visible = true;
                this.ComboBox_UserPermissions.Visible = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "DELETE FROM UserInfo WHERE ID = @ID";
                if (this.label3.Text.Equals("") || this.txtName.Text.Equals(""))
                {
                    MessageBox.Show("请选择一条数据");
                }
                else
                {
                    int num = int.Parse(this.label3.Text);
                    if (num == 1)
                    {
                        MessageBox.Show("此条记录是用来保存超级用户的不能删除,只能改");
                    }
                    else
                    {
                        try
                        {
                            OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@ID", num) };
                            if (MessageBox.Show("确定要修改用户名为" + this.txtName.Text + "的这条数据", "删除", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                            {
                                DBHelper.ExecuteCommand(sql, values);
                            }
                            else
                            {
                                return;
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                            throw exception;
                        }
                        this.GetUserInfo();
                    }
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.toolStripTextBox1.Text.Trim().ToString();
                string safeSql = "select * from UserInfo where UserName like '%" + str + "%' or Passwrod like '%" + str + "%'";
                this.dataGridView1.DataSource = DBHelper.GetDataSet(safeSql);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }
    }
}

