namespace KAD_DX.EquManage
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Card;
    using DevExpress.XtraGrid.Views.Grid;
    using KAD_DX.BaseClass;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmEquTimeD : Form
    {
        private bool bolDelTimeD = false;
        private bool bolReLoad = false;
        private BaseOperate boperate = new BaseOperate();
        private SimpleButton btnAdd;
        private SimpleButton btnDel;
        private SimpleButton btnDown;
        private SimpleButton btnExcelOut;
        private SimpleButton btnExit;
        private SimpleButton btnRead;
        private SimpleButton btnSelAll;
        private SimpleButton btnSpeicalTime;
        private RepositoryItemCheckEdit chkFri;
        private RepositoryItemCheckEdit chkMon;
        private RepositoryItemCheckEdit chkSat;
        private RepositoryItemCheckEdit chkSun;
        private RepositoryItemCheckEdit chkThurs;
        private RepositoryItemCheckEdit chkTues;
        private RepositoryItemCheckEdit chkWed;
        private GridColumn colCommand;
        private GridColumn colEnd1;
        private GridColumn colEnd2;
        private GridColumn colEnd3;
        private GridColumn colEnd4;
        private GridColumn colFri;
        private GridColumn colID;
        private GridColumn colMon;
        private GridColumn colResult;
        private GridColumn colSat;
        private GridColumn colStart1;
        private GridColumn colStart2;
        private GridColumn colStart3;
        private GridColumn colStart4;
        private GridColumn colSun;
        private GridColumn colTarget;
        private GridColumn colThurs;
        private GridColumn colTime;
        private GridColumn colTues;
        private GridColumn colWed;
        private IContainer components = null;
        private GridView dgvExecute;
        private CardView dgvUInfo;
        private GridControl GridControl1;
        private GridControl GridControl2;
        private ImageCollection imageCollection1;
        private ImageCollection imageCollection2;
        private ImageListBoxControl imgList;
        private int intFocusHandle;
        private int intRowHandleValue;
        private bool M_bol_value = false;
        protected int M_int_judge;
        protected string M_str_sql = "select TimeID,TimeStart1,TimeEnd1,TimeStart2,TimeEnd2,TimeStart3,TimeEnd3,TimeStart4,TimeEnd4,TimeMon,TimeTues,TimeWed,TimeThurs,TimeFri,TimeSat,TimeSun,TimeName from tb_TimeD order by TimeName";
        protected string M_str_table = "tb_TimeD";
        private int nToltal;
        private OperateAndValidate opAndvalidate = new OperateAndValidate();
        private OperateComm opComm = new OperateComm();
        private PanelControl panelControl1;
        private PanelControl panelControl2;
        private PanelControl panelControl3;
        private PanelControl panelControl4;
        private RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private string[] strAddCount = new string[0x3e7];
        private string[] strAddress = new string[0x3e7];
        private string strHandleID;
        private string[] strLocation = new string[0x3e7];
        private RepositoryItemTimeEdit teEnd1;
        private RepositoryItemTimeEdit teEnd2;
        private RepositoryItemTimeEdit teEnd3;
        private RepositoryItemTimeEdit teEnd4;
        private RepositoryItemTimeEdit teStart1;
        private RepositoryItemTimeEdit teStart2;
        private RepositoryItemTimeEdit teStart3;
        private RepositoryItemTimeEdit teStart4;
        private System.Windows.Forms.Timer timeReceive;
        private RepositoryItemTextEdit txtEnd1;
        private RepositoryItemTextEdit txtEnd2;
        private RepositoryItemTextEdit txtEnd3;
        private RepositoryItemTextEdit txtEnd4;
        private RepositoryItemTextEdit txtStart1;
        private RepositoryItemTextEdit txtStart2;
        private RepositoryItemTextEdit txtStart3;
        private RepositoryItemTextEdit txtStart4;
        private TextBox txtUserID = new TextBox();

        public frmEquTimeD()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmTimeDSet set = new frmTimeDSet();
            set.ShowDialog();
            if (set.DialogResult == DialogResult.OK)
            {
                DataSet set2 = this.boperate.getds(this.M_str_sql, this.M_str_table);
                this.GridControl1.DataSource = set2.Tables[0];
                this.dgvUInfo.OptionsView.ShowQuickCustomizeButton = false;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if ((this.dgvUInfo.SelectedRowsCount > 0) && (MessageBox.Show("你确定要删除选中的记录吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, 0, false) == DialogResult.Yes))
            {
                this.boperate.getcom("delete from tb_TimeD where TimeID='" + this.txtUserID.Text.Trim() + "'");
                this.dgvUInfo.DeleteSelectedRows();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                this.SendData("DownTimeD");
            }
            catch
            {
                MessageBox.Show("请先设置时间段再下载,不能为ff", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnExcelOut_Click(object sender, EventArgs e)
        {
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            this.SendData("UpdataTimeD");
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            this.imgList.SelectAll();
        }

        private void btnSpeicalTime_Click(object sender, EventArgs e)
        {
        }

        private void dgvUInfo_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            this.M_bol_value = true;
            this.intRowHandleValue = e.RowHandle;
            this.strHandleID = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeID").ToString();
        }

        private void dgvUInfo_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int focusedRowHandle = e.FocusedRowHandle;
            if (!this.bolReLoad)
            {
                this.intFocusHandle = focusedRowHandle;
            }
            object rowCellValue = this.dgvUInfo.GetRowCellValue(focusedRowHandle, "TimeID");
            try
            {
                this.txtUserID.Text = rowCellValue.ToString();
            }
            catch
            {
            }
            if (DBNull.Value != rowCellValue)
            {
            }
            if (this.M_bol_value)
            {
                this.M_bol_value = false;
                string str = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeStart1").ToString();
                string str2 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeEnd1").ToString();
                string str3 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeStart2").ToString();
                string str4 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeEnd2").ToString();
                string str5 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeStart3").ToString();
                string str6 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeEnd3").ToString();
                string str7 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeStart4").ToString();
                string str8 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeEnd4").ToString();
                string str9 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeMon").ToString();
                string str10 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeTues").ToString();
                string str11 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeWed").ToString();
                string str12 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeThurs").ToString();
                string str13 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeFri").ToString();
                string str14 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeSat").ToString();
                string str15 = this.dgvUInfo.GetRowCellValue(this.intRowHandleValue, "TimeSun").ToString();
                if (str.Length > 10)
                {
                    str = str.Substring(11, 5);
                }
                if (str2.Length > 10)
                {
                    str2 = str2.Substring(11, 5);
                }
                if (str3.Length > 10)
                {
                    str3 = str3.Substring(11, 5);
                }
                if (str4.Length > 10)
                {
                    str4 = str4.Substring(11, 5);
                }
                if (str5.Length > 10)
                {
                    str5 = str5.Substring(11, 5);
                }
                if (str6.Length > 10)
                {
                    str6 = str6.Substring(11, 5);
                }
                if (str7.Length > 10)
                {
                    str7 = str7.Substring(11, 5);
                }
                if (str8.Length > 10)
                {
                    str8 = str8.Substring(11, 5);
                }
                this.boperate.getcom("update tb_TimeD set TimeStart1='" + str + "',TimeEnd1='" + str2 + "',TimeStart2='" + str3 + "',TimeEnd2='" + str4 + "',TimeStart3='" + str5 + "',TimeEnd3='" + str6 + "',TimeStart4='" + str7 + "',TimeEnd4='" + str8 + "',TimeMon='" + str9 + "',TimeTues='" + str10 + "',TimeWed='" + str11 + "',TimeThurs='" + str12 + "',TimeFri='" + str13 + "',TimeSat='" + str14 + "',TimeSun='" + str15 + "' where TimeID='" + this.strHandleID + "'");
                DataSet set = this.boperate.getds(this.M_str_sql, this.M_str_table);
                this.GridControl1.DataSource = set.Tables[0];
                this.dgvUInfo.FocusedRowHandle = focusedRowHandle;
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

        private void frmEquTimeD_Load(object sender, EventArgs e)
        {
            string str = "select EquID,EquAddress,EquFloor,EquLocation,EquCount from tb_Equ";
            this.imgList.Items.Clear();
            OleDbDataReader reader = this.boperate.getread(str);
            this.nToltal = 0;
            while (reader.Read())
            {
                this.strLocation[this.nToltal] = reader["EquLocation"].ToString().Trim();
                this.strAddress[this.nToltal] = reader["EquAddress"].ToString().Trim();
                this.strAddCount[this.nToltal] = reader["EquCount"].ToString().Trim();
                this.imgList.Items.Add(this.strLocation[this.nToltal]);
                this.imgList.Items[this.nToltal].ImageIndex = 0;
                this.nToltal++;
            }
            this.boperate.getcom("delete * from tb_Execute");
            str = "select ExID,ExTime,ExCmd,ExTarget,ExResult from tb_Execute";
            DataSet set = this.boperate.getds(str, "tb_Execute");
            this.GridControl2.DataSource = set.Tables[0];
            this.dgvExecute.OptionsBehavior.Editable = false;
            this.dgvExecute.OptionsView.ShowGroupPanel = false;
            this.dgvExecute.Columns[0].FieldName = "ExTime";
            this.dgvExecute.Columns[1].FieldName = "ExCmd";
            this.dgvExecute.Columns[2].FieldName = "ExTarget";
            this.dgvExecute.Columns[3].FieldName = "ExResult";
            set = this.boperate.getds(this.M_str_sql, this.M_str_table);
            this.GridControl1.DataSource = set.Tables[0];
            this.dgvUInfo.OptionsView.ShowQuickCustomizeButton = false;
            this.dgvUInfo.Columns[0].FieldName = "TimeName";
            this.dgvUInfo.Columns[1].FieldName = "TimeStart1";
            this.dgvUInfo.Columns[2].FieldName = "TimeEnd1";
            this.dgvUInfo.Columns[3].FieldName = "TimeStart2";
            this.dgvUInfo.Columns[4].FieldName = "TimeEnd2";
            this.dgvUInfo.Columns[5].FieldName = "TimeStart3";
            this.dgvUInfo.Columns[6].FieldName = "TimeEnd3";
            this.dgvUInfo.Columns[7].FieldName = "TimeStart4";
            this.dgvUInfo.Columns[8].FieldName = "TimeEnd4";
            this.dgvUInfo.Columns[9].FieldName = "TimeMon";
            this.dgvUInfo.Columns[10].FieldName = "TimeTues";
            this.dgvUInfo.Columns[11].FieldName = "TimeWed";
            this.dgvUInfo.Columns[12].FieldName = "TimeThurs";
            this.dgvUInfo.Columns[13].FieldName = "TimeFri";
            this.dgvUInfo.Columns[14].FieldName = "TimeSat";
            this.dgvUInfo.Columns[15].FieldName = "TimeSun";
            if (set.Tables[0].Rows.Count > 0)
            {
                this.btnDel.Enabled = true;
            }
            else
            {
                this.btnDel.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmEquTimeD));
            this.teStart1 = new RepositoryItemTimeEdit();
            this.teEnd1 = new RepositoryItemTimeEdit();
            this.teStart2 = new RepositoryItemTimeEdit();
            this.teEnd2 = new RepositoryItemTimeEdit();
            this.teStart3 = new RepositoryItemTimeEdit();
            this.teEnd3 = new RepositoryItemTimeEdit();
            this.teStart4 = new RepositoryItemTimeEdit();
            this.teEnd4 = new RepositoryItemTimeEdit();
            this.chkMon = new RepositoryItemCheckEdit();
            this.chkTues = new RepositoryItemCheckEdit();
            this.chkWed = new RepositoryItemCheckEdit();
            this.chkThurs = new RepositoryItemCheckEdit();
            this.chkFri = new RepositoryItemCheckEdit();
            this.chkSat = new RepositoryItemCheckEdit();
            this.chkSun = new RepositoryItemCheckEdit();
            this.imageCollection2 = new ImageCollection(this.components);
            this.panelControl1 = new PanelControl();
            this.btnExcelOut = new SimpleButton();
            this.btnSpeicalTime = new SimpleButton();
            this.btnSelAll = new SimpleButton();
            this.btnDown = new SimpleButton();
            this.btnRead = new SimpleButton();
            this.btnExit = new SimpleButton();
            this.btnDel = new SimpleButton();
            this.btnAdd = new SimpleButton();
            this.GridControl1 = new GridControl();
            this.dgvUInfo = new CardView();
            this.colID = new GridColumn();
            this.colStart1 = new GridColumn();
            this.repositoryItemTimeEdit1 = new RepositoryItemTimeEdit();
            this.colEnd1 = new GridColumn();
            this.txtEnd1 = new RepositoryItemTextEdit();
            this.colStart2 = new GridColumn();
            this.txtStart2 = new RepositoryItemTextEdit();
            this.colEnd2 = new GridColumn();
            this.txtEnd2 = new RepositoryItemTextEdit();
            this.colStart3 = new GridColumn();
            this.txtStart3 = new RepositoryItemTextEdit();
            this.colEnd3 = new GridColumn();
            this.txtEnd3 = new RepositoryItemTextEdit();
            this.colStart4 = new GridColumn();
            this.txtStart4 = new RepositoryItemTextEdit();
            this.colEnd4 = new GridColumn();
            this.txtEnd4 = new RepositoryItemTextEdit();
            this.colMon = new GridColumn();
            this.colTues = new GridColumn();
            this.colWed = new GridColumn();
            this.colThurs = new GridColumn();
            this.colFri = new GridColumn();
            this.colSat = new GridColumn();
            this.colSun = new GridColumn();
            this.txtStart1 = new RepositoryItemTextEdit();
            this.panelControl2 = new PanelControl();
            this.imgList = new ImageListBoxControl();
            this.imageCollection1 = new ImageCollection(this.components);
            this.panelControl3 = new PanelControl();
            this.timeReceive = new System.Windows.Forms.Timer(this.components);
            this.panelControl4 = new PanelControl();
            this.GridControl2 = new GridControl();
            this.dgvExecute = new GridView();
            this.colTime = new GridColumn();
            this.colCommand = new GridColumn();
            this.colTarget = new GridColumn();
            this.colResult = new GridColumn();
            this.teStart1.BeginInit();
            this.teEnd1.BeginInit();
            this.teStart2.BeginInit();
            this.teEnd2.BeginInit();
            this.teStart3.BeginInit();
            this.teEnd3.BeginInit();
            this.teStart4.BeginInit();
            this.teEnd4.BeginInit();
            this.chkMon.BeginInit();
            this.chkTues.BeginInit();
            this.chkWed.BeginInit();
            this.chkThurs.BeginInit();
            this.chkFri.BeginInit();
            this.chkSat.BeginInit();
            this.chkSun.BeginInit();
            this.imageCollection2.BeginInit();
            this.panelControl1.BeginInit();
            this.panelControl1.SuspendLayout();
            this.GridControl1.BeginInit();
            this.dgvUInfo.BeginInit();
            this.repositoryItemTimeEdit1.BeginInit();
            this.txtEnd1.BeginInit();
            this.txtStart2.BeginInit();
            this.txtEnd2.BeginInit();
            this.txtStart3.BeginInit();
            this.txtEnd3.BeginInit();
            this.txtStart4.BeginInit();
            this.txtEnd4.BeginInit();
            this.txtStart1.BeginInit();
            this.panelControl2.BeginInit();
            this.panelControl2.SuspendLayout();
            ((ISupportInitialize) this.imgList).BeginInit();
            this.imageCollection1.BeginInit();
            this.panelControl3.BeginInit();
            this.panelControl3.SuspendLayout();
            this.panelControl4.BeginInit();
            this.panelControl4.SuspendLayout();
            this.GridControl2.BeginInit();
            this.dgvExecute.BeginInit();
            base.SuspendLayout();
            this.teStart1.AutoHeight = false;
            this.teStart1.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teStart1.Name = "teStart1";
            this.teEnd1.AutoHeight = false;
            this.teEnd1.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teEnd1.Name = "teEnd1";
            this.teStart2.AutoHeight = false;
            this.teStart2.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teStart2.Name = "teStart2";
            this.teEnd2.AutoHeight = false;
            this.teEnd2.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teEnd2.Name = "teEnd2";
            this.teStart3.AutoHeight = false;
            this.teStart3.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teStart3.Name = "teStart3";
            this.teEnd3.AutoHeight = false;
            this.teEnd3.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teEnd3.Name = "teEnd3";
            this.teStart4.AutoHeight = false;
            this.teStart4.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teStart4.Name = "teStart4";
            this.teEnd4.AutoHeight = false;
            this.teEnd4.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.teEnd4.Name = "teEnd4";
            this.chkMon.AutoHeight = false;
            this.chkMon.Name = "chkMon";
            this.chkMon.ValueChecked = "Y";
            this.chkMon.ValueUnchecked = "N";
            this.chkTues.AutoHeight = false;
            this.chkTues.Name = "chkTues";
            this.chkTues.ValueChecked = "Y";
            this.chkTues.ValueUnchecked = "N";
            this.chkWed.AutoHeight = false;
            this.chkWed.Name = "chkWed";
            this.chkWed.ValueChecked = "Y";
            this.chkWed.ValueUnchecked = "N";
            this.chkThurs.AutoHeight = false;
            this.chkThurs.Name = "chkThurs";
            this.chkThurs.ValueChecked = "Y";
            this.chkThurs.ValueUnchecked = "N";
            this.chkFri.AutoHeight = false;
            this.chkFri.Name = "chkFri";
            this.chkFri.ValueChecked = "Y";
            this.chkFri.ValueUnchecked = "N";
            this.chkSat.AutoHeight = false;
            this.chkSat.Name = "chkSat";
            this.chkSat.ValueChecked = "Y";
            this.chkSat.ValueUnchecked = "N";
            this.chkSun.AutoHeight = false;
            this.chkSun.Name = "chkSun";
            this.chkSun.ValueChecked = "Y";
            this.chkSun.ValueUnchecked = "N";
            this.imageCollection2.ImageStream = (ImageCollectionStreamer) manager.GetObject("imageCollection2.ImageStream");
            this.panelControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.panelControl1.BorderStyle = BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnExcelOut);
            this.panelControl1.Controls.Add(this.btnSpeicalTime);
            this.panelControl1.Controls.Add(this.btnSelAll);
            this.panelControl1.Controls.Add(this.btnDown);
            this.panelControl1.Controls.Add(this.btnRead);
            this.panelControl1.Controls.Add(this.btnExit);
            this.panelControl1.Controls.Add(this.btnDel);
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Dock = DockStyle.Top;
            this.panelControl1.Location = new Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new Size(890, 50);
            this.panelControl1.TabIndex = 7;
            this.btnExcelOut.ImageIndex = 0;
            this.btnExcelOut.ImageList = this.imageCollection2;
            this.btnExcelOut.Location = new Point(0x298, 12);
            this.btnExcelOut.Name = "btnExcelOut";
            this.btnExcelOut.Size = new Size(0x6d, 0x1f);
            this.btnExcelOut.TabIndex = 0x16;
            this.btnExcelOut.Text = "导出EXCEL";
            this.btnExcelOut.Visible = false;
            this.btnExcelOut.Click += new EventHandler(this.btnExcelOut_Click);
            this.btnSpeicalTime.ImageIndex = 5;
            this.btnSpeicalTime.ImageList = this.imageCollection2;
            this.btnSpeicalTime.Location = new Point(0x31d, 12);
            this.btnSpeicalTime.Name = "btnSpeicalTime";
            this.btnSpeicalTime.Size = new Size(0x51, 0x1f);
            this.btnSpeicalTime.TabIndex = 10;
            this.btnSpeicalTime.Text = "特殊时段";
            this.btnSpeicalTime.Visible = false;
            this.btnSpeicalTime.Click += new EventHandler(this.btnSpeicalTime_Click);
            this.btnSelAll.ImageIndex = 3;
            this.btnSelAll.ImageList = this.imageCollection2;
            this.btnSelAll.Location = new Point(0x160, 12);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new Size(0x4b, 0x1f);
            this.btnSelAll.TabIndex = 9;
            this.btnSelAll.Text = "全  选";
            this.btnSelAll.Click += new EventHandler(this.btnSelAll_Click);
            this.btnDown.ImageIndex = 4;
            this.btnDown.ImageList = this.imageCollection2;
            this.btnDown.Location = new Point(250, 12);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new Size(0x4b, 0x1f);
            this.btnDown.TabIndex = 8;
            this.btnDown.Text = "下载时段";
            this.btnDown.Click += new EventHandler(this.btnDown_Click);
            this.btnRead.ImageIndex = 5;
            this.btnRead.ImageList = this.imageCollection2;
            this.btnRead.Location = new Point(0x1c6, 12);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new Size(0x4b, 0x1f);
            this.btnRead.TabIndex = 7;
            this.btnRead.Text = "读取时段";
            this.btnRead.Click += new EventHandler(this.btnRead_Click);
            this.btnExit.ImageIndex = 2;
            this.btnExit.ImageList = this.imageCollection2;
            this.btnExit.Location = new Point(0x22c, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(0x4b, 0x1f);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "退  出";
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnDel.ImageIndex = 1;
            this.btnDel.ImageList = this.imageCollection2;
            this.btnDel.Location = new Point(0x94, 12);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new Size(0x4b, 0x1f);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "删   除";
            this.btnDel.Click += new EventHandler(this.btnDel_Click);
            this.btnAdd.ImageIndex = 1;
            this.btnAdd.ImageList = this.imageCollection2;
            this.btnAdd.Location = new Point(13, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x6c, 0x1f);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添  加/修  改";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.GridControl1.Dock = DockStyle.Fill;
            this.GridControl1.EmbeddedNavigator.Appearance.Font = new Font("Verdana", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.GridControl1.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.GridControl1.EmbeddedNavigator.Name = "";
            this.GridControl1.Location = new Point(2, 2);
            this.GridControl1.MainView = this.dgvUInfo;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.RepositoryItems.AddRange(new RepositoryItem[] { this.txtStart1, this.txtEnd1, this.txtStart2, this.txtEnd2, this.txtStart3, this.txtEnd3, this.txtStart4, this.txtEnd4, this.repositoryItemTimeEdit1 });
            this.GridControl1.Size = new Size(0x376, 0x159);
            this.GridControl1.TabIndex = 8;
            this.GridControl1.ViewCollection.AddRange(new BaseView[] { this.dgvUInfo });
            this.dgvUInfo.CardCaptionFormat = "记录N {0}";
            this.dgvUInfo.CardWidth = 0xc3;
            this.dgvUInfo.Columns.AddRange(new GridColumn[] { this.colID, this.colStart1, this.colEnd1, this.colStart2, this.colEnd2, this.colStart3, this.colEnd3, this.colStart4, this.colEnd4, this.colMon, this.colTues, this.colWed, this.colThurs, this.colFri, this.colSat, this.colSun });
            this.dgvUInfo.FocusedCardTopFieldIndex = 0;
            this.dgvUInfo.GridControl = this.GridControl1;
            this.dgvUInfo.Name = "dgvUInfo";
            this.dgvUInfo.OptionsBehavior.Editable = false;
            this.dgvUInfo.OptionsSelection.MultiSelect = true;
            this.dgvUInfo.FocusedRowChanged += new FocusedRowChangedEventHandler(this.dgvUInfo_FocusedRowChanged);
            this.dgvUInfo.CellValueChanged += new CellValueChangedEventHandler(this.dgvUInfo_CellValueChanged);
            this.colID.AppearanceCell.ForeColor = Color.Fuchsia;
            this.colID.AppearanceCell.Options.UseForeColor = true;
            this.colID.AppearanceCell.Options.UseTextOptions = true;
            this.colID.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            this.colID.Caption = "时段";
            this.colID.Name = "colID";
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            this.colStart1.Caption = "开始1";
            this.colStart1.ColumnEdit = this.repositoryItemTimeEdit1;
            this.colStart1.Name = "colStart1";
            this.colStart1.Visible = true;
            this.colStart1.VisibleIndex = 1;
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.repositoryItemTimeEdit1.DisplayFormat.FormatString = "t";
            this.repositoryItemTimeEdit1.DisplayFormat.FormatType = FormatType.DateTime;
            this.repositoryItemTimeEdit1.EditFormat.FormatString = "t";
            this.repositoryItemTimeEdit1.EditFormat.FormatType = FormatType.DateTime;
            this.repositoryItemTimeEdit1.Mask.EditMask = "HH:mm";
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            this.colEnd1.Caption = "结束1";
            this.colEnd1.ColumnEdit = this.txtEnd1;
            this.colEnd1.Name = "colEnd1";
            this.colEnd1.Visible = true;
            this.colEnd1.VisibleIndex = 2;
            this.txtEnd1.AutoHeight = false;
            this.txtEnd1.MaxLength = 5;
            this.txtEnd1.Name = "txtEnd1";
            this.colStart2.Caption = "开始2";
            this.colStart2.ColumnEdit = this.txtStart2;
            this.colStart2.Name = "colStart2";
            this.colStart2.Visible = true;
            this.colStart2.VisibleIndex = 3;
            this.txtStart2.AutoHeight = false;
            this.txtStart2.MaxLength = 5;
            this.txtStart2.Name = "txtStart2";
            this.colEnd2.Caption = "结束2";
            this.colEnd2.ColumnEdit = this.txtEnd2;
            this.colEnd2.Name = "colEnd2";
            this.colEnd2.Visible = true;
            this.colEnd2.VisibleIndex = 4;
            this.txtEnd2.AutoHeight = false;
            this.txtEnd2.MaxLength = 5;
            this.txtEnd2.Name = "txtEnd2";
            this.colStart3.Caption = "开始3";
            this.colStart3.ColumnEdit = this.txtStart3;
            this.colStart3.Name = "colStart3";
            this.colStart3.Visible = true;
            this.colStart3.VisibleIndex = 5;
            this.txtStart3.AutoHeight = false;
            this.txtStart3.MaxLength = 5;
            this.txtStart3.Name = "txtStart3";
            this.colEnd3.Caption = "结束3";
            this.colEnd3.ColumnEdit = this.txtEnd3;
            this.colEnd3.Name = "colEnd3";
            this.colEnd3.Visible = true;
            this.colEnd3.VisibleIndex = 6;
            this.txtEnd3.AutoHeight = false;
            this.txtEnd3.MaxLength = 5;
            this.txtEnd3.Name = "txtEnd3";
            this.colStart4.Caption = "开始4";
            this.colStart4.ColumnEdit = this.txtStart4;
            this.colStart4.Name = "colStart4";
            this.colStart4.Visible = true;
            this.colStart4.VisibleIndex = 7;
            this.txtStart4.AutoHeight = false;
            this.txtStart4.MaxLength = 5;
            this.txtStart4.Name = "txtStart4";
            this.colEnd4.Caption = "结束4";
            this.colEnd4.ColumnEdit = this.txtEnd4;
            this.colEnd4.Name = "colEnd4";
            this.colEnd4.Visible = true;
            this.colEnd4.VisibleIndex = 8;
            this.txtEnd4.AutoHeight = false;
            this.txtEnd4.MaxLength = 5;
            this.txtEnd4.Name = "txtEnd4";
            this.colMon.Caption = "星期一";
            this.colMon.ColumnEdit = this.chkMon;
            this.colMon.Name = "colMon";
            this.colMon.Visible = true;
            this.colMon.VisibleIndex = 9;
            this.colTues.Caption = "星期二";
            this.colTues.ColumnEdit = this.chkTues;
            this.colTues.Name = "colTues";
            this.colTues.Visible = true;
            this.colTues.VisibleIndex = 10;
            this.colWed.Caption = "星期三";
            this.colWed.ColumnEdit = this.chkWed;
            this.colWed.Name = "colWed";
            this.colWed.Visible = true;
            this.colWed.VisibleIndex = 11;
            this.colThurs.Caption = "星期四";
            this.colThurs.ColumnEdit = this.chkThurs;
            this.colThurs.Name = "colThurs";
            this.colThurs.Visible = true;
            this.colThurs.VisibleIndex = 12;
            this.colFri.Caption = "星期五";
            this.colFri.ColumnEdit = this.chkFri;
            this.colFri.Name = "colFri";
            this.colFri.Visible = true;
            this.colFri.VisibleIndex = 13;
            this.colSat.Caption = "星期六";
            this.colSat.ColumnEdit = this.chkSat;
            this.colSat.Name = "colSat";
            this.colSat.Visible = true;
            this.colSat.VisibleIndex = 14;
            this.colSun.Caption = "星期日";
            this.colSun.ColumnEdit = this.chkSun;
            this.colSun.Name = "colSun";
            this.colSun.Visible = true;
            this.colSun.VisibleIndex = 15;
            this.txtStart1.AutoHeight = false;
            this.txtStart1.MaxLength = 5;
            this.txtStart1.Name = "txtStart1";
            this.panelControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.panelControl2.Controls.Add(this.imgList);
            this.panelControl2.Dock = DockStyle.Top;
            this.panelControl2.Location = new Point(0, 50);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new Size(890, 0x51);
            this.panelControl2.TabIndex = 9;
            this.imgList.Dock = DockStyle.Fill;
            this.imgList.ImageList = this.imageCollection1;
            this.imgList.Location = new Point(2, 2);
            this.imgList.MultiColumn = true;
            this.imgList.Name = "imgList";
            this.imgList.SelectionMode = SelectionMode.MultiExtended;
            this.imgList.Size = new Size(0x376, 0x4d);
            this.imgList.TabIndex = 6;
            this.imageCollection1.ImageSize = new Size(0x20, 0x20);
            this.imageCollection1.ImageStream = (ImageCollectionStreamer) manager.GetObject("imageCollection1.ImageStream");
            this.panelControl3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.panelControl3.Controls.Add(this.GridControl1);
            this.panelControl3.Dock = DockStyle.Top;
            this.panelControl3.Location = new Point(0, 0x83);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new Size(890, 0x15d);
            this.panelControl3.TabIndex = 10;
            this.timeReceive.Tick += new EventHandler(this.timeReceive_Tick);
            this.panelControl4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.panelControl4.Controls.Add(this.GridControl2);
            this.panelControl4.Dock = DockStyle.Fill;
            this.panelControl4.Location = new Point(0, 480);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new Size(890, 140);
            this.panelControl4.TabIndex = 11;
            this.GridControl2.Dock = DockStyle.Fill;
            this.GridControl2.EmbeddedNavigator.Name = "";
            this.GridControl2.Location = new Point(2, 2);
            this.GridControl2.MainView = this.dgvExecute;
            this.GridControl2.Name = "GridControl2";
            this.GridControl2.Size = new Size(0x376, 0x88);
            this.GridControl2.TabIndex = 1;
            this.GridControl2.ViewCollection.AddRange(new BaseView[] { this.dgvExecute });
            this.dgvExecute.Columns.AddRange(new GridColumn[] { this.colTime, this.colCommand, this.colTarget, this.colResult });
            this.dgvExecute.GridControl = this.GridControl2;
            this.dgvExecute.Name = "dgvExecute";
            this.colTime.Caption = "执行时间";
            this.colTime.Name = "colTime";
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 0;
            this.colCommand.Caption = "执行命令";
            this.colCommand.Name = "colCommand";
            this.colCommand.Visible = true;
            this.colCommand.VisibleIndex = 1;
            this.colTarget.Caption = " 执行对象";
            this.colTarget.Name = "colTarget";
            this.colTarget.Visible = true;
            this.colTarget.VisibleIndex = 2;
            this.colResult.Caption = "执行结果";
            this.colResult.Name = "colResult";
            this.colResult.Visible = true;
            this.colResult.VisibleIndex = 3;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(890, 620);
            base.Controls.Add(this.panelControl4);
            base.Controls.Add(this.panelControl3);
            base.Controls.Add(this.panelControl2);
            base.Controls.Add(this.panelControl1);
            base.Name = "frmEquTimeD";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "设置时间段";
            base.Load += new EventHandler(this.frmEquTimeD_Load);
            this.teStart1.EndInit();
            this.teEnd1.EndInit();
            this.teStart2.EndInit();
            this.teEnd2.EndInit();
            this.teStart3.EndInit();
            this.teEnd3.EndInit();
            this.teStart4.EndInit();
            this.teEnd4.EndInit();
            this.chkMon.EndInit();
            this.chkTues.EndInit();
            this.chkWed.EndInit();
            this.chkThurs.EndInit();
            this.chkFri.EndInit();
            this.chkSat.EndInit();
            this.chkSun.EndInit();
            this.imageCollection2.EndInit();
            this.panelControl1.EndInit();
            this.panelControl1.ResumeLayout(false);
            this.GridControl1.EndInit();
            this.dgvUInfo.EndInit();
            this.repositoryItemTimeEdit1.EndInit();
            this.txtEnd1.EndInit();
            this.txtStart2.EndInit();
            this.txtEnd2.EndInit();
            this.txtStart3.EndInit();
            this.txtEnd3.EndInit();
            this.txtStart4.EndInit();
            this.txtEnd4.EndInit();
            this.txtStart1.EndInit();
            this.panelControl2.EndInit();
            this.panelControl2.ResumeLayout(false);
            ((ISupportInitialize) this.imgList).EndInit();
            this.imageCollection1.EndInit();
            this.panelControl3.EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl4.EndInit();
            this.panelControl4.ResumeLayout(false);
            this.GridControl2.EndInit();
            this.dgvExecute.EndInit();
            base.ResumeLayout(false);
        }

        private void SaveTimeD(int nTimeName, byte[] bybuff)
        {
            int num2;
            string str = "时间段" + Convert.ToString((int) (nTimeName + 1));
            string[] strArray = new string[7];
            string[] strArray2 = new string[8];
            int index = 0;
            for (num2 = 0; num2 < 0x10; num2 += 2)
            {
                strArray2[index] = Convert.ToString(bybuff[num2 + 5], 0x10).PadLeft(2, '0') + ":" + Convert.ToString(bybuff[num2 + 6], 0x10).PadLeft(2, '0');
                index++;
            }
            for (num2 = 0; num2 < 7; num2++)
            {
                if ((bybuff[0x15] & (((int) 1) << num2)) != 0)
                {
                    strArray[num2] = "Y";
                }
                else
                {
                    strArray[num2] = "N";
                }
            }
            if (!this.bolDelTimeD)
            {
                this.bolDelTimeD = true;
                this.boperate.getcom("delete * from tb_TimeD");
            }
            this.opAndvalidate.autoNum("select max(TimeID) from tb_TimeD", "tb_TimeD", "TimeID", "SJ", "1000001", this.txtUserID);
            this.boperate.getcom("insert into tb_TimeD(TimeID,TimeStart1,TimeEnd1,TimeStart2,TimeEnd2,TimeStart3,TimeEnd3,TimeStart4,TimeEnd4,TimeMon,TimeTues,TimeWed,TimeThurs,TimeFri,TimeSat,TimeSun,TimeName) values('" + this.txtUserID.Text.Trim() + "','" + strArray2[0] + "','" + strArray2[1] + "','" + strArray2[2] + "','" + strArray2[3] + "','" + strArray2[4] + "','" + strArray2[5] + "','" + strArray2[6] + "','" + strArray2[7] + "','" + strArray[0] + "','" + strArray[1] + "','" + strArray[2] + "','" + strArray[3] + "','" + strArray[4] + "','" + strArray[5] + "','" + strArray[6] + "','" + str + "')");
        }

        private void SendData(string cmd)
        {
            int num5;
            int num = 0;
            byte num2 = 0;
            byte[] buf = new byte[30];
            byte[] bybuff = new byte[30];
            byte[] buffer3 = new byte[7];
            string[] strArray = new string[this.nToltal];
            string[] strArray2 = new string[this.nToltal];
            string[] strArray3 = new string[0x17];
            string[] strArray4 = new string[this.imgList.SelectedItems.Count];
            byte[] buffer4 = new byte[this.imgList.SelectedItems.Count];
            string str2 = "下载时段";
            string str3 = "检测不到设备";
            if (cmd == "DownTimeD")
            {
                str2 = "下载时段";
            }
            else if (cmd == "UpdataTimeD")
            {
                str2 = "上传时段";
            }
            int index = 0;
            while (index < this.imgList.SelectedItems.Count)
            {
                strArray[index] = this.imgList.SelectedItems[index].ToString();
                num5 = 0;
                while (num5 < this.nToltal)
                {
                    if (strArray[index] == this.strLocation[num5])
                    {
                        strArray2[index] = this.strAddress[num5];
                        strArray4[index] = this.strLocation[num5];
                        if (this.strAddCount[num5] == "1号门")
                        {
                            buffer4[index] = 1;
                        }
                        else if (this.strAddCount[num5] == "2号门")
                        {
                            buffer4[index] = 2;
                        }
                        else if (this.strAddCount[num5] == "3号门")
                        {
                            buffer4[index] = 3;
                        }
                        else if (this.strAddCount[num5] == "4号门")
                        {
                            buffer4[index] = 4;
                        }
                        break;
                    }
                    num5++;
                }
                index++;
            }
            if (!this.opComm.OpenComm())
            {
                MessageBox.Show("没发现此串口或串口已经在使用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.imgList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                for (index = 0; index < this.imgList.SelectedItems.Count; index++)
                {
                    int num3;
                    DataSet set;
                    int num8;
                    string str5;
                    buf[0] = 0xbd;
                    if (cmd == "DownTimeD")
                    {
                        buf[1] = 0x18;
                        buf[2] = 10;
                    }
                    else
                    {
                        if (!(cmd == "UpdataTimeD"))
                        {
                            break;
                        }
                        buf[1] = 7;
                        buf[2] = 11;
                    }
                    buf[3] = 0;
                    buf[4] = 0;
                    strArray2[index] = strArray2[index].PadLeft(4, '0');
                    buf[5] = this.opAndvalidate.DexToHex(strArray2[index].Substring(0, 2));
                    buf[6] = this.opAndvalidate.DexToHex(strArray2[index].Substring(2, 2));
                    buf[7] = buffer4[index];
                    if (cmd == "DownTimeD")
                    {
                        OleDbDataReader reader = this.boperate.getread(this.M_str_sql);
                        while (reader.Read())
                        {
                            int num6 = 0;
                            strArray3[num6] = reader["TimeStart1"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeEnd1"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeStart2"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeEnd2"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeStart3"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeEnd3"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeStart4"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeEnd4"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeMon"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeTues"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeWed"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeThurs"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeFri"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeSat"].ToString().Trim();
                            num6++;
                            strArray3[num6] = reader["TimeSun"].ToString().Trim();
                            string str4 = reader["TimeName"].ToString().Trim();
                            str4 = str4.Substring(str4.Length - 1, 1);
                            buf[8] = Convert.ToByte(str4);
                            int num7 = 0;
                            num8 = 0;
                            while (num8 < 8)
                            {
                                buf[9 + num7] = this.opAndvalidate.DexToHex(strArray3[num8].Substring(0, 2));
                                buf[10 + num7] = this.opAndvalidate.DexToHex(strArray3[num8].Substring(3, 2));
                                num7 += 2;
                                num8++;
                            }
                            buf[0x19] = 0;
                            num8 = 0;
                            while (num8 < 7)
                            {
                                if (strArray3[num8 + 8] == "Y")
                                {
                                    buf[0x19] = (byte) (buf[0x19] | ((byte) (((int) 1) << num8)));
                                }
                                num8++;
                            }
                            num2 = 0;
                            num5 = 0;
                            while (num5 <= 0x19)
                            {
                                num2 = (byte) (num2 + buf[num5]);
                                num5++;
                            }
                            buf[0x1a] = num2;
                            this.opComm.SendData(buf, 0x1b);
                            num3 = this.opComm.Receive(this.timeReceive, 500);
                            if (num3 >= 7)
                            {
                                bybuff = this.opComm.ReadData(num3);
                                num2 = 0;
                                num5 = 0;
                                while (num5 <= (num3 - 2))
                                {
                                    num2 = (byte) (num2 + bybuff[num5]);
                                    num5++;
                                }
                                if ((num2 == bybuff[num3 - 1]) && ((bybuff[0] == 0xbd) && (bybuff[1] == 4)))
                                {
                                    if (cmd == "Detection")
                                    {
                                        str5 = Convert.ToString(bybuff[5], 0x10);
                                        str5 = "V" + str5.Substring(0, 1) + "." + str5.Substring(1, 1);
                                        str3 = "设备运行正常，版本为" + str5;
                                    }
                                    else if (cmd == "DownTimeD")
                                    {
                                        str3 = "下载时间段成功";
                                    }
                                    else if (cmd == "UpdataTimeD")
                                    {
                                        str3 = "读取时间段成功";
                                    }
                                    else if (cmd == "ProofTime")
                                    {
                                        str3 = "校对时间成功";
                                    }
                                }
                                num++;
                            }
                            else
                            {
                                str3 = "检测不到设备";
                            }
                        }
                        if (num != this.dgvUInfo.RowCount)
                        {
                            str3 = "检测不到设备";
                        }
                    }
                    else
                    {
                        this.bolDelTimeD = false;
                        for (num8 = 0; num8 < 4; num8++)
                        {
                            if (cmd == "UpdataTimeD")
                            {
                                buf[8] = (byte) (num8 + 1);
                                num2 = 0;
                                num5 = 0;
                                while (num5 <= 8)
                                {
                                    num2 = (byte) (num2 + buf[num5]);
                                    num5++;
                                }
                                buf[9] = num2;
                                this.opComm.SendData(buf, 10);
                            }
                            num3 = this.opComm.Receive(this.timeReceive, 500);
                            if (num3 >= 7)
                            {
                                bybuff = this.opComm.ReadData(num3);
                                num2 = 0;
                                for (num5 = 0; num5 <= (num3 - 2); num5++)
                                {
                                    num2 = (byte) (num2 + bybuff[num5]);
                                }
                                if ((num2 == bybuff[num3 - 1]) && ((bybuff[0] == 0xbd) && (bybuff[1] == 20)))
                                {
                                    if (cmd == "Detection")
                                    {
                                        str5 = Convert.ToString(bybuff[5], 0x10);
                                        str5 = "V" + str5.Substring(0, 1) + "." + str5.Substring(1, 1);
                                        str3 = "设备运行正常，版本为" + str5;
                                    }
                                    else if (cmd == "DownTimeD")
                                    {
                                        str3 = "下载时间段成功";
                                    }
                                    else if (cmd == "UpdataTimeD")
                                    {
                                        this.SaveTimeD(num8, bybuff);
                                        str3 = "读取时间段成功";
                                    }
                                    else if (cmd == "ProofTime")
                                    {
                                        str3 = "校对时间成功";
                                    }
                                }
                            }
                            else
                            {
                                str3 = "检测不到设备";
                            }
                        }
                    }
                    if (cmd == "UpdataTimeD")
                    {
                        set = this.boperate.getds(this.M_str_sql, this.M_str_table);
                        this.GridControl1.DataSource = set.Tables[0];
                    }
                    string str = DateTime.Now.ToLongTimeString();
                    this.opAndvalidate.autoNum("select max(ExID) from tb_Execute", "tb_Execute", "ExID", "EX", "1000001", this.txtUserID);
                    this.boperate.getcom("insert into tb_Execute(ExID,ExTime,ExCmd,ExTarget,ExResult) values('" + this.txtUserID.Text.Trim() + "','" + str + "','" + str2 + "','" + strArray4[index] + "','" + str3 + "')");
                    string str6 = "select ExID,ExTime,ExCmd,ExTarget,ExResult from tb_Execute";
                    set = this.boperate.getds(str6, "tb_Execute");
                    this.GridControl2.DataSource = set.Tables[0];
                    this.dgvExecute.FocusedRowHandle = set.Tables[0].Rows.Count - 1;
                }
            }
        }

        private void timeReceive_Tick(object sender, EventArgs e)
        {
            this.timeReceive.Enabled = false;
        }
    }
}

