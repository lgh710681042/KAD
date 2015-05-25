namespace 发卡模块
{
    using DAL;
    using KADUpperComputer.BaseClass;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using 发卡模块.Properties;

    public class Form4 : Form
    {
        private Button btnCancel;
        private Button btnUpdate;
        private IContainer components = null;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridView dvwviewCardNunber;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private OperateExcel opExcel = new OperateExcel();
        private OpenFileDialog opfExcel;
        private SaveFileDialog saveFileDialog1;
        private Thread th;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private ToolStripComboBox toolStripComboBox1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolTip toolTip1;
        private TextBox txtBuildingNo;
        private TextBox txtCardName;
        private TextBox txtRemarks;
        private TextBox txtUserName;
        private string UserPermissionType = string.Empty;
        private ToolStripMenuItem 查看设备编码ToolStripMenuItem;

        public Form4(string _UserPermissionType)
        {
            this.UserPermissionType = _UserPermissionType;
            this.InitializeComponent();
            if (this.UserPermissionType.Contains("Guest"))
            {
                this.toolStripButton2.Enabled = false;
                this.toolStripButton3.Enabled = false;
                this.toolStripButton5.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.UpdateCancel();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCardName.Text.Equals("") || this.label5.Text.Equals(""))
                {
                    MessageBox.Show("请选择您要修改的数据");
                }
                else
                {
                    string sql = "UPDATE CardNumber SET CardNumber = @CardNumber,UserName = @UserName,BuildingNo = @BuildingNo,Remarks = @Remarks WHERE ID = @ID";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@CardNumber", this.txtCardName.Text), new OleDbParameter("@UserName", this.txtUserName.Text), new OleDbParameter("@BuildingNo", this.txtBuildingNo.Text), new OleDbParameter("@Remarks", this.txtRemarks.Text), new OleDbParameter("@ID", int.Parse(this.label5.Text.ToString())) };
                        if (MessageBox.Show("确定要修改卡号为" + this.txtCardName.Text + "的这条数据", "修改", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                        {
                            DBHelper.ExecuteCommand(sql, values);
                            goto Label_0155;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        MessageBox.Show("数据不能更改啦，请联系客服！");
                    }
                }
                return;
            Label_0155:
                this.loadCardNunber();
                this.UpdateCancel();
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

        private void dvwviewCardNunber_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void dvwviewCardNunber_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (this.dvwviewCardNunber.Rows.Count != 0)
                {
                    int iD = (int) this.dvwviewCardNunber.CurrentRow.Cells["ID"].Value;
                    new Form7(iD) { Owner = this }.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void dvwviewCardNunber_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                e.ToolTipText = "双击或右键显示这次发卡所选择的设备编号";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void dvwviewCardNunber_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                this.toolStripComboBox1.SelectedIndex = (this.toolStripComboBox1.Items.Count > 0) ? 0 : -1;
                this.loadCardNunber();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void inExcel1()
        {
            try
            {
                inExcel method = new inExcel(this.inExcelweituo);
                base.Invoke(method);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void inExcelweituo()
        {
            try
            {
                int num = 0xf4241;
                string[] strArray = new string[12];
                if (MessageBox.Show("导入后，旧的数据将被全部删除，确定要导入吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    frmBusy busy = new frmBusy();
                    this.opfExcel.Filter = "EXCEL (*.xlsx;*.csv)|*.xlsx;*.csv|All files (*.*)|*.*";
                    if (this.opfExcel.ShowDialog() == DialogResult.OK)
                    {
                        busy.Show();
                        string path = this.opfExcel.FileName.ToString();
                        DataSet set = this.opExcel.ExcelToDS(path);
                        if (set != null)
                        {
                            DBHelper.ExecuteCommand("delete * from CardSerial");
                            for (int i = 0; i < set.Tables[0].Rows.Count; i++)
                            {
                                string str2 = "ST" + Convert.ToString(num);
                                for (int j = 0; j < 5; j++)
                                {
                                    strArray[j] = set.Tables[0].Rows[i][j].ToString();
                                }
                                string sql = "insert into CardNumber (CardNumber,UserName,BuildingNo,SendCardTime,Remarks)values (@CardNumber,@UserName,@BuildingNo,@SendCardTime,@Remarks) ";
                                try
                                {
                                    OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@CarNumber", strArray[0]), new OleDbParameter("@UserName", strArray[1]), new OleDbParameter("@BuildingNo", strArray[2]), new OleDbParameter("@SendCardTime", Convert.ToDateTime(strArray[3])), new OleDbParameter("@Remarks", strArray[4]) };
                                    DBHelper.ExecuteCommand(sql, values);
                                }
                                catch (Exception)
                                {
                                }
                                num++;
                            }
                        }
                        busy.Hide();
                        busy.Dispose();
                        this.loadCardNunber();
                        MessageBox.Show("导入完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form4));
            this.dvwviewCardNunber = new DataGridView();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.查看设备编码ToolStripMenuItem = new ToolStripMenuItem();
            this.toolStrip1 = new ToolStrip();
            this.toolStripLabel1 = new ToolStripLabel();
            this.toolStripComboBox1 = new ToolStripComboBox();
            this.toolStripTextBox1 = new ToolStripTextBox();
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.toolStripButton2 = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.toolStripButton3 = new ToolStripButton();
            this.toolStripButton5 = new ToolStripButton();
            this.toolStripButton4 = new ToolStripButton();
            this.btnUpdate = new Button();
            this.label1 = new Label();
            this.txtCardName = new TextBox();
            this.txtUserName = new TextBox();
            this.label2 = new Label();
            this.txtBuildingNo = new TextBox();
            this.label3 = new Label();
            this.txtRemarks = new TextBox();
            this.label4 = new Label();
            this.label5 = new Label();
            this.btnCancel = new Button();
            this.toolTip1 = new ToolTip(this.components);
            this.saveFileDialog1 = new SaveFileDialog();
            this.opfExcel = new OpenFileDialog();
            ((ISupportInitialize) this.dvwviewCardNunber).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.dvwviewCardNunber.AllowUserToAddRows = false;
            this.dvwviewCardNunber.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dvwviewCardNunber.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders;
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dvwviewCardNunber.ColumnHeadersDefaultCellStyle = style;
            this.dvwviewCardNunber.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvwviewCardNunber.ContextMenuStrip = this.contextMenuStrip1;
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dvwviewCardNunber.DefaultCellStyle = style2;
            this.dvwviewCardNunber.Location = new Point(0, 0x26);
            this.dvwviewCardNunber.Name = "dvwviewCardNunber";
            this.dvwviewCardNunber.ReadOnly = true;
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Control;
            style3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style3.ForeColor = SystemColors.WindowText;
            style3.SelectionBackColor = SystemColors.Highlight;
            style3.SelectionForeColor = SystemColors.HighlightText;
            style3.WrapMode = DataGridViewTriState.True;
            this.dvwviewCardNunber.RowHeadersDefaultCellStyle = style3;
            this.dvwviewCardNunber.RowTemplate.Height = 0x17;
            this.dvwviewCardNunber.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dvwviewCardNunber.Size = new Size(0x21f, 0xc1);
            this.dvwviewCardNunber.TabIndex = 0;
            this.dvwviewCardNunber.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dvwviewCardNunber_CellMouseClick);
            this.dvwviewCardNunber.MouseMove += new MouseEventHandler(this.dvwviewCardNunber_MouseMove);
            this.dvwviewCardNunber.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dvwviewCardNunber_CellMouseDoubleClick);
            this.dvwviewCardNunber.CellToolTipTextNeeded += new DataGridViewCellToolTipTextNeededEventHandler(this.dvwviewCardNunber_CellToolTipTextNeeded);
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.查看设备编码ToolStripMenuItem });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(0x95, 0x1a);
            this.toolTip1.SetToolTip(this.contextMenuStrip1, "查看这条记录是发的哪几个门");
            this.查看设备编码ToolStripMenuItem.Name = "查看设备编码ToolStripMenuItem";
            this.查看设备编码ToolStripMenuItem.Size = new Size(0x94, 0x16);
            this.查看设备编码ToolStripMenuItem.Text = "查看设备编码";
            this.查看设备编码ToolStripMenuItem.Click += new EventHandler(this.查看设备编码ToolStripMenuItem_Click);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripLabel1, this.toolStripComboBox1, this.toolStripTextBox1, this.toolStripButton1, this.toolStripSeparator2, this.toolStripButton2, this.toolStripSeparator1, this.toolStripButton3, this.toolStripButton5, this.toolStripButton4 });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x21d, 40);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0x38, 0x25);
            this.toolStripLabel1.Text = "查询条件";
            this.toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Font = new Font("宋体", 9f);
            this.toolStripComboBox1.Items.AddRange(new object[] { "卡号", "用户名", "楼号", "发卡时间" });
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new Size(0x4b, 40);
            this.toolStripTextBox1.Font = new Font("宋体", 9f);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new Size(100, 40);
            this.toolStripButton1.Image = Resources.Search;
            this.toolStripButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new Size(0x24, 0x25);
            this.toolStripButton1.Text = "查询";
            this.toolStripButton1.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton1.Click += new EventHandler(this.toolStripButton1_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 40);
            this.toolStripButton2.Image = Resources.gif_45_029;
            this.toolStripButton2.ImageTransparentColor = Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new Size(0x24, 0x25);
            this.toolStripButton2.Text = "修改";
            this.toolStripButton2.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton2.Click += new EventHandler(this.toolStripButton2_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 40);
            this.toolStripButton3.Image = Resources._2009210154657338;
            this.toolStripButton3.ImageTransparentColor = Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new Size(0x24, 0x25);
            this.toolStripButton3.Text = "删除";
            this.toolStripButton3.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton3.Click += new EventHandler(this.toolStripButton3_Click);
            this.toolStripButton5.Image = Resources.ex;
            this.toolStripButton5.ImageTransparentColor = Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new Size(0x24, 0x25);
            this.toolStripButton5.Text = "导入";
            this.toolStripButton5.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton5.Click += new EventHandler(this.toolStripButton5_Click);
            this.toolStripButton4.Image = Resources.ex;
            this.toolStripButton4.ImageTransparentColor = Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new Size(0x24, 0x25);
            this.toolStripButton4.Text = "导出";
            this.toolStripButton4.TextImageRelation = TextImageRelation.ImageAboveText;
            this.toolStripButton4.Click += new EventHandler(this.toolStripButton4_Click);
            this.btnUpdate.Location = new Point(0x162, 0x117);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(0x4b, 0x17);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "保  存";
            this.toolTip1.SetToolTip(this.btnUpdate, "保存修改完的记录");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(11, 0xf9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "卡 号:";
            this.label1.Visible = false;
            this.txtCardName.Enabled = false;
            this.txtCardName.Location = new Point(0x3a, 0xf6);
            this.txtCardName.Name = "txtCardName";
            this.txtCardName.Size = new Size(100, 0x15);
            this.txtCardName.TabIndex = 4;
            this.txtCardName.Visible = false;
            this.txtUserName.Location = new Point(0xee, 0xf6);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new Size(100, 0x15);
            this.txtUserName.TabIndex = 6;
            this.txtUserName.Visible = false;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xb9, 0xf9);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2f, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "用户名:";
            this.label2.Visible = false;
            this.txtBuildingNo.Location = new Point(0x199, 0xf6);
            this.txtBuildingNo.Name = "txtBuildingNo";
            this.txtBuildingNo.Size = new Size(100, 0x15);
            this.txtBuildingNo.TabIndex = 7;
            this.txtBuildingNo.Visible = false;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x16a, 0xf9);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "楼 号:";
            this.label3.Visible = false;
            this.txtRemarks.Location = new Point(0x3a, 0x119);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new Size(0x112, 0x15);
            this.txtRemarks.TabIndex = 8;
            this.txtRemarks.Visible = false;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(11, 0x11c);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "备 注:";
            this.label4.Visible = false;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x16a, 0x11c);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0, 12);
            this.label5.TabIndex = 11;
            this.label5.Visible = false;
            this.btnCancel.Location = new Point(0x1b3, 0x117);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取  消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.saveFileDialog1.Filter = "*.xlsx|*.xlsx";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x21d, 0x13a);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.txtRemarks);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.txtBuildingNo);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.txtUserName);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.txtCardName);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnUpdate);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.dvwviewCardNunber);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form4";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "查询发卡记录";
            base.Load += new EventHandler(this.Form4_Load);
            ((ISupportInitialize) this.dvwviewCardNunber).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void loadCardNunber()
        {
            try
            {
                string safeSql = "select  ID,CardNumber,UserName,BuildingNo,SendCardTime,Remarks from CardNumber order by SendCardTime";
                this.dvwviewCardNunber.DataSource = DBHelper.GetDataSet(safeSql);
                this.dvwviewCardNunber.Columns["ID"].Visible = false;
                this.dvwviewCardNunber.Columns["CardNumber"].HeaderText = "卡号";
                this.dvwviewCardNunber.Columns["UserName"].HeaderText = "用户名";
                this.dvwviewCardNunber.Columns["BuildingNo"].HeaderText = "楼号";
                this.dvwviewCardNunber.Columns["SendCardTime"].HeaderText = "发卡时间";
                this.dvwviewCardNunber.Columns["Remarks"].HeaderText = "备注";
                this.dvwviewCardNunber.Focus();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string str;
                DataTable dataSet;
                if (this.toolStripComboBox1.SelectedItem.ToString().Equals("卡号"))
                {
                    str = "select * from CardNumber where CardNumber like '%" + this.toolStripTextBox1.Text.Trim() + "%'";
                    if (this.toolStripTextBox1.Text.Equals(""))
                    {
                        MessageBox.Show("请输入卡号");
                    }
                    else
                    {
                        dataSet = DBHelper.GetDataSet(str);
                        if (dataSet.Rows.Count == 0)
                        {
                            MessageBox.Show("查无数据，请输入正确的卡号");
                        }
                        else
                        {
                            this.dvwviewCardNunber.DataSource = dataSet;
                        }
                    }
                }
                else if (this.toolStripComboBox1.SelectedItem.ToString().Equals("用户名"))
                {
                    str = "select * from CardNumber where UserName like '%" + this.toolStripTextBox1.Text.Trim() + "%'";
                    if (this.toolStripTextBox1.Text.Equals(""))
                    {
                        MessageBox.Show("请输入用户名");
                    }
                    else
                    {
                        dataSet = DBHelper.GetDataSet(str);
                        if (dataSet.Rows.Count == 0)
                        {
                            MessageBox.Show("查无数据，请输入正确的用户名");
                        }
                        else
                        {
                            this.dvwviewCardNunber.DataSource = dataSet;
                        }
                    }
                }
                else if (this.toolStripComboBox1.SelectedItem.ToString().Equals("楼号"))
                {
                    str = "select * from CardNumber where BuildingNo like '%" + this.toolStripTextBox1.Text.Trim() + "%'";
                    if (this.toolStripTextBox1.Text.Equals(""))
                    {
                        MessageBox.Show("请输入用楼号");
                    }
                    else
                    {
                        dataSet = DBHelper.GetDataSet(str);
                        if (dataSet.Rows.Count == 0)
                        {
                            MessageBox.Show("查无数据，请输入正确的楼号");
                        }
                        else
                        {
                            this.dvwviewCardNunber.DataSource = dataSet;
                        }
                    }
                }
                else
                {
                    str = "select * from CardNumber where SendCardTime like '%" + this.toolStripTextBox1.Text.Trim() + "%'";
                    if (this.toolStripTextBox1.Text.Equals(""))
                    {
                        MessageBox.Show("发卡时间");
                    }
                    else
                    {
                        dataSet = DBHelper.GetDataSet(str);
                        if (dataSet.Rows.Count == 0)
                        {
                            MessageBox.Show("查无数据，请输入正确的楼号");
                        }
                        else
                        {
                            this.dvwviewCardNunber.DataSource = dataSet;
                        }
                    }
                }
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
                if (this.dvwviewCardNunber.Rows.Count != 0)
                {
                    this.txtCardName.Text = this.dvwviewCardNunber.CurrentRow.Cells["CardNumber"].Value.ToString();
                    this.txtUserName.Text = this.dvwviewCardNunber.CurrentRow.Cells["UserName"].Value.ToString();
                    this.txtBuildingNo.Text = this.dvwviewCardNunber.CurrentRow.Cells["BuildingNo"].Value.ToString();
                    this.txtRemarks.Text = this.dvwviewCardNunber.CurrentRow.Cells["Remarks"].Value.ToString();
                    this.label5.Text = this.dvwviewCardNunber.CurrentRow.Cells["ID"].Value.ToString();
                    this.txtCardName.Visible = true;
                    this.txtUserName.Visible = true;
                    this.txtBuildingNo.Visible = true;
                    this.txtRemarks.Visible = true;
                    this.label1.Visible = true;
                    this.label2.Visible = true;
                    this.label3.Visible = true;
                    this.label4.Visible = true;
                    this.btnUpdate.Visible = true;
                    this.btnCancel.Visible = true;
                }
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
                if (this.dvwviewCardNunber.Rows.Count != 0)
                {
                    this.txtCardName.Text = this.dvwviewCardNunber.CurrentRow.Cells["CardNumber"].Value.ToString();
                    this.txtUserName.Text = this.dvwviewCardNunber.CurrentRow.Cells["UserName"].Value.ToString();
                    this.txtBuildingNo.Text = this.dvwviewCardNunber.CurrentRow.Cells["BuildingNo"].Value.ToString();
                    this.txtRemarks.Text = this.dvwviewCardNunber.CurrentRow.Cells["Remarks"].Value.ToString();
                    this.label5.Text = this.dvwviewCardNunber.CurrentRow.Cells["ID"].Value.ToString();
                    string sql = "DELETE FROM CardNumber WHERE ID = @ID";
                    if (this.label5.Text.Equals(""))
                    {
                        MessageBox.Show("请选择一条数据");
                        goto Label_0226;
                    }
                    int num = int.Parse(this.dvwviewCardNunber.CurrentRow.Cells["ID"].Value.ToString());
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@ID", int.Parse(this.dvwviewCardNunber.CurrentRow.Cells["ID"].Value.ToString())) };
                        if (MessageBox.Show("确定要删除ID为" + num.ToString() + "的这条数据", "删除", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                        {
                            DBHelper.ExecuteCommand(sql, values);
                            goto Label_0226;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        MessageBox.Show("无法删除啦，请联系客服，谢谢！");
                    }
                }
                return;
            Label_0226:
                this.loadCardNunber();
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
                if (this.dvwviewCardNunber.Rows.Count == 0)
                {
                    MessageBox.Show("没有数据可以导出");
                }
                else
                {
                    DataGridView[] gridViews = new DataGridView[] { this.dvwviewCardNunber };
                    DataGridViewToExcel1 excel = new DataGridViewToExcel1();
                    this.saveFileDialog1.ShowDialog();
                    if (!this.saveFileDialog1.FileName.ToString().Equals(""))
                    {
                        frmBusy busy = new frmBusy();
                        busy.Show();
                        excel.ExportToExcel(gridViews, this.saveFileDialog1.FileName);
                        busy.Hide();
                        busy.Dispose();
                        MessageBox.Show("导出成功,路径：" + this.saveFileDialog1.FileName.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                this.th = new Thread(new ThreadStart(this.inExcel1));
                this.th.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void UpdateCancel()
        {
            try
            {
                this.txtCardName.Visible = false;
                this.txtUserName.Visible = false;
                this.txtBuildingNo.Visible = false;
                this.txtRemarks.Visible = false;
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.label3.Visible = false;
                this.label4.Visible = false;
                this.btnUpdate.Visible = false;
                this.btnCancel.Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void 查看设备编码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dvwviewCardNunber.Rows.Count != 0)
                {
                    int iD = (int) this.dvwviewCardNunber.CurrentRow.Cells["ID"].Value;
                    new Form7(iD) { Owner = this }.Show();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private delegate void barinexcel();

        private delegate void inExcel();
    }
}

