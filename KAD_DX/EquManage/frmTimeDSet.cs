namespace KAD_DX.EquManage
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using KAD_DX.BaseClass;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmTimeDSet : Form
    {
        private BaseOperate boperate = new BaseOperate();
        private SimpleButton btnExit;
        private SimpleButton btnSure;
        private ComboBoxEdit cboxTimeD;
        private CheckEdit chkFri;
        private CheckEdit chkMon;
        private CheckEdit chkSat;
        private CheckEdit chkSun;
        private CheckEdit chkThurs;
        private CheckEdit chkTues;
        private CheckEdit chkWed;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ImageCollection imageCollection2;
        private Label label1;
        private Label label10;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        protected string M_str_sql = "select TimeName,TimeID,TimeStart1,TimeEnd1,TimeStart2,TimeEnd2,TimeStart3,TimeEnd3,TimeStart4,TimeEnd4,TimeMon,TimeTues,TimeWed,TimeThurs,TimeFri,TimeSat,TimeSun from tb_TimeD";
        private OperateAndValidate opAndvalidate = new OperateAndValidate();
        private TimeEdit timeEnd1;
        private TimeEdit timeEnd2;
        private TimeEdit timeEnd3;
        private TimeEdit timeEnd4;
        private TimeEdit timeStart1;
        private TimeEdit timeStart2;
        private TimeEdit timeStart3;
        private TimeEdit timeStart4;

        public frmTimeDSet()
        {
            this.InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            string[] strArray = new string[7];
            string str = this.cboxTimeD.Text.Trim();
            TextBox txt = new TextBox();
            for (int i = 0; i < 7; i++)
            {
                strArray[i] = "N";
            }
            if (this.chkMon.Checked)
            {
                strArray[0] = "Y";
            }
            if (this.chkTues.Checked)
            {
                strArray[1] = "Y";
            }
            if (this.chkWed.Checked)
            {
                strArray[2] = "Y";
            }
            if (this.chkThurs.Checked)
            {
                strArray[3] = "Y";
            }
            if (this.chkFri.Checked)
            {
                strArray[4] = "Y";
            }
            if (this.chkSat.Checked)
            {
                strArray[5] = "Y";
            }
            if (this.chkSun.Checked)
            {
                strArray[6] = "Y";
            }
            if (this.boperate.getread("select * from tb_TimeD where TimeName='" + str + "'").HasRows)
            {
                this.boperate.getcom("update tb_TimeD set TimeStart1='" + this.timeStart1.Text + "',TimeEnd1='" + this.timeEnd1.Text + "',TimeStart2='" + this.timeStart2.Text + "',TimeEnd2='" + this.timeEnd2.Text + "',TimeStart3='" + this.timeStart3.Text + "',TimeEnd3='" + this.timeEnd3.Text + "',TimeStart4='" + this.timeStart4.Text + "',TimeEnd4='" + this.timeEnd4.Text + "',TimeMon='" + strArray[0] + "',TimeTues='" + strArray[1] + "',TimeWed='" + strArray[2] + "',TimeThurs='" + strArray[3] + "',TimeFri='" + strArray[4] + "',TimeSat='" + strArray[5] + "',TimeSun='" + strArray[6] + "' where TimeName='" + str + "'");
            }
            else
            {
                this.opAndvalidate.autoNum("select max(TimeID) from tb_TimeD", "tb_TimeD", "TimeID", "SJ", "1000001", txt);
                this.boperate.getcom("insert into tb_TimeD(TimeID,TimeStart1,TimeEnd1,TimeStart2,TimeEnd2,TimeStart3,TimeEnd3,TimeStart4,TimeEnd4,TimeMon,TimeTues,TimeWed,TimeThurs,TimeFri,TimeSat,TimeSun,TimeName) values('" + txt.Text.Trim() + "','" + this.timeStart1.Text + "','" + this.timeEnd1.Text + "','" + this.timeStart2.Text + "','" + this.timeEnd2.Text + "','" + this.timeStart3.Text + "','" + this.timeEnd3.Text + "','" + this.timeStart4.Text + "','" + this.timeEnd4.Text + "','" + strArray[0] + "','" + strArray[1] + "','" + strArray[2] + "','" + strArray[3] + "','" + strArray[4] + "','" + strArray[5] + "','" + strArray[6] + "','" + str + "')");
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void cboxTimeD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTimeName = this.cboxTimeD.SelectedItem.ToString();
            this.showTimeValue(strTimeName);
        }

        private void chkThurs_CheckedChanged(object sender, EventArgs e)
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

        private void frmTimeDSet_Load(object sender, EventArgs e)
        {
            foreach (object obj2 in this.groupBox2.Controls)
            {
                if (obj2 is TimeEdit)
                {
                    TimeEdit edit = (TimeEdit) obj2;
                    edit.Properties.Mask.EditMask = "HH:mm";
                }
            }
            this.cboxTimeD.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmTimeDSet));
            this.cboxTimeD = new ComboBoxEdit();
            this.label1 = new Label();
            this.groupBox1 = new GroupBox();
            this.chkSun = new CheckEdit();
            this.chkSat = new CheckEdit();
            this.chkFri = new CheckEdit();
            this.chkThurs = new CheckEdit();
            this.chkWed = new CheckEdit();
            this.chkTues = new CheckEdit();
            this.chkMon = new CheckEdit();
            this.groupBox2 = new GroupBox();
            this.btnExit = new SimpleButton();
            this.imageCollection2 = new ImageCollection(this.components);
            this.label9 = new Label();
            this.btnSure = new SimpleButton();
            this.timeEnd4 = new TimeEdit();
            this.label8 = new Label();
            this.timeStart4 = new TimeEdit();
            this.label7 = new Label();
            this.timeEnd3 = new TimeEdit();
            this.label6 = new Label();
            this.timeStart3 = new TimeEdit();
            this.label5 = new Label();
            this.timeEnd2 = new TimeEdit();
            this.label4 = new Label();
            this.timeStart2 = new TimeEdit();
            this.label3 = new Label();
            this.timeEnd1 = new TimeEdit();
            this.label2 = new Label();
            this.timeStart1 = new TimeEdit();
            this.label10 = new Label();
            this.cboxTimeD.Properties.BeginInit();
            this.groupBox1.SuspendLayout();
            this.chkSun.Properties.BeginInit();
            this.chkSat.Properties.BeginInit();
            this.chkFri.Properties.BeginInit();
            this.chkThurs.Properties.BeginInit();
            this.chkWed.Properties.BeginInit();
            this.chkTues.Properties.BeginInit();
            this.chkMon.Properties.BeginInit();
            this.groupBox2.SuspendLayout();
            this.imageCollection2.BeginInit();
            this.timeEnd4.Properties.BeginInit();
            this.timeStart4.Properties.BeginInit();
            this.timeEnd3.Properties.BeginInit();
            this.timeStart3.Properties.BeginInit();
            this.timeEnd2.Properties.BeginInit();
            this.timeStart2.Properties.BeginInit();
            this.timeEnd1.Properties.BeginInit();
            this.timeStart1.Properties.BeginInit();
            base.SuspendLayout();
            this.cboxTimeD.Location = new Point(0x67, 0x12);
            this.cboxTimeD.Name = "cboxTimeD";
            this.cboxTimeD.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cboxTimeD.Properties.Items.AddRange(new object[] { "时间段1", "时间段2", "时间段3", "时间段4" });
            this.cboxTimeD.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.cboxTimeD.Size = new Size(0x92, 0x15);
            this.cboxTimeD.TabIndex = 0;
            this.cboxTimeD.SelectedIndexChanged += new EventHandler(this.cboxTimeD_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x15, 0x17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "时间段编号";
            this.groupBox1.Controls.Add(this.chkSun);
            this.groupBox1.Controls.Add(this.chkSat);
            this.groupBox1.Controls.Add(this.chkFri);
            this.groupBox1.Controls.Add(this.chkThurs);
            this.groupBox1.Controls.Add(this.chkWed);
            this.groupBox1.Controls.Add(this.chkTues);
            this.groupBox1.Controls.Add(this.chkMon);
            this.groupBox1.Location = new Point(0x17, 0x39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x21d, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "星期选择";
            this.chkSun.Location = new Point(0x1b7, 0x27);
            this.chkSun.Name = "chkSun";
            this.chkSun.Properties.Appearance.ForeColor = Color.FromArgb(0xc0, 0, 0xc0);
            this.chkSun.Properties.Appearance.Options.UseForeColor = true;
            this.chkSun.Properties.Caption = "星期天";
            this.chkSun.Size = new Size(0x51, 0x13);
            this.chkSun.TabIndex = 6;
            this.chkSat.Location = new Point(0x173, 0x27);
            this.chkSat.Name = "chkSat";
            this.chkSat.Properties.Appearance.ForeColor = Color.FromArgb(0xc0, 0, 0xc0);
            this.chkSat.Properties.Appearance.Options.UseForeColor = true;
            this.chkSat.Properties.Caption = "星期六";
            this.chkSat.Size = new Size(100, 0x13);
            this.chkSat.TabIndex = 5;
            this.chkFri.Location = new Point(0x12f, 0x27);
            this.chkFri.Name = "chkFri";
            this.chkFri.Properties.Caption = "星期五";
            this.chkFri.Size = new Size(100, 0x13);
            this.chkFri.TabIndex = 4;
            this.chkThurs.Location = new Point(0xec, 0x27);
            this.chkThurs.Name = "chkThurs";
            this.chkThurs.Properties.Caption = "星期四";
            this.chkThurs.Size = new Size(100, 0x13);
            this.chkThurs.TabIndex = 3;
            this.chkThurs.CheckedChanged += new EventHandler(this.chkThurs_CheckedChanged);
            this.chkWed.Location = new Point(170, 0x27);
            this.chkWed.Name = "chkWed";
            this.chkWed.Properties.Caption = "星期三";
            this.chkWed.Size = new Size(100, 0x13);
            this.chkWed.TabIndex = 2;
            this.chkTues.Location = new Point(0x5f, 0x27);
            this.chkTues.Name = "chkTues";
            this.chkTues.Properties.Caption = "星期二";
            this.chkTues.Size = new Size(100, 0x13);
            this.chkTues.TabIndex = 1;
            this.chkMon.Location = new Point(0x15, 0x27);
            this.chkMon.Name = "chkMon";
            this.chkMon.Properties.Caption = "星期一";
            this.chkMon.Size = new Size(100, 0x13);
            this.chkMon.TabIndex = 0;
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnSure);
            this.groupBox2.Controls.Add(this.timeEnd4);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.timeStart4);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.timeEnd3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.timeStart3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.timeEnd2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.timeStart2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.timeEnd1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.timeStart1);
            this.groupBox2.Location = new Point(0x17, 0xa3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x21d, 0xbb);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "时间段选择";
            this.btnExit.ImageIndex = 1;
            this.btnExit.ImageList = this.imageCollection2;
            this.btnExit.Location = new Point(0x18c, 0x76);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(0x4b, 0x1f);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "退  出";
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.imageCollection2.ImageStream = (ImageCollectionStreamer) manager.GetObject("imageCollection2.ImageStream");
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0xb8, 0x9f);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x23, 12);
            this.label9.TabIndex = 0x12;
            this.label9.Text = "---->";
            this.btnSure.ImageIndex = 2;
            this.btnSure.ImageList = this.imageCollection2;
            this.btnSure.Location = new Point(0x18c, 0x3a);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new Size(0x4b, 0x1f);
            this.btnSure.TabIndex = 7;
            this.btnSure.Text = "确  定";
            this.btnSure.Click += new EventHandler(this.btnSure_Click);
            this.timeEnd4.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeEnd4.Location = new Point(0xec, 0x9b);
            this.timeEnd4.Name = "timeEnd4";
            this.timeEnd4.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeEnd4.Size = new Size(100, 0x15);
            this.timeEnd4.TabIndex = 0x11;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(6, 0xa4);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x2f, 12);
            this.label8.TabIndex = 0x10;
            this.label8.Text = "时区4：";
            this.timeStart4.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeStart4.Location = new Point(0x3a, 160);
            this.timeStart4.Name = "timeStart4";
            this.timeStart4.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeStart4.Size = new Size(100, 0x15);
            this.timeStart4.TabIndex = 15;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0xb8, 0x77);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x23, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "---->";
            this.timeEnd3.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeEnd3.Location = new Point(0xec, 0x73);
            this.timeEnd3.Name = "timeEnd3";
            this.timeEnd3.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeEnd3.Size = new Size(100, 0x15);
            this.timeEnd3.TabIndex = 13;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(6, 120);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x2f, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "时区3：";
            this.timeStart3.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeStart3.Location = new Point(0x3a, 0x74);
            this.timeStart3.Name = "timeStart3";
            this.timeStart3.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeStart3.Size = new Size(100, 0x15);
            this.timeStart3.TabIndex = 11;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0xba, 0x4d);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x23, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "---->";
            this.timeEnd2.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeEnd2.Location = new Point(0xee, 0x49);
            this.timeEnd2.Name = "timeEnd2";
            this.timeEnd2.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeEnd2.Size = new Size(100, 0x15);
            this.timeEnd2.TabIndex = 9;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(6, 0x4d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x2f, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "时区2：";
            this.timeStart2.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeStart2.Location = new Point(0x3a, 0x49);
            this.timeStart2.Name = "timeStart2";
            this.timeStart2.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeStart2.Size = new Size(100, 0x15);
            this.timeStart2.TabIndex = 7;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xba, 0x22);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x23, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "---->";
            this.timeEnd1.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeEnd1.Location = new Point(0xee, 30);
            this.timeEnd1.Name = "timeEnd1";
            this.timeEnd1.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeEnd1.Size = new Size(100, 0x15);
            this.timeEnd1.TabIndex = 5;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 0x21);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2f, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "时区1：";
            this.timeStart1.EditValue = new DateTime(0x7db, 5, 0x1f, 0, 0, 0, 0);
            this.timeStart1.Location = new Point(0x3a, 0x1d);
            this.timeStart1.Name = "timeStart1";
            this.timeStart1.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.timeStart1.Size = new Size(100, 0x15);
            this.timeStart1.TabIndex = 0;
            this.label10.AutoSize = true;
            this.label10.ForeColor = Color.Fuchsia;
            this.label10.Location = new Point(0x112, 0x17);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x10d, 12);
            this.label10.TabIndex = 0x13;
            this.label10.Text = "时间段1默认为禁止进入，时间段2默认为允许进入";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x241, 0x187);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cboxTimeD);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmTimeDSet";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "时间段设置";
            base.Load += new EventHandler(this.frmTimeDSet_Load);
            this.cboxTimeD.Properties.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.chkSun.Properties.EndInit();
            this.chkSat.Properties.EndInit();
            this.chkFri.Properties.EndInit();
            this.chkThurs.Properties.EndInit();
            this.chkWed.Properties.EndInit();
            this.chkTues.Properties.EndInit();
            this.chkMon.Properties.EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.imageCollection2.EndInit();
            this.timeEnd4.Properties.EndInit();
            this.timeStart4.Properties.EndInit();
            this.timeEnd3.Properties.EndInit();
            this.timeStart3.Properties.EndInit();
            this.timeEnd2.Properties.EndInit();
            this.timeStart2.Properties.EndInit();
            this.timeEnd1.Properties.EndInit();
            this.timeStart1.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void showTimeValue(string strTimeName)
        {
            OleDbDataReader reader = this.boperate.getread(this.M_str_sql + " where TimeName='" + strTimeName + "'");
            if (reader.HasRows)
            {
                reader.Read();
                this.timeStart1.EditValue = reader["TimeStart1"].ToString().Trim();
                this.timeEnd1.EditValue = reader["TimeEnd1"].ToString().Trim();
                this.timeStart2.EditValue = reader["TimeStart2"].ToString().Trim();
                this.timeEnd2.EditValue = reader["TimeEnd2"].ToString().Trim();
                this.timeStart3.EditValue = reader["TimeStart3"].ToString().Trim();
                this.timeEnd3.EditValue = reader["TimeEnd3"].ToString().Trim();
                this.timeStart4.EditValue = reader["TimeStart4"].ToString().Trim();
                this.timeEnd4.EditValue = reader["TimeEnd4"].ToString().Trim();
                if (reader["TimeMon"].ToString().Trim() == "Y")
                {
                    this.chkMon.Checked = true;
                }
                else
                {
                    this.chkMon.Checked = false;
                }
                if (reader["TimeTues"].ToString().Trim() == "Y")
                {
                    this.chkTues.Checked = true;
                }
                else
                {
                    this.chkTues.Checked = false;
                }
                if (reader["TimeWed"].ToString().Trim() == "Y")
                {
                    this.chkWed.Checked = true;
                }
                else
                {
                    this.chkWed.Checked = false;
                }
                if (reader["TimeThurs"].ToString().Trim() == "Y")
                {
                    this.chkThurs.Checked = true;
                }
                else
                {
                    this.chkThurs.Checked = false;
                }
                if (reader["TimeFri"].ToString().Trim() == "Y")
                {
                    this.chkFri.Checked = true;
                }
                else
                {
                    this.chkFri.Checked = false;
                }
                if (reader["TimeSat"].ToString().Trim() == "Y")
                {
                    this.chkSat.Checked = true;
                }
                else
                {
                    this.chkSat.Checked = false;
                }
                if (reader["TimeSun"].ToString().Trim() == "Y")
                {
                    this.chkSun.Checked = true;
                }
                else
                {
                    this.chkSun.Checked = false;
                }
            }
            else
            {
                foreach (object obj2 in this.groupBox2.Controls)
                {
                    if (obj2 is TimeEdit)
                    {
                        TimeEdit edit = (TimeEdit) obj2;
                        edit.EditValue = "00:00";
                    }
                }
                foreach (object obj2 in this.groupBox1.Controls)
                {
                    if (obj2 is CheckEdit)
                    {
                        CheckEdit edit2 = (CheckEdit) obj2;
                        edit2.Checked = false;
                    }
                }
            }
        }
    }
}

