namespace 发卡模块
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Card;
    using KAD_DX.BaseClass;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmAddEqu : Form
    {
        private bool bolAddError;
        private bool bolReLoad = true;
        private BaseOperate boperate = new BaseOperate();
        private SimpleButton btnAdd;
        private SimpleButton btnDel;
        private SimpleButton btnExit;
        private SimpleButton btnPrint;
        private SimpleButton btnRead;
        private SimpleButton btnTime;
        private RepositoryItemComboBox cboxDoorNO;
        private RepositoryItemComboBox cboxFloor;
        private GridColumn colAddress;
        private GridColumn colDoorNO;
        private GridColumn colFloor;
        private GridColumn colID;
        private GridColumn colLocation;
        private IContainer components = null;
        private CardView dgvUInfo;
        private GridControl GridControl1;
        private int intRowHandle;
        private bool M_bol_value = false;
        protected int M_int_judge;
        protected string M_str_sql = "select EquID,EquAddress,EquFloor,EquLocation,EquCount from tb_Equ";
        protected string M_str_table = "tb_Equ";
        private int nRowOldHandle;
        private OperateAndValidate opAndvalidate = new OperateAndValidate();
        private OperateComm opComm = new OperateComm();
        private PanelControl panelControl1;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private RepositoryItemComboBox repositoryItemComboBox3;
        private RepositoryItemComboBox repositoryItemComboBox4;
        private RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private RepositoryItemMemoEdit repositoryItemMemoEdit2;
        private RepositoryItemMemoEdit repositoryItemMemoEdit3;
        private RepositoryItemTextEdit repositoryItemTextEdit1;
        private RepositoryItemTextEdit repositoryItemTextEdit2;
        private RepositoryItemTextEdit repositoryItemTextEdit3;
        private string strOldCount;
        private RepositoryItemTextEdit txtAddress;
        private RepositoryItemMemoEdit txtLocation;
        private TextBox txtUserID = new TextBox();

        public frmAddEqu()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.opAndvalidate.autoNum("select max(EquID) from tb_Equ", "tb_Equ", "EquID", "EQ", "1000001", this.txtUserID);
            this.boperate.getcom("insert into tb_Equ(EquID) values('" + this.txtUserID.Text.Trim() + "')");
            DataSet set = this.boperate.getds(this.M_str_sql, this.M_str_table);
            this.GridControl1.DataSource = set.Tables[0];
            this.dgvUInfo.FocusedRowHandle = set.Tables[0].Rows.Count - 1;
            this.dgvUInfo.CardCaptionFormat = "设备{0}/" + set.Tables[0].Rows.Count;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if ((this.dgvUInfo.SelectedRowsCount > 0) && (MessageBox.Show("你确定要删除选中的记录吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, 0, false) == DialogResult.Yes))
            {
                int[] selectedRows = this.dgvUInfo.GetSelectedRows();
                if (selectedRows.Length > 0)
                {
                    for (int i = 0; i < selectedRows.Length; i++)
                    {
                        this.boperate.getcom("delete from tb_Equ where EquID='" + this.dgvUInfo.GetRowCellValue(selectedRows[i], "EquID").ToString() + "'");
                    }
                }
                this.dgvUInfo.DeleteSelectedRows();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void dgvUInfo_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            this.M_bol_value = true;
            this.nRowOldHandle = e.RowHandle;
            this.SaveRecord();
        }

        private void dgvUInfo_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle != 0)
            {
                this.intRowHandle = e.FocusedRowHandle;
            }
            if (this.bolReLoad)
            {
                this.nRowOldHandle = 0;
                this.bolReLoad = false;
            }
            else
            {
                object rowCellValue = this.dgvUInfo.GetRowCellValue(this.intRowHandle, "EquID");
                if (rowCellValue != null)
                {
                    this.txtUserID.Text = rowCellValue.ToString();
                    if (DBNull.Value != rowCellValue)
                    {
                    }
                    if (this.bolAddError)
                    {
                        this.bolAddError = false;
                        this.dgvUInfo.SetRowCellValue(this.nRowOldHandle, "EquAddress", "");
                        this.dgvUInfo.SetRowCellValue(this.nRowOldHandle, "EquCount", "");
                        this.dgvUInfo.SetRowCellValue(this.nRowOldHandle, "EquLocation", "");
                        this.M_bol_value = false;
                    }
                    if (!this.M_bol_value)
                    {
                        this.strOldCount = this.dgvUInfo.GetRowCellValue(this.intRowHandle, "EquCount").ToString();
                    }
                    else
                    {
                        this.M_bol_value = false;
                    }
                }
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

        private void frmAddEqu_FormClosing(object sender, FormClosingEventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void frmAddEqu_Load(object sender, EventArgs e)
        {
            DataSet set = this.boperate.getds(this.M_str_sql, this.M_str_table);
            this.GridControl1.DataSource = set.Tables[0];
            this.dgvUInfo.OptionsView.ShowQuickCustomizeButton = false;
            this.dgvUInfo.CardCaptionFormat = "设备{0}/" + set.Tables[0].Rows.Count;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmAddEqu));
            this.txtAddress = new RepositoryItemTextEdit();
            this.txtLocation = new RepositoryItemMemoEdit();
            this.cboxFloor = new RepositoryItemComboBox();
            this.cboxDoorNO = new RepositoryItemComboBox();
            this.panelControl1 = new PanelControl();
            this.btnTime = new SimpleButton();
            this.btnRead = new SimpleButton();
            this.btnExit = new SimpleButton();
            this.btnPrint = new SimpleButton();
            this.btnDel = new SimpleButton();
            this.btnAdd = new SimpleButton();
            this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
            this.repositoryItemMemoEdit1 = new RepositoryItemMemoEdit();
            this.repositoryItemComboBox1 = new RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new RepositoryItemComboBox();
            this.repositoryItemTextEdit2 = new RepositoryItemTextEdit();
            this.repositoryItemMemoEdit2 = new RepositoryItemMemoEdit();
            this.repositoryItemComboBox3 = new RepositoryItemComboBox();
            this.repositoryItemComboBox4 = new RepositoryItemComboBox();
            this.GridControl1 = new GridControl();
            this.dgvUInfo = new CardView();
            this.colID = new GridColumn();
            this.colAddress = new GridColumn();
            this.repositoryItemTextEdit3 = new RepositoryItemTextEdit();
            this.colDoorNO = new GridColumn();
            this.colFloor = new GridColumn();
            this.colLocation = new GridColumn();
            this.repositoryItemMemoEdit3 = new RepositoryItemMemoEdit();
            this.txtAddress.BeginInit();
            this.txtLocation.BeginInit();
            this.cboxFloor.BeginInit();
            this.cboxDoorNO.BeginInit();
            this.panelControl1.BeginInit();
            this.panelControl1.SuspendLayout();
            this.repositoryItemTextEdit1.BeginInit();
            this.repositoryItemMemoEdit1.BeginInit();
            this.repositoryItemComboBox1.BeginInit();
            this.repositoryItemComboBox2.BeginInit();
            this.repositoryItemTextEdit2.BeginInit();
            this.repositoryItemMemoEdit2.BeginInit();
            this.repositoryItemComboBox3.BeginInit();
            this.repositoryItemComboBox4.BeginInit();
            this.GridControl1.BeginInit();
            this.dgvUInfo.BeginInit();
            this.repositoryItemTextEdit3.BeginInit();
            this.repositoryItemMemoEdit3.BeginInit();
            base.SuspendLayout();
            this.txtAddress.AutoHeight = false;
            this.txtAddress.MaxLength = 3;
            this.txtAddress.Name = "txtAddress";
            this.txtLocation.LinesCount = 5;
            this.txtLocation.Name = "txtLocation";
            this.cboxFloor.AutoHeight = false;
            this.cboxFloor.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cboxFloor.MaxLength = 240;
            this.cboxFloor.Name = "cboxFloor";
            this.cboxFloor.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.cboxDoorNO.AutoHeight = false;
            this.cboxDoorNO.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cboxDoorNO.Items.AddRange(new object[] { "1号门", "2号门", "3号门", "4号门" });
            this.cboxDoorNO.Name = "cboxDoorNO";
            this.cboxDoorNO.Tag = "请选择门编号";
            this.cboxDoorNO.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.panelControl1.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.panelControl1.BorderStyle = BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnTime);
            this.panelControl1.Controls.Add(this.btnRead);
            this.panelControl1.Controls.Add(this.btnExit);
            this.panelControl1.Controls.Add(this.btnPrint);
            this.panelControl1.Controls.Add(this.btnDel);
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Dock = DockStyle.Top;
            this.panelControl1.Location = new Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new Size(0x2b1, 0x36);
            this.panelControl1.TabIndex = 7;
            this.btnTime.ImageIndex = 0x19;
            this.btnTime.Location = new Point(0x27d, 12);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new Size(0x4b, 0x1f);
            this.btnTime.TabIndex = 8;
            this.btnTime.Text = "同步时间";
            this.btnTime.Visible = false;
            this.btnRead.ImageIndex = 0x1b;
            this.btnRead.Location = new Point(0x2f0, 12);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new Size(0x4b, 0x1f);
            this.btnRead.TabIndex = 7;
            this.btnRead.Text = "读取信息";
            this.btnRead.Visible = false;
            this.btnExit.ImageIndex = 3;
            this.btnExit.Location = new Point(0x187, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(0x4b, 0x1f);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "退  出";
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnPrint.ImageIndex = 2;
            this.btnPrint.Location = new Point(0x10d, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x4b, 0x1f);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "打  印";
            this.btnDel.ImageIndex = 1;
            this.btnDel.Location = new Point(0x91, 12);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new Size(0x4b, 0x1f);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "删   除";
            this.btnDel.Click += new EventHandler(this.btnDel_Click);
            this.btnAdd.ImageIndex = 0;
            this.btnAdd.Location = new Point(0x15, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x4b, 0x1f);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添  加";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.MaxLength = 3;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.repositoryItemMemoEdit1.LinesCount = 5;
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox1.MaxLength = 240;
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox2.Items.AddRange(new object[] { "1号门", "2号门", "3号门", "4号门" });
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            this.repositoryItemComboBox2.Tag = "请选择门编号";
            this.repositoryItemComboBox2.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.MaxLength = 3;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            this.repositoryItemMemoEdit2.LinesCount = 5;
            this.repositoryItemMemoEdit2.Name = "repositoryItemMemoEdit2";
            this.repositoryItemComboBox3.AutoHeight = false;
            this.repositoryItemComboBox3.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox3.MaxLength = 240;
            this.repositoryItemComboBox3.Name = "repositoryItemComboBox3";
            this.repositoryItemComboBox3.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.repositoryItemComboBox4.AutoHeight = false;
            this.repositoryItemComboBox4.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox4.Items.AddRange(new object[] { "1号门", "2号门", "3号门", "4号门" });
            this.repositoryItemComboBox4.Name = "repositoryItemComboBox4";
            this.repositoryItemComboBox4.Tag = "请选择门编号";
            this.repositoryItemComboBox4.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.GridControl1.EmbeddedNavigator.Name = "";
            this.GridControl1.Location = new Point(0, 0x38);
            this.GridControl1.MainView = this.dgvUInfo;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemTextEdit3, this.repositoryItemMemoEdit3 });
            this.GridControl1.Size = new Size(0x2b0, 0x17a);
            this.GridControl1.TabIndex = 8;
            this.GridControl1.ViewCollection.AddRange(new BaseView[] { this.dgvUInfo });
            this.dgvUInfo.Columns.AddRange(new GridColumn[] { this.colID, this.colAddress, this.colDoorNO, this.colFloor, this.colLocation });
            this.dgvUInfo.FocusedCardTopFieldIndex = 0;
            this.dgvUInfo.GridControl = this.GridControl1;
            this.dgvUInfo.Name = "dgvUInfo";
            this.dgvUInfo.OptionsBehavior.FieldAutoHeight = true;
            this.dgvUInfo.OptionsSelection.MultiSelect = true;
            this.dgvUInfo.VertScrollVisibility = ScrollVisibility.Auto;
            this.dgvUInfo.FocusedRowChanged += new FocusedRowChangedEventHandler(this.dgvUInfo_FocusedRowChanged);
            this.dgvUInfo.CellValueChanged += new CellValueChangedEventHandler(this.dgvUInfo_CellValueChanged);
            this.colID.Caption = "编号";
            this.colID.FieldName = "EquID";
            this.colID.Name = "colID";
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            this.colAddress.Caption = "地址";
            this.colAddress.ColumnEdit = this.repositoryItemTextEdit3;
            this.colAddress.FieldName = "EquAddress";
            this.colAddress.Name = "colAddress";
            this.colAddress.Visible = true;
            this.colAddress.VisibleIndex = 1;
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            this.colDoorNO.Caption = "门编号";
            this.colDoorNO.FieldName = "EquCount";
            this.colDoorNO.Name = "colDoorNO";
            this.colFloor.Caption = "编号";
            this.colFloor.FieldName = "EquFloor";
            this.colFloor.Name = "colFloor";
            this.colLocation.Caption = "说明";
            this.colLocation.ColumnEdit = this.repositoryItemMemoEdit3;
            this.colLocation.FieldName = "EquLocation";
            this.colLocation.Name = "colLocation";
            this.colLocation.Visible = true;
            this.colLocation.VisibleIndex = 2;
            this.repositoryItemMemoEdit3.LinesCount = 5;
            this.repositoryItemMemoEdit3.Name = "repositoryItemMemoEdit3";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x2b1, 0x1b4);
            base.Controls.Add(this.GridControl1);
            base.Controls.Add(this.panelControl1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmAddEqu";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "添加设备";
            base.Load += new EventHandler(this.frmAddEqu_Load);
            base.FormClosing += new FormClosingEventHandler(this.frmAddEqu_FormClosing);
            this.txtAddress.EndInit();
            this.txtLocation.EndInit();
            this.cboxFloor.EndInit();
            this.cboxDoorNO.EndInit();
            this.panelControl1.EndInit();
            this.panelControl1.ResumeLayout(false);
            this.repositoryItemTextEdit1.EndInit();
            this.repositoryItemMemoEdit1.EndInit();
            this.repositoryItemComboBox1.EndInit();
            this.repositoryItemComboBox2.EndInit();
            this.repositoryItemTextEdit2.EndInit();
            this.repositoryItemMemoEdit2.EndInit();
            this.repositoryItemComboBox3.EndInit();
            this.repositoryItemComboBox4.EndInit();
            this.GridControl1.EndInit();
            this.dgvUInfo.EndInit();
            this.repositoryItemTextEdit3.EndInit();
            this.repositoryItemMemoEdit3.EndInit();
            base.ResumeLayout(false);
        }

        private void SaveRecord()
        {
            string str;
            string str2;
            string str3;
            string str4;
            try
            {
                str4 = this.dgvUInfo.GetRowCellValue(this.nRowOldHandle, "EquID").ToString().Trim();
                str = this.dgvUInfo.GetRowCellValue(this.nRowOldHandle, "EquAddress").ToString().Trim().PadLeft(3, '0');
                str3 = this.dgvUInfo.GetRowCellValue(this.nRowOldHandle, "EquCount").ToString().Trim();
                str2 = this.dgvUInfo.GetRowCellValue(this.nRowOldHandle, "EquLocation").ToString().Trim();
            }
            catch (Exception)
            {
                return;
            }
            if (str.Trim() == "000")
            {
                MessageBox.Show("请先输入设备地址！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.dgvUInfo.FocusedRowHandle = this.nRowOldHandle;
            }
            else
            {
                string str5 = "select EquID from tb_Equ where EquAddress='" + str + "'";
                OleDbDataReader reader = this.boperate.getread(str5);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader["EquID"].ToString().Trim() != str4)
                    {
                        MessageBox.Show("此设备已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dgvUInfo.FocusedRowHandle = this.nRowOldHandle;
                        return;
                    }
                }
                this.boperate.getcom("update tb_Equ set EquAddress='" + str + "',EquLocation='" + str2 + "',EquCount='" + str3 + "' where EquID='" + str4 + "'");
                DataSet set = this.boperate.getds(this.M_str_sql, this.M_str_table);
                this.GridControl1.DataSource = set.Tables[0];
                this.dgvUInfo.FocusedRowHandle = this.intRowHandle;
            }
        }
    }
}

