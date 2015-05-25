namespace 发卡模块
{
    using DAL;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class Form7 : Form
    {
        private int CardNumberID;
        private IContainer components;
        private DataGridView dataGridView1;

        public Form7()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public Form7(int ID)
        {
            this.components = null;
            this.CardNumberID = ID;
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void Form7_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from NickNameRecord where CardNumberID = @CardNumberID ";
                try
                {
                    OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@CardNumberID", this.CardNumberID) };
                    this.dataGridView1.DataSource = DBHelper.GetDataSet(sql, values);
                }
                catch
                {
                }
                this.dataGridView1.Columns["Nickname"].HeaderText = "设备名称";
                this.dataGridView1.Columns["CardNumberID"].HeaderText = "发卡记录ID";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form7));
            this.dataGridView1 = new DataGridView();
            ((ISupportInitialize) this.dataGridView1).BeginInit();
            base.SuspendLayout();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 0x17;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new Size(0x158, 0xb6);
            this.dataGridView1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x158, 0xb6);
            base.Controls.Add(this.dataGridView1);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form7";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "设备编码记录";
            base.Load += new EventHandler(this.Form7_Load);
            ((ISupportInitialize) this.dataGridView1).EndInit();
            base.ResumeLayout(false);
        }
    }
}

