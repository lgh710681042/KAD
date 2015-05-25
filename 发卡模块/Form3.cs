namespace 发卡模块
{
    using DAL;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class Form3 : Form
    {
        private Button btnAddGroup;
        private Button btnClose;
        private Button btnClosePanel;
        private Button btnDeleteGropu;
        private Button btnleft;
        private Button btnleftall;
        private Button btnNewGroup;
        private Button btnRefresh;
        private Button btnright;
        private Button btnrightall;
        private Button btnUpdateGroup;
        private Button btnUpdateGroups;
        private ComboBox cbxSelectGroup;
        private IContainer components;
        private int count;
        private DataGridView dvwViewGroups;
        private Label label1;
        private ListBox listBox1;
        private ListBox listBox2;
        private Panel panel1;
        private ToolTip toolTip1;
        private TextBox txtUpdateGroups;
        private TextBox txtViewGroupName;

        public Form3()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public Form3(int count)
        {
            this.components = null;
            this.count = count;
            this.InitializeComponent();
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtViewGroupName.Text.Equals(""))
                {
                    MessageBox.Show("请输入要添加的组名");
                }
                else
                {
                    string sql = "select * from Groups where GroupName = @GroupName";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("GroupName", this.txtViewGroupName.Text.Trim().ToString()) };
                        if (DBHelper.GetDataSet(sql, values).Rows.Count == 0)
                        {
                            string str2 = "insert into Groups (GroupName) values (@GroupName)";
                            try
                            {
                                OleDbParameter[] parameterArray2 = new OleDbParameter[] { new OleDbParameter("GroupName", this.txtViewGroupName.Text.Trim().ToString()) };
                                DBHelper.GetDataSet(str2, parameterArray2);
                            }
                            catch (Exception)
                            {
                            }
                            MessageBox.Show("添加成功");
                            this.txtViewGroupName.Text = "";
                            this.loadGroup();
                        }
                        else
                        {
                            MessageBox.Show("这个组名已经存在");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnAddGroup_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.toolTip1.SetToolTip(this.btnUpdateGroup, "请在左边的文本框输入要添加的组名");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
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

        private void btnClosePanel_Click(object sender, EventArgs e)
        {
            try
            {
                this.panel1.Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnDeleteGropu_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(this.dvwViewGroups.CurrentRow.Cells["GroupID"].Value.ToString()) == 0x16)
                {
                    MessageBox.Show("此条记录是系统基本需要,不能删除");
                }
                else
                {
                    string sql = "DELETE FROM Groups WHERE GroupID = @GroupID";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@GroupID", this.dvwViewGroups.CurrentRow.Cells["GroupID"].Value.ToString()) };
                        if (MessageBox.Show("确定要删除GroupID为" + this.dvwViewGroups.CurrentRow.Cells["GroupID"].Value.ToString() + "的这条数据", "删除", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                        {
                            DBHelper.ExecuteCommand(sql, values);
                            goto Label_0111;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        MessageBox.Show("无法删除啦，请联系客服，谢谢！");
                    }
                }
                return;
            Label_0111:
                this.loadGroup();
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        private void btnleftall_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.listBox2.Items.Count; i++)
                {
                    this.listBox1.Items.Add(this.listBox2.Items[i]);
                }
                this.listBox2.Items.Clear();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnNewGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.panel1.Visible)
                {
                    this.panel1.Visible = false;
                }
                else
                {
                    this.panel1.Visible = true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnNewGroup_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.listBox1.Items.Clear();
                this.listBox2.Items.Clear();
                this.RepartLoad();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnrightall_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.listBox1.Items.Count; i++)
                {
                    this.listBox2.Items.Add(this.listBox1.Items[i]);
                }
                this.listBox1.Items.Clear();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnUpdateGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBox2.Items.Count == 0)
                {
                    MessageBox.Show("请添加设备编码");
                }
                else if (this.cbxSelectGroup.Items.Count == 0)
                {
                    MessageBox.Show("请先创建一个组");
                }
                else if (this.cbxSelectGroup.Text.Equals(""))
                {
                    MessageBox.Show("请选择一个组");
                }
                else
                {
                    for (int i = 0; i < this.listBox2.Items.Count; i++)
                    {
                        string sql = "UPDATE  EquipmentCode SET _Groups = @_Groups where Nickname = @Nickname";
                        try
                        {
                            string[] strArray = this.listBox2.Items[i].ToString().Split(new char[] { '(' });
                            OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@_Groups", this.cbxSelectGroup.Text.Trim().ToString()), new OleDbParameter("@Nickname", strArray[0]) };
                            DBHelper.ExecuteCommand(sql, values);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    MessageBox.Show("分组成功,继续分组请选择不同的组.退出请点完成");
                    this.cbxSelectGroup_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnUpdateGroups_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(this.dvwViewGroups.CurrentRow.Cells["GroupID"].Value.ToString()) == 0x16)
                {
                    MessageBox.Show("此条记录是系统基本需要,不能修改");
                }
                else
                {
                    string sql = "update Groups set GroupName = @GroupName WHERE GroupID = @GroupID";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("GroupName", this.txtUpdateGroups.Text.Trim().ToString()), new OleDbParameter("@GroupID", this.dvwViewGroups.CurrentRow.Cells["GroupID"].Value.ToString()) };
                        if (MessageBox.Show("确定要修改GroupID为" + this.dvwViewGroups.CurrentRow.Cells["GroupID"].Value.ToString() + "的这条数据", "删除", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                        {
                            DBHelper.ExecuteCommand(sql, values);
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch
                    {
                    }
                    this.loadGroup();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnUpdateGroups_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.toolTip1.SetToolTip(this.btnUpdateGroup, "请在左边的文本框输入要修改的组名");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int count = this.listBox1.SelectedItems.Count;
                if (this.listBox1.SelectedItems.Count != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        this.listBox2.Items.Add(this.listBox1.SelectedItem);
                        this.listBox1.Items.Remove(this.listBox1.SelectedItem);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int count = this.listBox2.SelectedItems.Count;
                if (this.listBox2.SelectedItems.Count != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        this.listBox1.Items.Add(this.listBox2.SelectedItem);
                        this.listBox2.Items.Remove(this.listBox2.SelectedItem);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void cbxSelectGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbxSelectGroup.Text.Equals("全部"))
                {
                    this.listBox2.Items.Clear();
                }
                else
                {
                    this.listBox2.Items.Clear();
                    string sql = string.Format("select * from EquipmentCode where [_Groups] = @_Groups and EquipmentCode Between 0 and {0}", this.count);
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@_Groups", this.cbxSelectGroup.Text.ToString()) };
                        DataTable dataSet = DBHelper.GetDataSet(sql, values);
                        foreach (DataRow row in dataSet.Rows)
                        {
                            this.listBox2.Items.Add(((string) row["Nickname"]) + "(设备编码:" + row["EquipmentCode"].ToString() + ")");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
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

        private void dvwViewGroups_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtUpdateGroups.Text = this.dvwViewGroups.CurrentRow.Cells["GroupName"].Value.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                this.loadGroup();
                this.listBox1.Items.Clear();
                string sql = string.Format("select * from EquipmentCode where [_Groups] = @_Groups and EquipmentCode Between 0 and {0}", this.count);
                try
                {
                    OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@_Groups", "全部") };
                    DataTable dataSet = DBHelper.GetDataSet(sql, values);
                    foreach (DataRow row in dataSet.Rows)
                    {
                        this.listBox1.Items.Add(((string) row["Nickname"]) + "(设备编码:" + row["EquipmentCode"].ToString() + ")");
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form3));
            this.listBox1 = new ListBox();
            this.listBox2 = new ListBox();
            this.btnNewGroup = new Button();
            this.cbxSelectGroup = new ComboBox();
            this.btnUpdateGroup = new Button();
            this.label1 = new Label();
            this.btnClose = new Button();
            this.btnright = new Button();
            this.btnleft = new Button();
            this.btnrightall = new Button();
            this.btnleftall = new Button();
            this.btnRefresh = new Button();
            this.panel1 = new Panel();
            this.txtUpdateGroups = new TextBox();
            this.btnUpdateGroups = new Button();
            this.txtViewGroupName = new TextBox();
            this.btnClosePanel = new Button();
            this.btnDeleteGropu = new Button();
            this.btnAddGroup = new Button();
            this.dvwViewGroups = new DataGridView();
            this.toolTip1 = new ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.dvwViewGroups).BeginInit();
            base.SuspendLayout();
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new Point(0x1b, 0x24);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = SelectionMode.MultiSimple;
            this.listBox1.Size = new Size(0x97, 0xd0);
            this.listBox1.TabIndex = 0;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new Point(0xff, 0x24);
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectionMode = SelectionMode.MultiSimple;
            this.listBox2.Size = new Size(0x97, 0xd0);
            this.listBox2.TabIndex = 1;
            this.btnNewGroup.Location = new Point(0x72, 12);
            this.btnNewGroup.Name = "btnNewGroup";
            this.btnNewGroup.Size = new Size(0x40, 0x15);
            this.btnNewGroup.TabIndex = 2;
            this.btnNewGroup.Text = "分组管理";
            this.toolTip1.SetToolTip(this.btnNewGroup, "点击后会弹出一个添加,修改,删除组的管理界面");
            this.btnNewGroup.UseVisualStyleBackColor = true;
            this.btnNewGroup.MouseMove += new MouseEventHandler(this.btnNewGroup_MouseMove);
            this.btnNewGroup.Click += new EventHandler(this.btnNewGroup_Click);
            this.cbxSelectGroup.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxSelectGroup.FormattingEnabled = true;
            this.cbxSelectGroup.Location = new Point(0xff, 12);
            this.cbxSelectGroup.Name = "cbxSelectGroup";
            this.cbxSelectGroup.Size = new Size(0x79, 20);
            this.cbxSelectGroup.TabIndex = 3;
            this.cbxSelectGroup.SelectedIndexChanged += new EventHandler(this.cbxSelectGroup_SelectedIndexChanged);
            this.btnUpdateGroup.Location = new Point(0xfd, 250);
            this.btnUpdateGroup.Name = "btnUpdateGroup";
            this.btnUpdateGroup.Size = new Size(70, 0x15);
            this.btnUpdateGroup.TabIndex = 4;
            this.btnUpdateGroup.Text = "保存分组";
            this.toolTip1.SetToolTip(this.btnUpdateGroup, "保存分组信息");
            this.btnUpdateGroup.UseVisualStyleBackColor = true;
            this.btnUpdateGroup.Click += new EventHandler(this.btnUpdateGroup_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xd0, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x29, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "选择组";
            this.btnClose.Location = new Point(0x15c, 250);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x3a, 0x15);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "退 出";
            this.toolTip1.SetToolTip(this.btnClose, "分组完成关闭界面");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.btnright.Location = new Point(0xc2, 0x35);
            this.btnright.Name = "btnright";
            this.btnright.Size = new Size(0x2d, 0x15);
            this.btnright.TabIndex = 7;
            this.btnright.Text = ">";
            this.btnright.UseVisualStyleBackColor = true;
            this.btnright.Click += new EventHandler(this.button4_Click);
            this.btnleft.Location = new Point(0xc2, 0x5d);
            this.btnleft.Name = "btnleft";
            this.btnleft.Size = new Size(0x2d, 0x15);
            this.btnleft.TabIndex = 8;
            this.btnleft.Text = "<";
            this.btnleft.UseVisualStyleBackColor = true;
            this.btnleft.Click += new EventHandler(this.button5_Click);
            this.btnrightall.Location = new Point(0xc2, 0x85);
            this.btnrightall.Name = "btnrightall";
            this.btnrightall.Size = new Size(0x2d, 0x15);
            this.btnrightall.TabIndex = 9;
            this.btnrightall.Text = "> > >";
            this.btnrightall.UseVisualStyleBackColor = true;
            this.btnrightall.Click += new EventHandler(this.btnrightall_Click);
            this.btnleftall.Location = new Point(0xc1, 0xad);
            this.btnleftall.Name = "btnleftall";
            this.btnleftall.Size = new Size(0x2d, 0x15);
            this.btnleftall.TabIndex = 10;
            this.btnleftall.Text = "< < <";
            this.btnleftall.UseVisualStyleBackColor = true;
            this.btnleftall.Click += new EventHandler(this.btnleftall_Click);
            this.btnRefresh.Location = new Point(0x1b, 250);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(0x41, 0x15);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "重新分组";
            this.toolTip1.SetToolTip(this.btnRefresh, "重新取得设备编码进行分组");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            this.panel1.Controls.Add(this.txtUpdateGroups);
            this.panel1.Controls.Add(this.btnUpdateGroups);
            this.panel1.Controls.Add(this.txtViewGroupName);
            this.panel1.Controls.Add(this.btnClosePanel);
            this.panel1.Controls.Add(this.btnDeleteGropu);
            this.panel1.Controls.Add(this.btnAddGroup);
            this.panel1.Controls.Add(this.dvwViewGroups);
            this.panel1.Location = new Point(12, 0x24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x19d, 0xf5);
            this.panel1.TabIndex = 12;
            this.panel1.Visible = false;
            this.txtUpdateGroups.Location = new Point(0x99, 0xd7);
            this.txtUpdateGroups.Name = "txtUpdateGroups";
            this.txtUpdateGroups.Size = new Size(0x5d, 0x15);
            this.txtUpdateGroups.TabIndex = 6;
            this.btnUpdateGroups.Location = new Point(0xfc, 0xd6);
            this.btnUpdateGroups.Name = "btnUpdateGroups";
            this.btnUpdateGroups.Size = new Size(0x2c, 0x17);
            this.btnUpdateGroups.TabIndex = 5;
            this.btnUpdateGroups.Text = "修改";
            this.toolTip1.SetToolTip(this.btnUpdateGroups, "请在左边的文本框输入要修改的组名");
            this.btnUpdateGroups.UseVisualStyleBackColor = true;
            this.btnUpdateGroups.MouseMove += new MouseEventHandler(this.btnUpdateGroups_MouseMove);
            this.btnUpdateGroups.Click += new EventHandler(this.btnUpdateGroups_Click);
            this.txtViewGroupName.Location = new Point(8, 0xd6);
            this.txtViewGroupName.Name = "txtViewGroupName";
            this.txtViewGroupName.Size = new Size(0x5d, 0x15);
            this.txtViewGroupName.TabIndex = 4;
            this.btnClosePanel.Location = new Point(360, 0xd6);
            this.btnClosePanel.Name = "btnClosePanel";
            this.btnClosePanel.Size = new Size(0x2c, 0x17);
            this.btnClosePanel.TabIndex = 3;
            this.btnClosePanel.Text = "关闭";
            this.toolTip1.SetToolTip(this.btnClosePanel, "关闭管理分组界面");
            this.btnClosePanel.UseVisualStyleBackColor = true;
            this.btnClosePanel.Click += new EventHandler(this.btnClosePanel_Click);
            this.btnDeleteGropu.Location = new Point(0x131, 0xd6);
            this.btnDeleteGropu.Name = "btnDeleteGropu";
            this.btnDeleteGropu.Size = new Size(0x2c, 0x17);
            this.btnDeleteGropu.TabIndex = 2;
            this.btnDeleteGropu.Text = "删除";
            this.btnDeleteGropu.UseVisualStyleBackColor = true;
            this.btnDeleteGropu.Click += new EventHandler(this.btnDeleteGropu_Click);
            this.btnAddGroup.Location = new Point(0x67, 0xd6);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new Size(0x2c, 0x17);
            this.btnAddGroup.TabIndex = 1;
            this.btnAddGroup.Text = "添加";
            this.toolTip1.SetToolTip(this.btnAddGroup, "请在左边的文本框输入要添加的组名");
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.MouseMove += new MouseEventHandler(this.btnAddGroup_MouseMove);
            this.btnAddGroup.Click += new EventHandler(this.btnAddGroup_Click);
            this.dvwViewGroups.AllowUserToAddRows = false;
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dvwViewGroups.ColumnHeadersDefaultCellStyle = style;
            this.dvwViewGroups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dvwViewGroups.DefaultCellStyle = style2;
            this.dvwViewGroups.Location = new Point(8, 5);
            this.dvwViewGroups.Name = "dvwViewGroups";
            this.dvwViewGroups.ReadOnly = true;
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Control;
            style3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style3.ForeColor = SystemColors.WindowText;
            style3.SelectionBackColor = SystemColors.Highlight;
            style3.SelectionForeColor = SystemColors.HighlightText;
            style3.WrapMode = DataGridViewTriState.True;
            this.dvwViewGroups.RowHeadersDefaultCellStyle = style3;
            this.dvwViewGroups.RowTemplate.Height = 0x17;
            this.dvwViewGroups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dvwViewGroups.Size = new Size(0x18c, 0xcc);
            this.dvwViewGroups.TabIndex = 0;
            this.dvwViewGroups.Click += new EventHandler(this.dvwViewGroups_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x1bb, 0x11b);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.btnRefresh);
            base.Controls.Add(this.btnleftall);
            base.Controls.Add(this.btnrightall);
            base.Controls.Add(this.btnleft);
            base.Controls.Add(this.btnright);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnUpdateGroup);
            base.Controls.Add(this.cbxSelectGroup);
            base.Controls.Add(this.btnNewGroup);
            base.Controls.Add(this.listBox2);
            base.Controls.Add(this.listBox1);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form3";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "分组";
            base.Load += new EventHandler(this.Form3_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((ISupportInitialize) this.dvwViewGroups).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void loadGroup()
        {
            try
            {
                this.cbxSelectGroup.Items.Clear();
                string safeSql = "select * from Groups order by GroupID desc";
                DataTable dataSet = DBHelper.GetDataSet(safeSql);
                foreach (DataRow row in dataSet.Rows)
                {
                    if (!row["GroupName"].ToString().Equals("全部"))
                    {
                        this.cbxSelectGroup.Items.Add(row["GroupName"].ToString());
                    }
                }
                this.cbxSelectGroup.Items.Add("全部");
                this.cbxSelectGroup.SelectedIndex = (this.cbxSelectGroup.Items.Count > 0) ? 0 : -1;
                this.dvwViewGroups.DataSource = dataSet;
                this.dvwViewGroups.Columns["GroupName"].HeaderText = "组名";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void RepartLoad()
        {
            try
            {
                DataTable dataSet = DBHelper.GetDataSet(string.Format("select * from EquipmentCode where EquipmentCode Between 0 and {0}", this.count));
                foreach (DataRow row in dataSet.Rows)
                {
                    this.listBox1.Items.Add(((string) row["Nickname"]) + "(设备编码:" + row["EquipmentCode"].ToString() + ")");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }
    }
}

