namespace 发卡模块
{
    using DAL;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;

    public class Form2 : Form
    {
        private Button btnSave;
        private Button button1;
        private IContainer components = null;
        private DataGridView dvwViewAddress;
        private Label label1;
        private ToolTip toolTip1;
        private TextBox txtNumber;

        public Form2()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dvwViewAddress.Rows.Count == 0)
                {
                    MessageBox.Show("请至少选择一个设备编码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    int num;
                    int num2;
                    Exception exception;
                    string str = null;
                    for (num = 0; num < this.dvwViewAddress.Rows.Count; num++)
                    {
                        str = this.dvwViewAddress.Rows[num].Cells["Nickname"].Value.ToString();
                        num2 = num + 1;
                        while (num2 < this.dvwViewAddress.Rows.Count)
                        {
                            if (str.Equals(this.dvwViewAddress.Rows[num2].Cells["Nickname"].Value.ToString()))
                            {
                                MessageBox.Show("不能输入相同的昵称");
                                return;
                            }
                            num2++;
                        }
                    }
                    for (num = 1; num <= this.dvwViewAddress.Rows.Count; num++)
                    {
                        string str2 = "UPDATE EquipmentCode SET Nickname = @Nickname where EquipmentCode = @EquipmentCode";
                        try
                        {
                            OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@Nickname", this.dvwViewAddress.Rows[num - 1].Cells["Nickname"].Value.ToString()), new OleDbParameter("@EquipmentCode", num) };
                            Console.WriteLine(str2);
                            DBHelper.ExecuteCommand(str2, values);
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            Console.WriteLine(exception.ToString());
                        }
                    }
                    if (this.dvwViewAddress.Rows.Count == 0)
                    {
                        MessageBox.Show("请至少选择一个设备编码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        XmlDocument document = new XmlDocument();
                        XmlNode newChild = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                        document.AppendChild(newChild);
                        XmlNode node2 = document.CreateElement("NewXML");
                        document.AppendChild(node2);
                        try
                        {
                            for (num = 0; num < this.dvwViewAddress.Rows.Count; num++)
                            {
                                XmlNode node3 = document.CreateElement("row" + num);
                                for (num2 = 0; num2 < this.dvwViewAddress.Columns.Count; num2++)
                                {
                                    XmlAttribute node = document.CreateAttribute(this.dvwViewAddress.Columns[num2].HeaderText);
                                    node.Value = this.dvwViewAddress.Rows[num].Cells[num2].Value.ToString().Trim();
                                    node3.Attributes.Append(node);
                                    node2.AppendChild(node3);
                                }
                            }
                            document.Save(Application.StartupPath + @"\address.xml");
                        }
                        catch (Exception exception3)
                        {
                            exception = exception3;
                            MessageBox.Show(exception.Message, "Exporting Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        MessageBox.Show("选择数量成功");
                        Form1 form = null;
                        foreach (Form form2 in Application.OpenForms)
                        {
                            if (form2 is Form1)
                            {
                                form = form2 as Form1;
                                form.Activate();
                                form.load();
                                break;
                            }
                            if (form == null)
                            {
                                form = new Form1();
                            }
                            form.Show();
                            form.BringToFront();
                        }
                        base.Close();
                    }
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                new Form3(this.dvwViewAddress.Rows.Count) { Owner = this }.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
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

        private void dvwViewAddress_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int num = e.RowIndex + 1;
                if ((((DataTable) this.dvwViewAddress.DataSource).Rows[e.RowIndex][e.ColumnIndex].ToString() != num.ToString()) && (Regex.Match(((DataTable) this.dvwViewAddress.DataSource).Rows[e.RowIndex][e.ColumnIndex].ToString(), @"^\d+$").Success && (int.Parse(((DataTable) this.dvwViewAddress.DataSource).Rows[e.RowIndex][e.ColumnIndex].ToString()) < 0x1770)))
                {
                    MessageBox.Show("设备备注除与设备编码相同外，不允许输入6000以内的纯数字!", "错误!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    ((DataTable) this.dvwViewAddress.DataSource).Rows[e.RowIndex][e.ColumnIndex] = e.RowIndex + 1;
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                int count = Form1.GetDataTable().Rows.Count;
                this.txtNumber.Text = count.ToString();
                if (count != 0)
                {
                    string safeSql = string.Format("select EquipmentCode,Nickname from EquipmentCode where EquipmentCode Between 0 and {0}", count);
                    this.dvwViewAddress.DataSource = DBHelper.GetDataSet(safeSql);
                    this.dvwViewAddress.Columns["EquipmentCode"].HeaderText = "设备编码";
                    this.dvwViewAddress.Columns["Nickname"].HeaderText = "昵称-设备名称";
                    this.dvwViewAddress.Columns[0].ReadOnly = true;
                    this.dvwViewAddress.Columns[1].ReadOnly = false;
                    this.dvwViewAddress.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    this.dvwViewAddress.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form2));
            this.txtNumber = new TextBox();
            this.label1 = new Label();
            this.btnSave = new Button();
            this.dvwViewAddress = new DataGridView();
            this.button1 = new Button();
            this.toolTip1 = new ToolTip(this.components);
            ((ISupportInitialize) this.dvwViewAddress).BeginInit();
            base.SuspendLayout();
            this.txtNumber.Location = new Point(0x61, 0x11);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new Size(100, 0x15);
            this.txtNumber.TabIndex = 1;
            this.txtNumber.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(2, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "设定设备数量：";
            this.btnSave.Location = new Point(12, 0xfd);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x4b, 0x17);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存设置";
            this.toolTip1.SetToolTip(this.btnSave, "保存选择跟修改过的设备编码");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.button1_Click);
            this.dvwViewAddress.AllowUserToAddRows = false;
            this.dvwViewAddress.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dvwViewAddress.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvwViewAddress.Location = new Point(12, 0x2c);
            this.dvwViewAddress.Name = "dvwViewAddress";
            this.dvwViewAddress.RowTemplate.Height = 0x17;
            this.dvwViewAddress.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dvwViewAddress.Size = new Size(330, 0xcb);
            this.dvwViewAddress.TabIndex = 0;
            this.dvwViewAddress.CellEndEdit += new DataGridViewCellEventHandler(this.dvwViewAddress_CellEndEdit);
            this.button1.Location = new Point(0x61, 0xfd);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x55, 0x17);
            this.button1.TabIndex = 6;
            this.button1.Text = "设备分组管理";
            this.toolTip1.SetToolTip(this.button1, "根据需要把设备编码分到不同的组别");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click_1);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x162, 0x120);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.dvwViewAddress);
            base.Controls.Add(this.btnSave);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.txtNumber);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form2";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "选择数据";
            base.Load += new EventHandler(this.Form2_Load);
            ((ISupportInitialize) this.dvwViewAddress).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.txtNumber.Text.Equals(""))
                {
                    if (int.Parse(this.txtNumber.Text.Trim().ToString()) > 0x37f)
                    {
                        MessageBox.Show("最大不能超过895");
                        this.txtNumber.Text = "895";
                    }
                    else
                    {
                        string safeSql = string.Format("select EquipmentCode,Nickname from EquipmentCode where EquipmentCode Between 0 and {0}", int.Parse(this.txtNumber.Text));
                        this.dvwViewAddress.DataSource = DBHelper.GetDataSet(safeSql);
                        this.dvwViewAddress.Columns["EquipmentCode"].HeaderText = "设备编码";
                        this.dvwViewAddress.Columns["Nickname"].HeaderText = "昵称-设备名称";
                        this.dvwViewAddress.Columns["EquipmentCode"].ReadOnly = true;
                        this.dvwViewAddress.Columns["Nickname"].ReadOnly = false;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }
    }
}

