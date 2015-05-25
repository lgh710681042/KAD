namespace 发卡模块
{
    using DAL;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO.Ports;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class Family : Form
    {
        private string cardserial;
        private RadioButton Choose_AddToBlacklist;
        private RadioButton Choose_ExtendTheValidity;
        private RadioButton Choose_RemoveToBlacklist;
        private DataGridViewCheckBoxColumn Column1;
        public SerialPort comm;
        private IContainer components = null;
        private CheckBox DateCheckBox;
        private Label DisplayCardCode;
        private DataGridView dvwShowBlacklist;
        private Button ExtendTheValidity;
        private CheckBox FamilyChoose;
        private Form1 FamilyParentForm;
        private Button FindCard;
        private TextBox FindCardCode;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private string portname = string.Empty;
        private Button ReadCard;
        private Timer timer1;
        private DateTimePicker ToDate;
        public TextBox UseCount;
        private CheckBox UseCountCheckBox;

        public Family(Form1 _ParentForm, string s)
        {
            this.FamilyParentForm = _ParentForm;
            this.portname = s;
            this.comm = Form1.comm;
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

        private void DvwBlacklistheaderText()
        {
            try
            {
                this.dvwShowBlacklist.Columns["CardNumber"].HeaderText = "卡号";
                this.dvwShowBlacklist.Columns["UserName"].HeaderText = "姓名";
                this.dvwShowBlacklist.Columns["Sex"].HeaderText = "性别";
                this.dvwShowBlacklist.Columns["AreaName"].HeaderText = "小区名称";
                this.dvwShowBlacklist.Columns["BuildingNo"].HeaderText = "楼号";
                this.dvwShowBlacklist.Columns["RoomNumber"].HeaderText = "房号";
                this.dvwShowBlacklist.Columns["Count"].HeaderText = "次数";
                this.dvwShowBlacklist.Columns["StartPeriod"].HeaderText = "开始日期";
                this.dvwShowBlacklist.Columns["Period"].HeaderText = "结束日期";
                this.dvwShowBlacklist.Columns["Blacklist"].HeaderText = "权限";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void Family_Load(object sender, EventArgs e)
        {
            EventHandler handler = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            try
            {
                this.ToDate.Enabled = true;
                this.UseCount.Enabled = false;
                this.ToDate.Checked = true;
                this.DateCheckBox.Enabled = true;
                this.FamilyChoose.Checked = true;
                if (handler == null)
                {
                    handler = delegate (object FamilyChooseE, EventArgs FamilyChooseSender) {
                        Exception exception;
                        try
                        {
                            if (this.FamilyChoose.Checked)
                            {
                                this.SelectFamilyCard();
                            }
                            else
                            {
                                foreach (DataGridViewRow row in (IEnumerable) this.dvwShowBlacklist.Rows)
                                {
                                    try
                                    {
                                        if ((bool) row.Cells[0].Value)
                                        {
                                            row.Cells[0].Value = false;
                                        }
                                    }
                                    catch (Exception exception1)
                                    {
                                        exception = exception1;
                                    }
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            MessageBox.Show(exception.Message.ToString());
                            Console.WriteLine(exception.ToString());
                        }
                    };
                }
                this.FamilyChoose.CheckedChanged += handler;
                string safeSql = "select CardNumber,UserName,Sex,AreaName,BuildingNo,RoomNumber,Count,StartPeriod,Period,Blacklist from CardSerial";
                this.dvwShowBlacklist.DataSource = DBHelper.GetDataSet(safeSql);
                this.dvwShowBlacklist.Columns[0].Width = 20;
                this.dvwShowBlacklist.Columns[3].Width = 60;
                this.dvwShowBlacklist.Columns[4].Width = 80;
                this.dvwShowBlacklist.Columns[5].Width = 60;
                this.dvwShowBlacklist.Columns[6].Width = 60;
                this.dvwShowBlacklist.Columns[8].Width = 80;
                this.dvwShowBlacklist.Columns[9].Width = 80;
                this.DvwBlacklistheaderText();
                for (int j = 1; j <= 10; j++)
                {
                    this.dvwShowBlacklist.Columns[j].ReadOnly = true;
                }
                this.dvwShowBlacklist.Focus();
                this.Text = string.Format((string) base.Tag, this.dvwShowBlacklist.Rows.Count);
                this.SelectFamilyCard();
                if (handler2 == null)
                {
                    handler2 = delegate (object ExtendTheValidity_Sender, EventArgs ExtendTheValidity_E) {
                        try
                        {
                            byte num2;
                            byte num3;
                            byte num4;
                            byte num5;
                            this.ReadCard.Enabled = false;
                            this.ExtendTheValidity.Enabled = false;
                            List<string> selectFamilyCardCodeList = new List<string>();
                            string str = string.Empty;
                            if (this.UseCountCheckBox.Checked)
                            {
                                if ((!Regex.IsMatch(this.UseCount.Text, @"^\d{3}$") && !Regex.IsMatch(this.UseCount.Text, @"^\d{2}$")) && !Regex.IsMatch(this.UseCount.Text, @"^\d{1}$"))
                                {
                                    throw new Exception("请输入正确的次数!");
                                }
                                if (int.Parse(this.UseCount.Text) > 250)
                                {
                                    throw new Exception("次数不能够大于250");
                                }
                            }
                            for (int k = 0; k < this.dvwShowBlacklist.Rows.Count; k++)
                            {
                                this.dvwShowBlacklist.EndEdit();
                                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell) this.dvwShowBlacklist.Rows[k].Cells[0];
                                if (Convert.ToBoolean(cell.Value))
                                {
                                    selectFamilyCardCodeList.Add(this.dvwShowBlacklist.Rows[k].Cells[1].Value.ToString().Trim());
                                }
                            }
                            if (selectFamilyCardCodeList.Count > 10)
                            {
                                throw new Exception("一张用户卡，一次只能同时操作10张家庭成员卡!");
                            }
                            string name = string.Empty;
                            if (this.Choose_ExtendTheValidity.Checked)
                            {
                                name = this.Choose_ExtendTheValidity.Name;
                            }
                            else if (this.Choose_AddToBlacklist.Checked)
                            {
                                name = this.Choose_AddToBlacklist.Name;
                            }
                            else if (this.Choose_RemoveToBlacklist.Checked)
                            {
                                name = this.Choose_RemoveToBlacklist.Name;
                            }
                            if (this.ToDate.Enabled)
                            {
                                num2 = Function.Dex2Hex(this.ToDate.Value.ToString("yyyy-MM-dd").Split(new char[] { '-' })[0].Substring(2, 2));
                                num3 = Function.Dex2Hex(this.ToDate.Value.ToString("yyy-MM-dd").Split(new char[] { '-' })[1]);
                                num4 = Function.Dex2Hex(this.ToDate.Value.ToString("yyy-MM-dd").Split(new char[] { '-' })[2]);
                            }
                            else
                            {
                                num2 = Function.Dex2Hex(DateTime.Now.AddYears(60).ToString("yyyy-MM-dd").Split(new char[] { '-' })[0].Substring(2, 2));
                                num3 = Function.Dex2Hex(DateTime.Now.AddYears(60).ToString("yyy-MM-dd").Split(new char[] { '-' })[1]);
                                num4 = Function.Dex2Hex(DateTime.Now.AddYears(60).ToString("yyy-MM-dd").Split(new char[] { '-' })[2]);
                            }
                            if (this.UseCountCheckBox.Checked)
                            {
                                num5 = (byte) int.Parse(this.UseCount.Text.Trim());
                            }
                            else
                            {
                                num5 = 0xff;
                            }
                            str = Function.FamilyExtendTheValidity(this.comm, this.portname, num2, num3, num4, num5, new List<string>(), name, selectFamilyCardCodeList, this.DisplayCardCode);
                            MessageBox.Show("操作成功，请到设备上刷卡。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            DBHelper.ExecuteCommand(string.Format("\r\n                                    UPDATE CardSerial\r\n                                       SET [Count] = '{0}',  \r\n                                           Period = '{1}'\r\n                                     WHERE CardNumber = '{2}'", this.UseCountCheckBox.Checked ? this.UseCount.Text : "无限制", this.ToDate.Enabled ? this.ToDate.Value.ToString("yyy-MM-dd") : DateTime.Now.AddYears(60).ToString("yyy-MM-dd"), str));
                            foreach (string str4 in selectFamilyCardCodeList)
                            {
                                DBHelper.ExecuteCommand(string.Format("\r\n                                    UPDATE CardSerial\r\n                                       SET [Count] = '{0}',  \r\n                                           Period = '{1}',\r\n                                           Blacklist = '{3}'\r\n                                     WHERE CardNumber = '{2}'", new object[] { this.UseCountCheckBox.Checked ? this.UseCount.Text : "无限制", this.ToDate.Enabled ? this.ToDate.Value.ToString("yyy-MM-dd") : DateTime.Now.AddYears(60).ToString("yyy-MM-dd"), str4, this.Choose_AddToBlacklist.Checked ? "禁止开门" : "允许开门" }));
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message.ToString());
                            Console.WriteLine(exception.ToString());
                        }
                        finally
                        {
                            this.ReadCard.Enabled = true;
                            this.ExtendTheValidity.Enabled = true;
                            this.comm.Close();
                        }
                    };
                }
                this.ExtendTheValidity.Click += handler2;
                if (handler3 == null)
                {
                    handler3 = (DateCheckBox_E, DateCheckBox_Sender) => this.ToDate.Enabled = this.DateCheckBox.Checked;
                }
                this.DateCheckBox.CheckedChanged += handler3;
                if (handler4 == null)
                {
                    handler4 = delegate (object UseCountCheckBoxE, EventArgs UseCountCheckBoxSender) {
                        if (this.UseCountCheckBox.Checked)
                        {
                            this.UseCount.Enabled = true;
                        }
                        else
                        {
                            this.UseCount.Enabled = false;
                            this.UseCount.Text = "无限制";
                        }
                    };
                }
                this.UseCountCheckBox.CheckedChanged += handler4;
                if (handler5 == null)
                {
                    handler5 = delegate (object Choose_ExtendTheValiditySender, EventArgs Choose_ExtendTheValidity_E) {
                        try
                        {
                            if (!this.Choose_ExtendTheValidity.Checked)
                            {
                                this.FamilyChoose.Checked = false;
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message.ToString());
                            Console.WriteLine(exception.ToString());
                        }
                    };
                }
                this.Choose_ExtendTheValidity.CheckedChanged += handler5;
                if (handler6 == null)
                {
                    handler6 = delegate (object FindCard_Sender, EventArgs FindCard_E) {
                        try
                        {
                            for (int m = 0; m < this.dvwShowBlacklist.Rows.Count; m++)
                            {
                                if (this.dvwShowBlacklist.Rows[m].Cells[1].Value.ToString().IndexOf(this.FindCardCode.Text) > -1)
                                {
                                    this.dvwShowBlacklist.Rows[m].Selected = true;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message.ToString());
                            Console.WriteLine(exception.ToString());
                        }
                    };
                }
                this.FindCard.Click += handler6;
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
            this.FamilyChoose = new CheckBox();
            this.dvwShowBlacklist = new DataGridView();
            this.Column1 = new DataGridViewCheckBoxColumn();
            this.Choose_ExtendTheValidity = new RadioButton();
            this.Choose_AddToBlacklist = new RadioButton();
            this.label1 = new Label();
            this.ToDate = new DateTimePicker();
            this.ExtendTheValidity = new Button();
            this.timer1 = new Timer(this.components);
            this.ReadCard = new Button();
            this.UseCount = new TextBox();
            this.DateCheckBox = new CheckBox();
            this.UseCountCheckBox = new CheckBox();
            this.label2 = new Label();
            this.Choose_RemoveToBlacklist = new RadioButton();
            this.label3 = new Label();
            this.label4 = new Label();
            this.FindCard = new Button();
            this.FindCardCode = new TextBox();
            this.DisplayCardCode = new Label();
            this.label5 = new Label();
            ((ISupportInitialize) this.dvwShowBlacklist).BeginInit();
            base.SuspendLayout();
            this.FamilyChoose.AutoSize = true;
            this.FamilyChoose.Location = new Point(13, 10);
            this.FamilyChoose.Name = "FamilyChoose";
            this.FamilyChoose.Size = new Size(0x48, 0x10);
            this.FamilyChoose.TabIndex = 0;
            this.FamilyChoose.Text = "家庭成员";
            this.FamilyChoose.UseVisualStyleBackColor = true;
            this.dvwShowBlacklist.AllowUserToAddRows = false;
            this.dvwShowBlacklist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dvwShowBlacklist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvwShowBlacklist.Columns.AddRange(new DataGridViewColumn[] { this.Column1 });
            this.dvwShowBlacklist.Location = new Point(13, 0x23);
            this.dvwShowBlacklist.Name = "dvwShowBlacklist";
            this.dvwShowBlacklist.RowTemplate.Height = 0x17;
            this.dvwShowBlacklist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dvwShowBlacklist.Size = new Size(0x31d, 0x169);
            this.dvwShowBlacklist.TabIndex = 1;
            this.Column1.FalseValue = "false";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.TrueValue = "true";
            this.Choose_ExtendTheValidity.AutoSize = true;
            this.Choose_ExtendTheValidity.Checked = true;
            this.Choose_ExtendTheValidity.Location = new Point(0x10, 0x192);
            this.Choose_ExtendTheValidity.Name = "Choose_ExtendTheValidity";
            this.Choose_ExtendTheValidity.Size = new Size(0x53, 0x10);
            this.Choose_ExtendTheValidity.TabIndex = 2;
            this.Choose_ExtendTheValidity.TabStop = true;
            this.Choose_ExtendTheValidity.Text = "延长有效期";
            this.Choose_ExtendTheValidity.UseVisualStyleBackColor = true;
            this.Choose_AddToBlacklist.AutoSize = true;
            this.Choose_AddToBlacklist.Location = new Point(0x6b, 0x192);
            this.Choose_AddToBlacklist.Name = "Choose_AddToBlacklist";
            this.Choose_AddToBlacklist.Size = new Size(0x6b, 0x10);
            this.Choose_AddToBlacklist.TabIndex = 2;
            this.Choose_AddToBlacklist.Text = "业主卡加黑名单";
            this.Choose_AddToBlacklist.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x14d, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "有效期结束至";
            this.ToDate.Location = new Point(0x19b, 8);
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new Size(0x67, 0x15);
            this.ToDate.TabIndex = 0x1d;
            this.ExtendTheValidity.Location = new Point(0x2df, 7);
            this.ExtendTheValidity.Name = "ExtendTheValidity";
            this.ExtendTheValidity.Size = new Size(0x4b, 0x17);
            this.ExtendTheValidity.TabIndex = 3;
            this.ExtendTheValidity.Text = "延长有效期";
            this.ExtendTheValidity.UseVisualStyleBackColor = true;
            this.ReadCard.Location = new Point(0x293, 7);
            this.ReadCard.Name = "ReadCard";
            this.ReadCard.Size = new Size(0x4b, 0x17);
            this.ReadCard.TabIndex = 30;
            this.ReadCard.Text = "读卡";
            this.ReadCard.UseVisualStyleBackColor = true;
            this.ReadCard.Click += new EventHandler(this.ReadCard_Click);
            this.UseCount.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.UseCount.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.UseCount.Location = new Point(590, 8);
            this.UseCount.Name = "UseCount";
            this.UseCount.Size = new Size(0x2e, 0x15);
            this.UseCount.TabIndex = 0x1f;
            this.UseCount.Text = "无限制";
            this.DateCheckBox.AutoSize = true;
            this.DateCheckBox.Checked = true;
            this.DateCheckBox.CheckState = CheckState.Checked;
            this.DateCheckBox.Location = new Point(0x206, 11);
            this.DateCheckBox.Name = "DateCheckBox";
            this.DateCheckBox.Size = new Size(15, 14);
            this.DateCheckBox.TabIndex = 0x20;
            this.DateCheckBox.UseVisualStyleBackColor = true;
            this.UseCountCheckBox.AutoSize = true;
            this.UseCountCheckBox.Location = new Point(640, 11);
            this.UseCountCheckBox.Name = "UseCountCheckBox";
            this.UseCountCheckBox.Size = new Size(15, 14);
            this.UseCountCheckBox.TabIndex = 0x20;
            this.UseCountCheckBox.UseVisualStyleBackColor = true;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x218, 12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 0x21;
            this.label2.Text = "使用次数";
            this.Choose_RemoveToBlacklist.AutoSize = true;
            this.Choose_RemoveToBlacklist.Location = new Point(220, 0x193);
            this.Choose_RemoveToBlacklist.Name = "Choose_RemoveToBlacklist";
            this.Choose_RemoveToBlacklist.Size = new Size(0x77, 0x10);
            this.Choose_RemoveToBlacklist.TabIndex = 0x22;
            this.Choose_RemoveToBlacklist.Text = "业主卡移除黑名单";
            this.Choose_RemoveToBlacklist.UseVisualStyleBackColor = true;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x120, 0x193);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0, 12);
            this.label3.TabIndex = 0x23;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xf4, 12);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x23, 12);
            this.label4.TabIndex = 0x24;
            this.label4.Text = "卡号:";
            this.FindCard.Location = new Point(0xbf, 7);
            this.FindCard.Name = "FindCard";
            this.FindCard.Size = new Size(0x37, 0x17);
            this.FindCard.TabIndex = 0x25;
            this.FindCard.Text = "查找";
            this.FindCard.UseVisualStyleBackColor = true;
            this.FindCardCode.Location = new Point(0x74, 8);
            this.FindCardCode.Name = "FindCardCode";
            this.FindCardCode.Size = new Size(70, 0x15);
            this.FindCardCode.TabIndex = 0x26;
            this.DisplayCardCode.AutoSize = true;
            this.DisplayCardCode.Location = new Point(0x115, 12);
            this.DisplayCardCode.Name = "DisplayCardCode";
            this.DisplayCardCode.Size = new Size(0, 12);
            this.DisplayCardCode.TabIndex = 0x27;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x54, 12);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1d, 12);
            this.label5.TabIndex = 40;
            this.label5.Text = "卡号";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x339, 0x1aa);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.DisplayCardCode);
            base.Controls.Add(this.FindCardCode);
            base.Controls.Add(this.FindCard);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.Choose_RemoveToBlacklist);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.UseCountCheckBox);
            base.Controls.Add(this.DateCheckBox);
            base.Controls.Add(this.UseCount);
            base.Controls.Add(this.ReadCard);
            base.Controls.Add(this.ToDate);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.ExtendTheValidity);
            base.Controls.Add(this.Choose_AddToBlacklist);
            base.Controls.Add(this.Choose_ExtendTheValidity);
            base.Controls.Add(this.dvwShowBlacklist);
            base.Controls.Add(this.FamilyChoose);
            base.Name = "Family";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.Tag = "家庭成员 - 用户数量{0}条";
            this.Text = "家庭成员";
            base.Load += new EventHandler(this.Family_Load);
            ((ISupportInitialize) this.dvwShowBlacklist).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ReadCard_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReadCard.Enabled = false;
                this.ExtendTheValidity.Enabled = false;
                string str = Function.ReadCardInformation(this.comm, this.portname);
                if (str.Split(new char[] { '-' }).Length < 20)
                {
                    throw new Exception("卡数据读取失败!");
                }
                if (str.Split(new char[] { '-' })[15] == "00")
                {
                    this.ToDate.Enabled = false;
                    this.DateCheckBox.Checked = false;
                }
                else
                {
                    this.ToDate.Enabled = true;
                    this.DateCheckBox.Checked = true;
                    this.ToDate.Value = new DateTime(int.Parse(str.Split(new char[] { '-' })[0x12]) + 0x7d0, int.Parse(str.Split(new char[] { '-' })[0x13]), int.Parse(str.Split(new char[] { '-' })[20]));
                }
                if (str.Split(new char[] { '-' })[0x11] != "FF")
                {
                    this.UseCount.Enabled = true;
                    this.UseCountCheckBox.Checked = true;
                    this.UseCount.Text = Function.String2Byte(str.Split(new char[] { '-' })[0x11]).ToString();
                }
                else
                {
                    this.UseCountCheckBox.Checked = false;
                    this.UseCount.Enabled = false;
                }
                this.DisplayCardCode.Text = Function.ReadCardCode(this.comm, this.portname);
                Function.Beel(this.comm, this.portname);
                this.ReadCard.Enabled = true;
                this.ExtendTheValidity.Enabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                MessageBox.Show(exception.Message.ToString(), "错误!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                this.comm.Close();
                this.ReadCard.Enabled = true;
                this.ExtendTheValidity.Enabled = true;
            }
        }

        private void SelectFamilyCard()
        {
            int num = 0;
            foreach (DataGridViewRow row in (IEnumerable) this.dvwShowBlacklist.Rows)
            {
                if ((((row.Cells[4].Value.ToString() == this.FamilyParentForm.txtAreaName.Text) && (row.Cells[5].Value.ToString() == this.FamilyParentForm.txtBuildingNo.Text)) && (row.Cells[6].Value.ToString() == this.FamilyParentForm.txtRoomNumber.Text)) && (row.Cells[1].Value.ToString() != this.FamilyParentForm.txtCardNumber.Text))
                {
                    row.Cells[0].Value = true;
                    num++;
                }
            }
            if (num > 10)
            {
                MessageBox.Show("卡超过10张!", "警告!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}

