namespace 发卡模块
{
    using DAL;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class frmDataRevert : Form
    {
        private Button btnDRevert;
        private Button btnExit;
        private Button btnSel;
        private Button button1;
        private IContainer components = null;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private OpenFileDialog ofDialogFile;
        private OpenFileDialog openFileDialog1;
        private TextBox textBox1;
        private TextBox txtDRPath;

        public frmDataRevert()
        {
            this.InitializeComponent();
        }

        private void btnDRevert_Click(object sender, EventArgs e)
        {
            try
            {
                Exception exception;
                string text = this.txtDRPath.Text;
                string str2 = Application.StartupPath + @"\SentCard.mdb";
                try
                {
                    try
                    {
                        if (MessageBox.Show("还原数据库会覆盖掉原来的数据库,请谨慎操作!", "还原", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                        {
                            int num;
                            this.txtDRPath.Enabled = false;
                            this.btnSel.Enabled = false;
                            this.button1.Enabled = false;
                            this.textBox1.Enabled = false;
                            this.btnDRevert.Enabled = false;
                            this.btnExit.Enabled = false;
                            this.label3.Visible = true;
                            string safeSql = string.Empty;
                            safeSql = "delete from CardSerial";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from Building";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from CardNumber";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from NickNameRecord";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from EquipmentCode";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from Groups";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from tb_Alm";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from tb_Equ";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from tb_Execute";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from tb_Note";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from tb_TimeD";
                            DBHelper.ExecuteCommand(safeSql);
                            safeSql = "delete from UserInfo";
                            DBHelper.ExecuteCommand(safeSql);
                            DBHelper.ExecuteCommand("insert into Building select * from [;database=" + text + ";pwd=xmkad-5725111].Building");
                            DBHelper.ExecuteCommand("insert into UserInfo select * from [;database=" + text + ";pwd=xmkad-5725111].UserInfo");
                            DataTable dataSet = DBHelper.GetDataSet("select * from [;database=" + text + ";pwd=xmkad-5725111].CardNumber");
                            for (num = 0; num < dataSet.Rows.Count; num++)
                            {
                                DBHelper.ExecuteCommand("insert into CardNumber(CardNumber, UserName, BuildingNo, SendCardTime, Remarks) values('" + dataSet.Rows[num][1].ToString() + "','" + dataSet.Rows[num][2].ToString() + "','" + dataSet.Rows[num][3].ToString() + "','" + dataSet.Rows[num][4].ToString() + "','" + dataSet.Rows[num][5].ToString() + "')");
                            }
                            DBHelper.ExecuteCommand("insert into CardSerial select * from [;database=" + text + ";pwd=xmkad-5725111].CardSerial");
                            DBHelper.ExecuteCommand("insert into Groups select * from [;database=" + text + ";pwd=xmkad-5725111].Groups");
                            DBHelper.ExecuteCommand("insert into EquipmentCode select * from [;database=" + text + ";pwd=xmkad-5725111].EquipmentCode");
                            safeSql = "select top 895 * from EquipmentCode";
                            DataTable table2 = DBHelper.GetDataSet(safeSql);
                            XmlDocument document = new XmlDocument();
                            XmlNode newChild = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                            document.AppendChild(newChild);
                            XmlNode node2 = document.CreateElement("NewXML");
                            document.AppendChild(node2);
                            try
                            {
                                for (num = 0; num < table2.Rows.Count; num++)
                                {
                                    XmlNode node3 = document.CreateElement("row" + num);
                                    XmlAttribute node = document.CreateAttribute("设备编码");
                                    node.Value = table2.Rows[num][1].ToString();
                                    node3.Attributes.Append(node);
                                    node2.AppendChild(node3);
                                    XmlAttribute attribute2 = document.CreateAttribute("昵称-设备名称");
                                    attribute2.Value = table2.Rows[num][6].ToString();
                                    node3.Attributes.Append(attribute2);
                                    node2.AppendChild(node3);
                                }
                                document.Save(Application.StartupPath + @"\address.xml");
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                MessageBox.Show(exception.Message, "Exporting Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            DBHelper.ExecuteCommand("insert into NickNameRecord select * from [;database=" + text + ";pwd=xmkad-5725111].NickNameRecord");
                            DBHelper.ExecuteCommand("insert into tb_Alm select * from [;database=" + text + ";pwd=xmkad-5725111].tb_Alm");
                            DBHelper.ExecuteCommand("insert into tb_Equ select * from [;database=" + text + ";pwd=xmkad-5725111].tb_Equ");
                            DBHelper.ExecuteCommand("insert into tb_Execute select * from [;database=" + text + ";pwd=xmkad-5725111].tb_Execute");
                            DBHelper.ExecuteCommand("insert into tb_Note select * from [;database=" + text + ";pwd=xmkad-5725111].tb_Note");
                            DBHelper.ExecuteCommand("insert into tb_TimeD select * from [;database=" + text + ";pwd=xmkad-5725111].tb_TimeD");
                            if (File.Exists(this.textBox1.Text))
                            {
                                try
                                {
                                    File.Copy(this.textBox1.Text, Application.StartupPath + @"\address.xml", true);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            base.DialogResult = DialogResult.Yes;
                        }
                    }
                    catch (Exception exception4)
                    {
                        exception = exception4;
                        MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    base.Close();
                }
                catch (Exception exception5)
                {
                    exception = exception5;
                    MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
            base.Close();
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            try
            {
                this.ofDialogFile.InitialDirectory = @"D:\";
                this.ofDialogFile.Filter = "bak files (*.bak)|*.bak";
                this.ofDialogFile.RestoreDirectory = true;
                this.ofDialogFile.ShowDialog();
                this.txtDRPath.Text = this.ofDialogFile.FileName.ToString().Trim();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = @"D:\";
            this.openFileDialog1.Filter = "device list bak files (*.dak)|*.dak";
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.ShowDialog();
            this.textBox1.Text = this.openFileDialog1.FileName.ToString().Trim();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmDataRevert));
            this.groupBox2 = new GroupBox();
            this.label2 = new Label();
            this.button1 = new Button();
            this.btnSel = new Button();
            this.label1 = new Label();
            this.textBox1 = new TextBox();
            this.txtDRPath = new TextBox();
            this.btnExit = new Button();
            this.btnDRevert = new Button();
            this.ofDialogFile = new OpenFileDialog();
            this.openFileDialog1 = new OpenFileDialog();
            this.label3 = new Label();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.btnSel);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.txtDRPath);
            this.groupBox2.Location = new Point(0x19, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x13d, 0x66);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new EventHandler(this.groupBox2_Enter);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x11, 60);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4d, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "设备编码文件";
            this.button1.Location = new Point(0xcc, 0x49);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4a, 0x17);
            this.button1.TabIndex = 1;
            this.button1.Text = "打开文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.btnSel.Location = new Point(0xcc, 30);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new Size(0x4a, 0x17);
            this.btnSel.TabIndex = 1;
            this.btnSel.Text = "打开文件";
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new EventHandler(this.btnSel_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(15, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库文件路径";
            this.textBox1.Location = new Point(0x13, 0x4b);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xb5, 0x15);
            this.textBox1.TabIndex = 0;
            this.txtDRPath.Location = new Point(0x13, 0x20);
            this.txtDRPath.Name = "txtDRPath";
            this.txtDRPath.Size = new Size(0xb5, 0x15);
            this.txtDRPath.TabIndex = 0;
            this.btnExit.Location = new Point(0xe2, 0x7c);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(0x4b, 0x17);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnDRevert.Location = new Point(0x43, 0x7c);
            this.btnDRevert.Name = "btnDRevert";
            this.btnDRevert.Size = new Size(0x4b, 0x17);
            this.btnDRevert.TabIndex = 5;
            this.btnDRevert.Text = "数据还原";
            this.btnDRevert.UseVisualStyleBackColor = true;
            this.btnDRevert.Click += new EventHandler(this.btnDRevert_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x7c, 0x98);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "正在还原数据....";
            this.label3.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x16c, 0xab);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.btnExit);
            base.Controls.Add(this.btnDRevert);
            base.Controls.Add(this.groupBox2);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "frmDataRevert";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "还原数据";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

