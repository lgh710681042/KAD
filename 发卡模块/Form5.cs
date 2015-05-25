namespace 发卡模块
{
    using DAL;
    using KADUpperComputer.BaseClass;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Globalization;
    using System.IO.Ports;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class Form5 : Form
    {
        private Button btnAddBlacklist;
        private Button btnCancel;
        private Button btnDel;
        private Button btnDelBlack;
        private Button btnExportExcel;
        private Button btnSelect;
        private Button btnUpdate;
        private Button btnUpdateSave;
        private Button button1;
        private Button button2;
        private Button button3;
        private string cardserial;
        private ComboBox cbxsex;
        private DataGridViewCheckBoxColumn Column1;
        private IContainer components;
        private DateTimePicker dateTimePicker1;
        private DataGridView dvwShowBlacklist;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Button MasterCardCalibrationTime;
        private OperateExcel opExcel;
        private OpenFileDialog opfExcel;
        private string portname;
        private SaveFileDialog saveFileDialog1;
        private Thread th;
        private System.Windows.Forms.Timer timer1;
        private TextBox txtAreaName;
        private TextBox txtBuildingNo;
        private TextBox txtCardNumber;
        private TextBox txtName;
        private TextBox txtRoomNumber;
        private TextBox txtSelect;

        public Form5()
        {
            this.components = null;
            this.opExcel = new OperateExcel();
            this.InitializeComponent();
        }

        public Form5(string s)
        {
            this.components = null;
            this.opExcel = new OperateExcel();
            this.portname = s;
            this.InitializeComponent();
        }

        private void Addblackfalse()
        {
            try
            {
                this.btnAddBlacklist.Enabled = false;
                this.btnAddBlacklist.Text = "正在加入黑名单......";
                this.SetOperationButtonState(false);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void Addblacktrue()
        {
            try
            {
                this.btnAddBlacklist.Enabled = true;
                this.btnAddBlacklist.Text = "母卡加入黑名单";
                this.SetOperationButtonState(true);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnAddBlacklist_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.portname.Equals(""))
                {
                    MessageBox.Show("没有找到设备,请检查设备是否连接");
                }
                else
                {
                    int num;
                    List<string> list = new List<string>();
                    for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                    {
                        this.dvwShowBlacklist.EndEdit();
                        DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell) this.dvwShowBlacklist.Rows[num].Cells[0];
                        if (Convert.ToBoolean(cell.Value))
                        {
                            list.Add(this.dvwShowBlacklist.Rows[num].Cells[1].Value.ToString().Trim());
                        }
                    }
                    if (list.Count > 0x4b)
                    {
                        throw new Exception("添加黑名单卡数量不能超过75张!");
                    }
                    string str = "禁止开门";
                    int num2 = 0;
                    ArrayList list2 = new ArrayList();
                    byte[] collection = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 1, 0 };
                    byte[] buffer2 = new byte[] { 
                        0xbd, 0x16, 0xb9, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 
                        0, 0, 0, 0, 0, 0, 0, 0, 0
                     };
                    ArrayList list3 = new ArrayList();
                    ArrayList list4 = new ArrayList();
                    List<byte> list5 = new List<byte>(0x1000);
                    this.OpenSerialPort();
                    Function.Encryption(Form1.comm, this.portname);
                    int num3 = this.readsanqu();
                    if (num3 == 11)
                    {
                        MessageBox.Show("读取扇区错误");
                        Form1.comm.Close();
                    }
                    else
                    {
                        collection[7] = (byte) (num3 + 1);
                        buffer2[7] = (byte) ((num3 + 1) * 4);
                        for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                        {
                            if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num].Cells[0].Value))
                            {
                                MatchCollection matchs = Regex.Matches(Convert.ToString(int.Parse(this.dvwShowBlacklist.Rows[num].Cells["CardNumber"].Value.ToString()), 0x10).PadLeft(6, '0'), @"\S\S", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                                foreach (Match match in matchs)
                                {
                                    list2.Add(Convert.ToByte(match.ToString(), 0x10));
                                }
                                num2++;
                            }
                        }
                        if (num2 == 0)
                        {
                            MessageBox.Show("请至少选择一条数据");
                        }
                        else
                        {
                            int num18;
                            int num19;
                            byte[] buffer9;
                            byte num20;
                            int num21;
                            int num4 = 0;
                            for (num = 0; num < list2.Count; num++)
                            {
                                byte[] buffer3;
                                byte[] buffer4;
                                byte num7;
                                int num8;
                                byte num9;
                                byte[] buffer5;
                                byte[] buffer6;
                                string str4;
                                ArrayList list6;
                                byte num10;
                                int num11;
                                int num5 = int.Parse(buffer2[7].ToString("X2"), NumberStyles.HexNumber);
                                int num6 = int.Parse(collection[7].ToString("X2"), NumberStyles.HexNumber);
                                buffer2[8 + num] = (byte) list2[num];
                                if (num == (list2.Count - 1))
                                {
                                    buffer3 = new byte[0x19];
                                    buffer4 = new byte[9];
                                    num7 = 0;
                                    num8 = 0;
                                    while (num8 < 0x18)
                                    {
                                        num7 = (byte) (num7 + buffer2[num8]);
                                        num8++;
                                    }
                                    buffer2[0x18] = num7;
                                    list5.AddRange(buffer2);
                                    list5.CopyTo(0, buffer3, 0, 0x19);
                                    list3.Add(buffer3);
                                    list5.RemoveRange(0, 0x19);
                                    num9 = 0;
                                    num8 = 0;
                                    while (num8 < 8)
                                    {
                                        num9 = (byte) (num9 + collection[num8]);
                                        num8++;
                                    }
                                    collection[8] = num9;
                                    list5.AddRange(collection);
                                    list5.CopyTo(0, buffer4, 0, 9);
                                    list4.Add(buffer4);
                                    list5.RemoveRange(0, 9);
                                    buffer5 = new byte[] { 
                                        0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 7, 
                                        0x80, 0x69, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0
                                     };
                                    buffer6 = new byte[0x19];
                                    str4 = Convert.ToString((int) ((num6 * 4) + 3), 0x10);
                                    list6 = Form1.IntoByte((num6 * 4) + 3, str4);
                                    buffer5[7] = (byte) list6[0];
                                    num10 = 0;
                                    num11 = 0;
                                    while (num11 < 0x18)
                                    {
                                        num10 = (byte) (num10 + buffer5[num11]);
                                        num11++;
                                    }
                                    buffer5[0x18] = num10;
                                    list5.AddRange(buffer5);
                                    list5.CopyTo(0, buffer6, 0, 0x19);
                                    list3.Add(buffer6);
                                    list5.RemoveRange(0, 0x19);
                                }
                                else if (((num % 14) == 0) && (num != 0))
                                {
                                    buffer3 = new byte[0x19];
                                    buffer4 = new byte[9];
                                    num7 = 0;
                                    num8 = 0;
                                    while (num8 < 0x18)
                                    {
                                        num7 = (byte) (num7 + buffer2[num8]);
                                        num8++;
                                    }
                                    buffer2[0x18] = num7;
                                    list5.AddRange(buffer2);
                                    list5.CopyTo(0, buffer3, 0, 0x19);
                                    list3.Add(buffer3);
                                    num8 = 0;
                                    while (num8 < 0x11)
                                    {
                                        buffer2[num8 + 8] = 0;
                                        num8++;
                                    }
                                    list5.RemoveRange(0, 0x19);
                                    num5++;
                                    if (num5 == ((num6 * 4) + 3))
                                    {
                                        buffer5 = new byte[] { 
                                            0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 7, 
                                            0x80, 0x69, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0
                                         };
                                        buffer6 = new byte[0x19];
                                        str4 = Convert.ToString(num5, 0x10);
                                        list6 = Form1.IntoByte(num5, str4);
                                        buffer5[7] = (byte) list6[0];
                                        num10 = 0;
                                        for (num11 = 0; num11 < 0x18; num11++)
                                        {
                                            num10 = (byte) (num10 + buffer5[num11]);
                                        }
                                        buffer5[0x18] = num10;
                                        list5.AddRange(buffer5);
                                        list5.CopyTo(0, buffer6, 0, 0x19);
                                        list3.Add(buffer6);
                                        list5.RemoveRange(0, 0x19);
                                        num5++;
                                    }
                                    string blockNumber = Convert.ToString(num5, 0x10);
                                    ArrayList list7 = Form1.IntoByte(num5, blockNumber);
                                    buffer2[7] = (byte) list7[0];
                                    num4++;
                                    if (((num4 % 3) == 0) && (num4 != 0))
                                    {
                                        num9 = 0;
                                        for (num8 = 0; num8 < 8; num8++)
                                        {
                                            num9 = (byte) (num9 + collection[num8]);
                                        }
                                        collection[8] = num9;
                                        list5.AddRange(collection);
                                        list5.CopyTo(0, buffer4, 0, 9);
                                        list4.Add(buffer4);
                                        collection[8] = 0;
                                        list5.RemoveRange(0, 9);
                                        num6++;
                                        string str6 = Convert.ToString(num6, 0x10);
                                        ArrayList list8 = Form1.IntoByte(num6, str6);
                                        collection[7] = (byte) list8[0];
                                    }
                                    num = -1;
                                    list2.RemoveRange(0, 15);
                                }
                            }
                            this.Addblackfalse();
                            bool isloaddefaultpassword = true;
                            bool isExecutionconinue = false;
                            int num12 = 0;
                            for (num = 0; num < 1; num++)
                            {
                                byte[] password = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 120 };
                                password[7] = (byte) num3;
                                byte num13 = 0;
                                for (int i = 0; i < 8; i++)
                                {
                                    num13 = (byte) (num13 + password[i]);
                                }
                                password[8] = num13;
                                int sector = num3;
                                this.LoadEquipmentPasswrod(sector);
                                this.stop();
                                if (!this.SearchCard())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.GetCardNumber())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.VerificationPassword(ref isloaddefaultpassword, ref isExecutionconinue, ref num, password, sector))
                                {
                                    Form1.comm.Close();
                                    MessageBox.Show("非法卡");
                                    this.Addblacktrue();
                                    return;
                                }
                                byte[] buffer = new byte[] { 
                                    0xbd, 0x16, 0xb9, 0, 0, 0, 0, 1, 0x80, 0, 0, 0, 0, 0, 0, 0, 
                                    0, 0, 0, 0, 0, 0, 0, 0, 0
                                 };
                                string str7 = Convert.ToString(num2, 0x10);
                                ArrayList list9 = Form1.IntoByte(num2, str7);
                                buffer[7] = (byte) ((num3 * 4) + 1);
                                buffer[0x12] = (byte) list9[0];
                                byte num16 = 0;
                                for (int j = 0; j < 0x18; j++)
                                {
                                    num16 = (byte) (num16 + buffer[j]);
                                }
                                buffer[0x18] = num16;
                                Form1.comm.DiscardInBuffer();
                                Form1.comm.Write(buffer, 0, 0x19);
                                num18 = 0;
                                while (num18 < 3)
                                {
                                    try
                                    {
                                        num19 = this.Receive();
                                        if (num19 == 7)
                                        {
                                            buffer9 = new byte[num19];
                                            Form1.comm.Read(buffer9, 0, num19);
                                            if ((buffer9[0] == 0xbd) && (buffer9[1] == 4))
                                            {
                                                num20 = 0;
                                                num21 = 0;
                                                while (num21 < 6)
                                                {
                                                    num20 = (byte) (num20 + buffer9[num21]);
                                                    num21++;
                                                }
                                                if (num20 != buffer9[6])
                                                {
                                                    MessageBox.Show("接收数据不完整");
                                                    Form1.comm.Close();
                                                    this.Addblacktrue();
                                                    break;
                                                }
                                                if (buffer9[5] == 0)
                                                {
                                                    break;
                                                }
                                                MessageBox.Show("写入扇区错误");
                                                Form1.comm.Close();
                                                this.Addblacktrue();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            Form1.comm.Close();
                                            this.Addblacktrue();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (num18 == 3)
                                    {
                                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                                        Form1.comm.Close();
                                        this.Addblacktrue();
                                    }
                                    num18++;
                                }
                                if (!this.SetEquipment(sector))
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                isloaddefaultpassword = true;
                            }
                            num12 = 0;
                            for (num = 0; num < list4.Count; num++)
                            {
                                byte[] buffer10 = (byte[]) list4[num];
                                int num22 = int.Parse(buffer10[7].ToString("X2"), NumberStyles.HexNumber);
                                if (isloaddefaultpassword)
                                {
                                    this.LoadDefaultpassored(num22);
                                }
                                this.stop();
                                if (!this.SearchCard())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.GetCardNumber())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                num12++;
                                if (num12 > 3)
                                {
                                    MessageBox.Show("非法卡");
                                    num12 = 0;
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.VerificationPassword1(ref isloaddefaultpassword, ref isExecutionconinue, ref num, buffer10, num22))
                                {
                                    Form1.comm.Close();
                                    MessageBox.Show("非法卡");
                                    this.Addblacktrue();
                                    return;
                                }
                                if (isExecutionconinue)
                                {
                                    isExecutionconinue = false;
                                }
                                else
                                {
                                    for (int k = 0; k < list3.Count; k++)
                                    {
                                        byte[] buffer11 = (byte[]) list3[k];
                                        int num24 = int.Parse(buffer11[7].ToString("X2"), NumberStyles.HexNumber) / 4;
                                        if (num24 == num22)
                                        {
                                            Form1.comm.DiscardInBuffer();
                                            Form1.comm.Write(buffer11, 0, 0x19);
                                            for (num18 = 0; num18 < 3; num18++)
                                            {
                                                try
                                                {
                                                    num19 = this.Receive();
                                                    if (num19 == 7)
                                                    {
                                                        buffer9 = new byte[num19];
                                                        Form1.comm.Read(buffer9, 0, num19);
                                                        if ((buffer9[0] == 0xbd) && (buffer9[1] == 4))
                                                        {
                                                            num20 = 0;
                                                            for (num21 = 0; num21 < 6; num21++)
                                                            {
                                                                num20 = (byte) (num20 + buffer9[num21]);
                                                            }
                                                            if (num20 != buffer9[6])
                                                            {
                                                                MessageBox.Show("接收数据不完整");
                                                                Form1.comm.Close();
                                                                this.Addblacktrue();
                                                                break;
                                                            }
                                                            if (buffer9[5] == 0)
                                                            {
                                                                break;
                                                            }
                                                            MessageBox.Show("写入扇区错误");
                                                            Form1.comm.Close();
                                                            this.Addblacktrue();
                                                            return;
                                                        }
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                }
                                                if (num18 == 3)
                                                {
                                                    MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                                                }
                                            }
                                        }
                                    }
                                    this.SerachSerialport();
                                    num12 = 0;
                                    isloaddefaultpassword = true;
                                }
                            }
                            MessageBox.Show("加入黑名单成功，请到设备上刷卡。");
                            for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                            {
                                if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num].Cells[0].Value))
                                {
                                    string str2 = this.dvwShowBlacklist.Rows[num].Cells["CardNumber"].Value.ToString();
                                    string sql = "update CardSerial set Blacklist = @Blacklist where CardNumber = @CardNumber";
                                    try
                                    {
                                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@Blacklist", str), new OleDbParameter("@CardNumber", str2) };
                                        DBHelper.ExecuteCommand(sql, values);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            string safeSql = "select CardNumber,UserName,Sex,AreaName,BuildingNo,RoomNumber,Count,StartPeriod,Period,Blacklist from CardSerial";
                            this.dvwShowBlacklist.DataSource = DBHelper.GetDataSet(safeSql);
                            this.DvwBlacklistheaderText();
                            Form1.comm.Close();
                            this.Addblacktrue();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                this.SetOperationButtonState(true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.CancelUpdate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                int num2;
                int num = 0;
                for (num2 = 0; num2 < this.dvwShowBlacklist.Rows.Count; num2++)
                {
                    if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num2].Cells[0].Value))
                    {
                        num++;
                    }
                }
                if (num == 0)
                {
                    MessageBox.Show("请至少选择一条数据");
                }
                else
                {
                    if (MessageBox.Show("确定要删除所选择的卡号", "删除", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                    {
                        for (num2 = 0; num2 < this.dvwShowBlacklist.Rows.Count; num2++)
                        {
                            if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num2].Cells[0].Value))
                            {
                                string sql = "DELETE FROM CardSerial WHERE CardNumber = @CardNumber";
                                try
                                {
                                    OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@CardNumber", this.dvwShowBlacklist.Rows[num2].Cells["CardNumber"].Value.ToString()) };
                                    DBHelper.ExecuteCommand(sql, values);
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine(exception.Message);
                                    MessageBox.Show("无法删除啦，请联系客服，谢谢！");
                                    return;
                                }
                            }
                        }
                    }
                    this.GetCardSerial();
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        private void btnDelBlack_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.portname.Equals(""))
                {
                    MessageBox.Show("没有找到设备,请检查设备是否连接");
                }
                else
                {
                    int num;
                    List<string> list = new List<string>();
                    for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                    {
                        this.dvwShowBlacklist.EndEdit();
                        DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell) this.dvwShowBlacklist.Rows[num].Cells[0];
                        if (Convert.ToBoolean(cell.Value))
                        {
                            list.Add(this.dvwShowBlacklist.Rows[num].Cells[1].Value.ToString().Trim());
                        }
                    }
                    if (list.Count > 0x4b)
                    {
                        throw new Exception("移除黑名单卡数量不能超过75张!");
                    }
                    string str = "允许开门";
                    int num2 = 0;
                    ArrayList list2 = new ArrayList();
                    byte[] collection = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 1, 0 };
                    byte[] buffer2 = new byte[] { 
                        0xbd, 0x16, 0xb9, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 
                        0, 0, 0, 0, 0, 0, 0, 0, 0
                     };
                    ArrayList list3 = new ArrayList();
                    ArrayList list4 = new ArrayList();
                    List<byte> list5 = new List<byte>(0x1000);
                    this.OpenSerialPort();
                    Function.Encryption(Form1.comm, this.portname);
                    int num3 = this.readsanqu();
                    if (num3 == 11)
                    {
                        MessageBox.Show("读取扇区错误");
                        Form1.comm.Close();
                    }
                    else
                    {
                        collection[7] = (byte) (num3 + 1);
                        buffer2[7] = (byte) ((num3 + 1) * 4);
                        for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                        {
                            if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num].Cells[0].Value))
                            {
                                MatchCollection matchs = Regex.Matches(Convert.ToString(int.Parse(this.dvwShowBlacklist.Rows[num].Cells["CardNumber"].Value.ToString()), 0x10).PadLeft(6, '0'), @"\S\S", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                                foreach (Match match in matchs)
                                {
                                    list2.Add(Convert.ToByte(match.ToString(), 0x10));
                                }
                                num2++;
                            }
                        }
                        if (num2 == 0)
                        {
                            MessageBox.Show("请至少选择一条数据");
                        }
                        else
                        {
                            int num18;
                            int num19;
                            byte[] buffer9;
                            byte num20;
                            int num21;
                            int num4 = 0;
                            for (num = 0; num < list2.Count; num++)
                            {
                                byte[] buffer3;
                                byte[] buffer4;
                                byte num7;
                                int num8;
                                byte num9;
                                byte[] buffer5;
                                byte[] buffer6;
                                string str4;
                                ArrayList list6;
                                byte num10;
                                int num11;
                                int num5 = int.Parse(buffer2[7].ToString("X2"), NumberStyles.HexNumber);
                                int num6 = int.Parse(collection[7].ToString("X2"), NumberStyles.HexNumber);
                                buffer2[8 + num] = (byte) list2[num];
                                if (num == (list2.Count - 1))
                                {
                                    buffer3 = new byte[0x19];
                                    buffer4 = new byte[9];
                                    num7 = 0;
                                    num8 = 0;
                                    while (num8 < 0x18)
                                    {
                                        num7 = (byte) (num7 + buffer2[num8]);
                                        num8++;
                                    }
                                    buffer2[0x18] = num7;
                                    list5.AddRange(buffer2);
                                    list5.CopyTo(0, buffer3, 0, 0x19);
                                    list3.Add(buffer3);
                                    list5.RemoveRange(0, 0x19);
                                    num9 = 0;
                                    num8 = 0;
                                    while (num8 < 8)
                                    {
                                        num9 = (byte) (num9 + collection[num8]);
                                        num8++;
                                    }
                                    collection[8] = num9;
                                    list5.AddRange(collection);
                                    list5.CopyTo(0, buffer4, 0, 9);
                                    list4.Add(buffer4);
                                    list5.RemoveRange(0, 9);
                                    buffer5 = new byte[] { 
                                        0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 7, 
                                        0x80, 0x69, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0
                                     };
                                    buffer6 = new byte[0x19];
                                    str4 = Convert.ToString((int) ((num6 * 4) + 3), 0x10);
                                    list6 = Form1.IntoByte((num6 * 4) + 3, str4);
                                    buffer5[7] = (byte) list6[0];
                                    num10 = 0;
                                    num11 = 0;
                                    while (num11 < 0x18)
                                    {
                                        num10 = (byte) (num10 + buffer5[num11]);
                                        num11++;
                                    }
                                    buffer5[0x18] = num10;
                                    list5.AddRange(buffer5);
                                    list5.CopyTo(0, buffer6, 0, 0x19);
                                    list3.Add(buffer6);
                                    list5.RemoveRange(0, 0x19);
                                }
                                else if (((num % 14) == 0) && (num != 0))
                                {
                                    buffer3 = new byte[0x19];
                                    buffer4 = new byte[9];
                                    num7 = 0;
                                    num8 = 0;
                                    while (num8 < 0x18)
                                    {
                                        num7 = (byte) (num7 + buffer2[num8]);
                                        num8++;
                                    }
                                    buffer2[0x18] = num7;
                                    list5.AddRange(buffer2);
                                    list5.CopyTo(0, buffer3, 0, 0x19);
                                    list3.Add(buffer3);
                                    num8 = 0;
                                    while (num8 < 0x11)
                                    {
                                        buffer2[num8 + 8] = 0;
                                        num8++;
                                    }
                                    list5.RemoveRange(0, 0x19);
                                    num5++;
                                    if (num5 == ((num6 * 4) + 3))
                                    {
                                        buffer5 = new byte[] { 
                                            0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 7, 
                                            0x80, 0x69, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0
                                         };
                                        buffer6 = new byte[0x19];
                                        str4 = Convert.ToString(num5, 0x10);
                                        list6 = Form1.IntoByte(num5, str4);
                                        buffer5[7] = (byte) list6[0];
                                        num10 = 0;
                                        for (num11 = 0; num11 < 0x18; num11++)
                                        {
                                            num10 = (byte) (num10 + buffer5[num11]);
                                        }
                                        buffer5[0x18] = num10;
                                        list5.AddRange(buffer5);
                                        list5.CopyTo(0, buffer6, 0, 0x19);
                                        list3.Add(buffer6);
                                        list5.RemoveRange(0, 0x19);
                                        num5++;
                                    }
                                    string blockNumber = Convert.ToString(num5, 0x10);
                                    ArrayList list7 = Form1.IntoByte(num5, blockNumber);
                                    buffer2[7] = (byte) list7[0];
                                    num4++;
                                    if (((num4 % 3) == 0) && (num4 != 0))
                                    {
                                        num9 = 0;
                                        for (num8 = 0; num8 < 8; num8++)
                                        {
                                            num9 = (byte) (num9 + collection[num8]);
                                        }
                                        collection[8] = num9;
                                        list5.AddRange(collection);
                                        list5.CopyTo(0, buffer4, 0, 9);
                                        list4.Add(buffer4);
                                        collection[8] = 0;
                                        list5.RemoveRange(0, 9);
                                        num6++;
                                        string str6 = Convert.ToString(num6, 0x10);
                                        ArrayList list8 = Form1.IntoByte(num6, str6);
                                        collection[7] = (byte) list8[0];
                                    }
                                    num = -1;
                                    list2.RemoveRange(0, 15);
                                }
                            }
                            this.Delblackfalse();
                            bool isloaddefaultpassword = true;
                            bool isExecutionconinue = false;
                            int num12 = 0;
                            for (num = 0; num < 1; num++)
                            {
                                byte[] password = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 120 };
                                password[7] = (byte) num3;
                                byte num13 = 0;
                                for (int i = 0; i < 8; i++)
                                {
                                    num13 = (byte) (num13 + password[i]);
                                }
                                password[8] = num13;
                                int sector = num3;
                                this.LoadEquipmentPasswrod(sector);
                                this.stop();
                                if (!this.SearchCard())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.GetCardNumber())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.VerificationPassword(ref isloaddefaultpassword, ref isExecutionconinue, ref num, password, sector))
                                {
                                    Form1.comm.Close();
                                    MessageBox.Show("非法卡");
                                    this.Addblacktrue();
                                    return;
                                }
                                byte[] buffer = new byte[] { 
                                    0xbd, 0x16, 0xb9, 0, 0, 0, 0, 1, 0x30, 0, 0, 0, 0, 0, 0, 0, 
                                    0, 0, 0, 0, 0, 0, 0, 0, 0
                                 };
                                string str7 = Convert.ToString(num2, 0x10);
                                ArrayList list9 = Form1.IntoByte(num2, str7);
                                buffer[7] = (byte) ((num3 * 4) + 1);
                                buffer[0x12] = (byte) list9[0];
                                byte num16 = 0;
                                for (int j = 0; j < 0x18; j++)
                                {
                                    num16 = (byte) (num16 + buffer[j]);
                                }
                                buffer[0x18] = num16;
                                Form1.comm.DiscardInBuffer();
                                Form1.comm.Write(buffer, 0, 0x19);
                                num18 = 0;
                                while (num18 < 3)
                                {
                                    try
                                    {
                                        num19 = this.Receive();
                                        if (num19 == 7)
                                        {
                                            buffer9 = new byte[num19];
                                            Form1.comm.Read(buffer9, 0, num19);
                                            if ((buffer9[0] == 0xbd) && (buffer9[1] == 4))
                                            {
                                                num20 = 0;
                                                num21 = 0;
                                                while (num21 < 6)
                                                {
                                                    num20 = (byte) (num20 + buffer9[num21]);
                                                    num21++;
                                                }
                                                if (num20 != buffer9[6])
                                                {
                                                    MessageBox.Show("接收数据不完整");
                                                    Form1.comm.Close();
                                                    this.Addblacktrue();
                                                    break;
                                                }
                                                if (buffer9[5] == 0)
                                                {
                                                    break;
                                                }
                                                MessageBox.Show("写入扇区错误");
                                                Form1.comm.Close();
                                                this.Addblacktrue();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            Form1.comm.Close();
                                            this.Addblacktrue();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (num18 == 3)
                                    {
                                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                                        Form1.comm.Close();
                                        this.Addblacktrue();
                                    }
                                    num18++;
                                }
                                if (!this.SetEquipment(sector))
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                isloaddefaultpassword = true;
                            }
                            num12 = 0;
                            for (num = 0; num < list4.Count; num++)
                            {
                                byte[] buffer10 = (byte[]) list4[num];
                                int num22 = int.Parse(buffer10[7].ToString("X2"), NumberStyles.HexNumber);
                                if (isloaddefaultpassword)
                                {
                                    this.LoadDefaultpassored(num22);
                                }
                                this.stop();
                                if (!this.SearchCard())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.GetCardNumber())
                                {
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                num12++;
                                if (num12 > 3)
                                {
                                    MessageBox.Show("非法卡");
                                    num12 = 0;
                                    Form1.comm.Close();
                                    this.Addblacktrue();
                                    return;
                                }
                                if (!this.VerificationPassword1(ref isloaddefaultpassword, ref isExecutionconinue, ref num, buffer10, num22))
                                {
                                    Form1.comm.Close();
                                    MessageBox.Show("非法卡");
                                    this.Addblacktrue();
                                    return;
                                }
                                if (isExecutionconinue)
                                {
                                    isExecutionconinue = false;
                                }
                                else
                                {
                                    for (int k = 0; k < list3.Count; k++)
                                    {
                                        byte[] buffer11 = (byte[]) list3[k];
                                        int num24 = int.Parse(buffer11[7].ToString("X2"), NumberStyles.HexNumber) / 4;
                                        if (num24 == num22)
                                        {
                                            Form1.comm.DiscardInBuffer();
                                            Form1.comm.Write(buffer11, 0, 0x19);
                                            for (num18 = 0; num18 < 3; num18++)
                                            {
                                                try
                                                {
                                                    num19 = this.Receive();
                                                    if (num19 == 7)
                                                    {
                                                        buffer9 = new byte[num19];
                                                        Form1.comm.Read(buffer9, 0, num19);
                                                        if ((buffer9[0] == 0xbd) && (buffer9[1] == 4))
                                                        {
                                                            num20 = 0;
                                                            for (num21 = 0; num21 < 6; num21++)
                                                            {
                                                                num20 = (byte) (num20 + buffer9[num21]);
                                                            }
                                                            if (num20 != buffer9[6])
                                                            {
                                                                MessageBox.Show("接收数据不完整");
                                                                Form1.comm.Close();
                                                                this.Addblacktrue();
                                                                break;
                                                            }
                                                            if (buffer9[5] == 0)
                                                            {
                                                                break;
                                                            }
                                                            MessageBox.Show("写入扇区错误");
                                                            Form1.comm.Close();
                                                            this.Addblacktrue();
                                                            return;
                                                        }
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                }
                                                if (num18 == 3)
                                                {
                                                    MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                                                }
                                            }
                                        }
                                    }
                                    this.SerachSerialport();
                                    num12 = 0;
                                    isloaddefaultpassword = true;
                                }
                            }
                            MessageBox.Show("移除黑名单成功，请到设备上刷卡。");
                            for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                            {
                                if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num].Cells[0].Value))
                                {
                                    string str2 = this.dvwShowBlacklist.Rows[num].Cells["CardNumber"].Value.ToString();
                                    string sql = "update CardSerial set Blacklist = @Blacklist where CardNumber = @CardNumber";
                                    try
                                    {
                                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@Blacklist", str), new OleDbParameter("@CardNumber", str2) };
                                        DBHelper.ExecuteCommand(sql, values);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            string safeSql = "select CardNumber,UserName,Sex,AreaName,BuildingNo,RoomNumber,Count,StartPeriod,Period,Blacklist from CardSerial";
                            this.dvwShowBlacklist.DataSource = DBHelper.GetDataSet(safeSql);
                            this.DvwBlacklistheaderText();
                            Form1.comm.Close();
                            this.Delblacktrue();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                this.SetOperationButtonState(true);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dvwShowBlacklist.Rows.Count == 0)
                {
                    MessageBox.Show("没有数据可以导出");
                }
                else
                {
                    DataGridView[] viewArray = new DataGridView[] { this.dvwShowBlacklist };
                    DataGridViewToExcel excel = new DataGridViewToExcel();
                    this.saveFileDialog1.ShowDialog();
                    if (!this.saveFileDialog1.FileName.ToString().Equals(""))
                    {
                        frmBusy busy = new frmBusy();
                        busy.Show();
                        OperateExcel excel2 = new OperateExcel();
                        excel2.ExportToExcel(this.saveFileDialog1.FileName, this.dvwShowBlacklist, "智能门禁脱机管理系统");
                        excel2.OpenFile(this.saveFileDialog1.FileName);
                        busy.Hide();
                        busy.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.txtSelect.Text.Trim().ToString();
                string safeSql = "select CardNumber,UserName,Sex,AreaName,BuildingNo,RoomNumber,Count,StartPeriod,Period,Blacklist from CardSerial where CardNumber like '%" + str + "%' or UserName like '%" + str + "%' or Sex like '%" + str + "%' or AreaName like '" + str + "%' or BuildingNo like '%" + str + "%' or RoomNumber like '%" + str + "%'or Count like '%" + str + "%' ";
                this.dvwShowBlacklist.DataSource = DBHelper.GetDataSet(safeSql);
                this.DvwBlacklistheaderText();
                this.dvwShowBlacklist.Focus();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dvwShowBlacklist.Rows.Count != 0)
                {
                    this.txtName.Text = this.dvwShowBlacklist.CurrentRow.Cells["UserName"].Value.ToString();
                    this.cbxsex.Text = this.dvwShowBlacklist.CurrentRow.Cells["Sex"].Value.ToString();
                    this.txtAreaName.Text = this.dvwShowBlacklist.CurrentRow.Cells["AreaName"].Value.ToString();
                    this.txtBuildingNo.Text = this.dvwShowBlacklist.CurrentRow.Cells["BuildingNo"].Value.ToString();
                    this.txtRoomNumber.Text = this.dvwShowBlacklist.CurrentRow.Cells["RoomNumber"].Value.ToString();
                    this.txtCardNumber.Text = this.dvwShowBlacklist.CurrentRow.Cells["CardNumber"].Value.ToString();
                    this.btnUpdateSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要修改?", "修改", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                {
                    string sql = "UPDATE CardSerial SET UserName = @UserName,Sex = @Sex,AreaName = @AreaName,BuildingNo = @BuildingNo,RoomNumber = @RoomNumber WHERE CardNumber = @CardNumber";
                    try
                    {
                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@UserName", this.txtName.Text), new OleDbParameter("@Sex", this.cbxsex.Text), new OleDbParameter("@AreaName", this.txtAreaName.Text), new OleDbParameter("@BuildingNo", this.txtBuildingNo.Text), new OleDbParameter("@RoomNumber", this.txtRoomNumber.Text), new OleDbParameter("@CardNumber", this.txtCardNumber.Text) };
                        DBHelper.ExecuteCommand(sql, values);
                    }
                    catch (Exception)
                    {
                    }
                    string str2 = "select CardNumber,UserName,Sex,AreaName,BuildingNo,RoomNumber,Count,StartPeriod,Period,Blacklist from CardSerial where CardNumber = @CardNumbe";
                    try
                    {
                        OleDbParameter[] parameterArray2 = new OleDbParameter[] { new OleDbParameter("@CardNumber", this.txtCardNumber.Text) };
                        this.dvwShowBlacklist.DataSource = DBHelper.GetDataSet(str2, parameterArray2);
                    }
                    catch (Exception)
                    {
                    }
                    this.CancelUpdate();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (this.portname.Equals(""))
                {
                    MessageBox.Show("没有找到设备,请检查设备是否连接");
                }
                else
                {
                    int num;
                    List<string> list = new List<string>();
                    for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                    {
                        this.dvwShowBlacklist.EndEdit();
                        DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell) this.dvwShowBlacklist.Rows[num].Cells[0];
                        if (Convert.ToBoolean(cell.Value))
                        {
                            list.Add(this.dvwShowBlacklist.Rows[num].Cells[1].Value.ToString().Trim());
                        }
                    }
                    if (list.Count > 30)
                    {
                        throw new Exception("延长有效期，选择卡数量不能超过30张!");
                    }
                    int num2 = 0;
                    ArrayList list2 = new ArrayList();
                    byte[] collection = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 1, 0 };
                    byte[] buffer2 = new byte[] { 
                        0xbd, 0x16, 0xb9, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 
                        0, 0, 0, 0, 0, 0, 0, 0, 0
                     };
                    ArrayList list3 = new ArrayList();
                    ArrayList list4 = new ArrayList();
                    List<byte> list5 = new List<byte>(0x1000);
                    this.OpenSerialPort();
                    Function.Encryption(Form1.comm, this.portname);
                    int num3 = this.readsanqu();
                    if (num3 == 11)
                    {
                        MessageBox.Show("读取扇区错误");
                        Form1.comm.Close();
                    }
                    else
                    {
                        collection[7] = (byte) (num3 + 1);
                        buffer2[7] = (byte) ((num3 + 1) * 4);
                        for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                        {
                            if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num].Cells[0].Value))
                            {
                                MatchCollection matchs = Regex.Matches(Convert.ToString(int.Parse(this.dvwShowBlacklist.Rows[num].Cells["CardNumber"].Value.ToString()), 0x10).PadLeft(6, '0'), @"\S\S", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                                foreach (Match match in matchs)
                                {
                                    list2.Add(Convert.ToByte(match.ToString(), 0x10));
                                }
                                string str3 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
                                list2.Add((byte) DadeTimeIntoByte(int.Parse(str3.Substring(2, 2)))[0]);
                                list2.Add((byte) DadeTimeIntoByte(int.Parse(str3.Substring(5, 2)))[0]);
                                list2.Add((byte) DadeTimeIntoByte(int.Parse(str3.Substring(8, 2)))[0]);
                                num2++;
                            }
                        }
                        if (num2 == 0)
                        {
                            MessageBox.Show("请至少选择一条数据");
                        }
                        else
                        {
                            int num18;
                            int num19;
                            byte[] buffer9;
                            byte num20;
                            int num21;
                            this.button1.Text = "数据处理中..";
                            this.button1.Enabled = false;
                            this.SetOperationButtonState(false);
                            int num4 = 0;
                            for (num = 0; num < list2.Count; num++)
                            {
                                byte[] buffer3;
                                byte[] buffer4;
                                byte num7;
                                int num8;
                                byte num9;
                                byte[] buffer5;
                                byte[] buffer6;
                                string str4;
                                ArrayList list6;
                                byte num10;
                                int num11;
                                int num5 = int.Parse(buffer2[7].ToString("X2"), NumberStyles.HexNumber);
                                int num6 = int.Parse(collection[7].ToString("X2"), NumberStyles.HexNumber);
                                buffer2[8 + num] = (byte) list2[num];
                                if (num == (list2.Count - 1))
                                {
                                    buffer3 = new byte[0x19];
                                    buffer4 = new byte[9];
                                    num7 = 0;
                                    num8 = 0;
                                    while (num8 < 0x18)
                                    {
                                        num7 = (byte) (num7 + buffer2[num8]);
                                        num8++;
                                    }
                                    buffer2[0x18] = num7;
                                    list5.AddRange(buffer2);
                                    list5.CopyTo(0, buffer3, 0, 0x19);
                                    list3.Add(buffer3);
                                    list5.RemoveRange(0, 0x19);
                                    num9 = 0;
                                    num8 = 0;
                                    while (num8 < 8)
                                    {
                                        num9 = (byte) (num9 + collection[num8]);
                                        num8++;
                                    }
                                    collection[8] = num9;
                                    list5.AddRange(collection);
                                    list5.CopyTo(0, buffer4, 0, 9);
                                    list4.Add(buffer4);
                                    list5.RemoveRange(0, 9);
                                    buffer5 = new byte[] { 
                                        0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 7, 
                                        0x80, 0x69, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0
                                     };
                                    buffer6 = new byte[0x19];
                                    str4 = Convert.ToString((int) ((num6 * 4) + 3), 0x10);
                                    list6 = Form1.IntoByte((num6 * 4) + 3, str4);
                                    buffer5[7] = (byte) list6[0];
                                    num10 = 0;
                                    num11 = 0;
                                    while (num11 < 0x18)
                                    {
                                        num10 = (byte) (num10 + buffer5[num11]);
                                        num11++;
                                    }
                                    buffer5[0x18] = num10;
                                    list5.AddRange(buffer5);
                                    list5.CopyTo(0, buffer6, 0, 0x19);
                                    list3.Add(buffer6);
                                    list5.RemoveRange(0, 0x19);
                                }
                                else if (((num % 11) == 0) && (num != 0))
                                {
                                    buffer3 = new byte[0x19];
                                    buffer4 = new byte[9];
                                    num7 = 0;
                                    num8 = 0;
                                    while (num8 < 0x18)
                                    {
                                        num7 = (byte) (num7 + buffer2[num8]);
                                        num8++;
                                    }
                                    buffer2[0x18] = num7;
                                    list5.AddRange(buffer2);
                                    list5.CopyTo(0, buffer3, 0, 0x19);
                                    list3.Add(buffer3);
                                    num8 = 0;
                                    while (num8 < 0x11)
                                    {
                                        buffer2[num8 + 8] = 0;
                                        num8++;
                                    }
                                    list5.RemoveRange(0, 0x19);
                                    num5++;
                                    if (num5 == ((num6 * 4) + 3))
                                    {
                                        buffer5 = new byte[] { 
                                            0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 7, 
                                            0x80, 0x69, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0
                                         };
                                        buffer6 = new byte[0x19];
                                        str4 = Convert.ToString(num5, 0x10);
                                        list6 = Form1.IntoByte(num5, str4);
                                        buffer5[7] = (byte) list6[0];
                                        num10 = 0;
                                        for (num11 = 0; num11 < 0x18; num11++)
                                        {
                                            num10 = (byte) (num10 + buffer5[num11]);
                                        }
                                        buffer5[0x18] = num10;
                                        list5.AddRange(buffer5);
                                        list5.CopyTo(0, buffer6, 0, 0x19);
                                        list3.Add(buffer6);
                                        list5.RemoveRange(0, 0x19);
                                        num5++;
                                    }
                                    string blockNumber = Convert.ToString(num5, 0x10);
                                    ArrayList list7 = Form1.IntoByte(num5, blockNumber);
                                    buffer2[7] = (byte) list7[0];
                                    num4++;
                                    if (((num4 % 3) == 0) && (num4 != 0))
                                    {
                                        num9 = 0;
                                        for (num8 = 0; num8 < 8; num8++)
                                        {
                                            num9 = (byte) (num9 + collection[num8]);
                                        }
                                        collection[8] = num9;
                                        list5.AddRange(collection);
                                        list5.CopyTo(0, buffer4, 0, 9);
                                        list4.Add(buffer4);
                                        collection[8] = 0;
                                        list5.RemoveRange(0, 9);
                                        num6++;
                                        string str6 = Convert.ToString(num6, 0x10);
                                        ArrayList list8 = Form1.IntoByte(num6, str6);
                                        collection[7] = (byte) list8[0];
                                    }
                                    num = -1;
                                    list2.RemoveRange(0, 12);
                                }
                            }
                            bool isloaddefaultpassword = true;
                            bool isExecutionconinue = false;
                            int num12 = 0;
                            for (num = 0; num < 1; num++)
                            {
                                byte[] password = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 120 };
                                password[7] = (byte) num3;
                                byte num13 = 0;
                                for (int i = 0; i < 8; i++)
                                {
                                    num13 = (byte) (num13 + password[i]);
                                }
                                password[8] = num13;
                                int sector = num3;
                                this.LoadEquipmentPasswrod(sector);
                                this.stop();
                                if (!this.SearchCard())
                                {
                                    Form1.comm.Close();
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                if (!this.GetCardNumber())
                                {
                                    Form1.comm.Close();
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                if (!this.VerificationPassword(ref isloaddefaultpassword, ref isExecutionconinue, ref num, password, sector))
                                {
                                    Form1.comm.Close();
                                    MessageBox.Show("非法卡");
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                byte[] buffer = new byte[] { 
                                    0xbd, 0x16, 0xb9, 0, 0, 0, 0, 1, 0x20, 0, 0, 0, 0, 0, 0, 0, 
                                    0, 0, 0, 0, 0, 0, 0, 0, 0
                                 };
                                string str7 = Convert.ToString(num2, 0x10);
                                ArrayList list9 = Form1.IntoByte(num2, str7);
                                buffer[7] = (byte) ((num3 * 4) + 1);
                                buffer[0x12] = (byte) list9[0];
                                byte num16 = 0;
                                for (int j = 0; j < 0x18; j++)
                                {
                                    num16 = (byte) (num16 + buffer[j]);
                                }
                                buffer[0x18] = num16;
                                this.OpenSerialPort();
                                Form1.comm.DiscardInBuffer();
                                Form1.comm.Write(buffer, 0, buffer.Length);
                                num18 = 0;
                                while (num18 < 3)
                                {
                                    try
                                    {
                                        num19 = this.Receive();
                                        if (num19 == 7)
                                        {
                                            buffer9 = new byte[num19];
                                            Form1.comm.Read(buffer9, 0, num19);
                                            if ((buffer9[0] == 0xbd) && (buffer9[1] == 4))
                                            {
                                                num20 = 0;
                                                num21 = 0;
                                                while (num21 < 6)
                                                {
                                                    num20 = (byte) (num20 + buffer9[num21]);
                                                    num21++;
                                                }
                                                if (num20 != buffer9[6])
                                                {
                                                    MessageBox.Show("接收数据不完整");
                                                    Form1.comm.Close();
                                                    this.Addblacktrue();
                                                    break;
                                                }
                                                if (buffer9[5] == 0)
                                                {
                                                    break;
                                                }
                                                MessageBox.Show("写入扇区错误");
                                                Form1.comm.Close();
                                                this.button1.Text = "母卡延长有效期";
                                                this.button1.Enabled = true;
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            Form1.comm.Close();
                                            this.button1.Text = "母卡延长有效期";
                                            this.button1.Enabled = true;
                                        }
                                    }
                                    catch (Exception exception1)
                                    {
                                        Exception exception = exception1;
                                        Console.WriteLine(exception.ToString());
                                    }
                                    if (num18 == 3)
                                    {
                                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                                        Form1.comm.Close();
                                        this.button1.Text = "母卡延长有效期";
                                        this.button1.Enabled = true;
                                    }
                                    num18++;
                                }
                                if (!this.SetEquipment(sector))
                                {
                                    Form1.comm.Close();
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                isloaddefaultpassword = true;
                            }
                            num12 = 0;
                            for (num = 0; num < list4.Count; num++)
                            {
                                byte[] buffer10 = (byte[]) list4[num];
                                int num22 = int.Parse(buffer10[7].ToString("X2"), NumberStyles.HexNumber);
                                if (isloaddefaultpassword)
                                {
                                    this.LoadDefaultpassored(num22);
                                }
                                this.stop();
                                if (!this.SearchCard())
                                {
                                    Form1.comm.Close();
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                if (!this.GetCardNumber())
                                {
                                    Form1.comm.Close();
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                num12++;
                                if (num12 > 3)
                                {
                                    MessageBox.Show("非法卡");
                                    num12 = 0;
                                    Form1.comm.Close();
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                if (!this.VerificationPassword1(ref isloaddefaultpassword, ref isExecutionconinue, ref num, buffer10, num22))
                                {
                                    Form1.comm.Close();
                                    MessageBox.Show("非法卡");
                                    this.button1.Text = "母卡延长有效期";
                                    this.button1.Enabled = true;
                                    return;
                                }
                                if (isExecutionconinue)
                                {
                                    isExecutionconinue = false;
                                }
                                else
                                {
                                    for (int k = 0; k < list3.Count; k++)
                                    {
                                        byte[] buffer11 = (byte[]) list3[k];
                                        int num24 = int.Parse(buffer11[7].ToString("X2"), NumberStyles.HexNumber) / 4;
                                        if (num24 == num22)
                                        {
                                            Form1.comm.DiscardInBuffer();
                                            Form1.comm.Write(buffer11, 0, 0x19);
                                            for (num18 = 0; num18 < 3; num18++)
                                            {
                                                try
                                                {
                                                    num19 = this.Receive();
                                                    if (num19 == 7)
                                                    {
                                                        buffer9 = new byte[num19];
                                                        Form1.comm.Read(buffer9, 0, num19);
                                                        if ((buffer9[0] == 0xbd) && (buffer9[1] == 4))
                                                        {
                                                            num20 = 0;
                                                            for (num21 = 0; num21 < 6; num21++)
                                                            {
                                                                num20 = (byte) (num20 + buffer9[num21]);
                                                            }
                                                            if (num20 != buffer9[6])
                                                            {
                                                                MessageBox.Show("接收数据不完整");
                                                                Form1.comm.Close();
                                                                this.button1.Text = "母卡延长有效期";
                                                                this.button1.Enabled = true;
                                                                break;
                                                            }
                                                            if (buffer9[5] == 0)
                                                            {
                                                                break;
                                                            }
                                                            MessageBox.Show("写入扇区错误");
                                                            Form1.comm.Close();
                                                            this.button1.Text = "母卡延长有效期";
                                                            this.button1.Enabled = true;
                                                            return;
                                                        }
                                                    }
                                                }
                                                catch (Exception exception3)
                                                {
                                                    Console.WriteLine(exception3.ToString());
                                                }
                                                if (num18 == 3)
                                                {
                                                    MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                                                }
                                            }
                                        }
                                    }
                                    this.SerachSerialport();
                                    num12 = 0;
                                    isloaddefaultpassword = true;
                                }
                            }
                            for (num = 0; num < this.dvwShowBlacklist.Rows.Count; num++)
                            {
                                if (Convert.ToBoolean(this.dvwShowBlacklist.Rows[num].Cells[0].Value))
                                {
                                    string str = this.dvwShowBlacklist.Rows[num].Cells["CardNumber"].Value.ToString();
                                    string sql = "update CardSerial set Period = @Period where CardNumber = @CardNumber";
                                    try
                                    {
                                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@Period", this.dateTimePicker1.Value.Date.ToString("yyyy-MM-dd")), new OleDbParameter("@CardNumber", str) };
                                        DBHelper.ExecuteCommand(sql, values);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            this.GetCardSerial();
                            MessageBox.Show("延长有效期成功，请到设备上刷卡。");
                            Form1.comm.Close();
                            this.button1.Text = "母卡延长有效期";
                            this.button1.Enabled = true;
                            this.SetOperationButtonState(true);
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
            finally
            {
                this.SetOperationButtonState(true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select CardNumber,UserName,Sex,AreaName,BuildingNo,RoomNumber,Count,StartPeriod,Period,Blacklist from CardSerial where Period = @Period";
                OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("Period", Convert.ToDateTime(this.dateTimePicker1.Value.ToString("yyyy-MM-dd"))) };
                this.dvwShowBlacklist.DataSource = DBHelper.GetDataSet(sql, values);
                this.DvwBlacklistheaderText();
                this.dvwShowBlacklist.Focus();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void CancelUpdate()
        {
            try
            {
                this.txtName.Text = "";
                this.cbxsex.Text = "";
                this.txtAreaName.Text = "";
                this.txtBuildingNo.Text = "";
                this.txtRoomNumber.Text = "";
                this.txtCardNumber.Text = "";
                this.btnUpdateSave.Enabled = false;
                this.btnCancel.Enabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void cbxOperatingType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private static ArrayList DadeTimeIntoByte(int _datetime)
        {
            try
            {
                MatchCollection matchs;
                ArrayList list = new ArrayList();
                if (_datetime > 9)
                {
                    matchs = Regex.Matches(_datetime.ToString(), @"\S\S", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                }
                else
                {
                    matchs = Regex.Matches("0" + _datetime, @"\S\S", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                }
                list.Add(Convert.ToByte(matchs[0].ToString(), 0x10));
                return list;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return new ArrayList();
            }
        }

        private void Delblackfalse()
        {
            try
            {
                this.btnAddBlacklist.Enabled = false;
                this.btnDelBlack.Text = "正在移除黑名单......";
                this.SetOperationButtonState(false);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void Delblacktrue()
        {
            try
            {
                this.btnAddBlacklist.Enabled = true;
                this.btnDelBlack.Text = "母卡移除黑名单";
                this.SetOperationButtonState(true);
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

        private void Form5_Load(object sender, EventArgs e)
        {
            this.GetCardSerial();
        }

        public bool GetCardNumber()
        {
            try
            {
                byte[] buffer = new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 };
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(buffer, 0, 8);
                Array.Clear(buffer, 0, 8);
                bool flag = false;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 11)
                        {
                            byte[] buffer2 = new byte[count];
                            Form1.comm.Read(buffer2, 0, count);
                            if ((buffer2[0] == 0xbd) && (buffer2[1] == 8))
                            {
                                byte num3 = 0;
                                int index = 0;
                                while (index < 10)
                                {
                                    num3 = (byte) (num3 + buffer2[index]);
                                    index++;
                                }
                                if (num3 != buffer2[10])
                                {
                                    flag = false;
                                }
                                else
                                {
                                    string s = "";
                                    for (index = 3; index > 0; index--)
                                    {
                                        s = s + buffer2[index + 5].ToString("X2");
                                    }
                                    this.cardserial = int.Parse(s, NumberStyles.HexNumber).ToString("00000000");
                                    flag = true;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Form1.comm.DiscardInBuffer();
                    }
                    if (i == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                        flag = false;
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        private void GetCardSerial()
        {
            try
            {
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
                for (int i = 1; i <= 10; i++)
                {
                    this.dvwShowBlacklist.Columns[i].ReadOnly = true;
                }
                this.dvwShowBlacklist.Focus();
                this.Text = string.Format((string) base.Tag, this.dvwShowBlacklist.Rows.Count);
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
            Exception exception;
            try
            {
                int num = 0xf4241;
                string[] strArray = new string[12];
                if (MessageBox.Show("导入后，旧的数据将被全部删除，确定要导入吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    frmBusy busy = new frmBusy();
                    this.opfExcel.Filter = "EXCEL (*.xls;*.csv)|*.xls;*.csv|All files (*.*)|*.*";
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
                                for (int j = 0; j < 10; j++)
                                {
                                    strArray[j] = set.Tables[0].Rows[i][j].ToString();
                                }
                                if (strArray[0] != "卡号")
                                {
                                    string sql = "INSERT INTO CardSerial(CardNumber,UserName,Sex,AreaName,BuildingNo,RoomNumber,[Count],StartPeriod,Period,Remark) VALUES(@CardNumber,@UserName,@Sex,@AreaName,@BuildingNo,@RoomNumber,@Count,@StartPeriod,@Period,@Remark)";
                                    try
                                    {
                                        OleDbParameter[] values = new OleDbParameter[] { new OleDbParameter("@CardNumber", strArray[0]), new OleDbParameter("@UserName", strArray[1]), new OleDbParameter("@Sex", strArray[2]), new OleDbParameter("@AreaName", strArray[3]), new OleDbParameter("@BuildingNo", strArray[4]), new OleDbParameter("@RoomNumber", strArray[5]), new OleDbParameter("@Count", strArray[6]), new OleDbParameter("@StartPeriod", Convert.ToDateTime(strArray[7])), new OleDbParameter("@Period", Convert.ToDateTime(strArray[8])), new OleDbParameter("@Remark", strArray[9]) };
                                        DBHelper.ExecuteCommand(sql, values);
                                    }
                                    catch (Exception exception1)
                                    {
                                        exception = exception1;
                                        MessageBox.Show(exception.Message.ToString(), "错误!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                        Console.WriteLine(exception.ToString());
                                    }
                                    num++;
                                }
                            }
                        }
                        busy.Hide();
                        busy.Dispose();
                        this.GetCardSerial();
                        MessageBox.Show("导入完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form5));
            this.dvwShowBlacklist = new DataGridView();
            this.Column1 = new DataGridViewCheckBoxColumn();
            this.btnAddBlacklist = new Button();
            this.btnDelBlack = new Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new Label();
            this.txtSelect = new TextBox();
            this.btnSelect = new Button();
            this.btnExportExcel = new Button();
            this.saveFileDialog1 = new SaveFileDialog();
            this.btnDel = new Button();
            this.btnUpdate = new Button();
            this.label1 = new Label();
            this.txtCardNumber = new TextBox();
            this.txtName = new TextBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.txtAreaName = new TextBox();
            this.label5 = new Label();
            this.txtBuildingNo = new TextBox();
            this.label6 = new Label();
            this.txtRoomNumber = new TextBox();
            this.label7 = new Label();
            this.btnUpdateSave = new Button();
            this.btnCancel = new Button();
            this.cbxsex = new ComboBox();
            this.button1 = new Button();
            this.dateTimePicker1 = new DateTimePicker();
            this.button2 = new Button();
            this.opfExcel = new OpenFileDialog();
            this.button3 = new Button();
            this.MasterCardCalibrationTime = new Button();
            ((ISupportInitialize) this.dvwShowBlacklist).BeginInit();
            base.SuspendLayout();
            this.dvwShowBlacklist.AllowUserToAddRows = false;
            this.dvwShowBlacklist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dvwShowBlacklist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvwShowBlacklist.Columns.AddRange(new DataGridViewColumn[] { this.Column1 });
            this.dvwShowBlacklist.Location = new Point(12, 0x27);
            this.dvwShowBlacklist.Name = "dvwShowBlacklist";
            this.dvwShowBlacklist.RowTemplate.Height = 0x17;
            this.dvwShowBlacklist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dvwShowBlacklist.Size = new Size(0x355, 0x169);
            this.dvwShowBlacklist.TabIndex = 0;
            this.Column1.FalseValue = "false";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.TrueValue = "true";
            this.btnAddBlacklist.Location = new Point(0x105, 10);
            this.btnAddBlacklist.Name = "btnAddBlacklist";
            this.btnAddBlacklist.Size = new Size(0x5f, 0x17);
            this.btnAddBlacklist.TabIndex = 3;
            this.btnAddBlacklist.Text = "母卡加黑名单";
            this.btnAddBlacklist.UseVisualStyleBackColor = true;
            this.btnAddBlacklist.Click += new EventHandler(this.btnAddBlacklist_Click);
            this.btnDelBlack.Location = new Point(0x16b, 10);
            this.btnDelBlack.Name = "btnDelBlack";
            this.btnDelBlack.Size = new Size(0x5f, 0x17);
            this.btnDelBlack.TabIndex = 2;
            this.btnDelBlack.Text = "母卡移除黑名单";
            this.btnDelBlack.UseVisualStyleBackColor = true;
            this.btnDelBlack.Click += new EventHandler(this.btnDelBlack_Click);
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(10, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4d, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "请输入查询值";
            this.txtSelect.Location = new Point(0x58, 12);
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new Size(0x52, 0x15);
            this.txtSelect.TabIndex = 7;
            this.btnSelect.Location = new Point(0xb2, 10);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new Size(0x4b, 0x17);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "查  询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
            this.btnExportExcel.Location = new Point(0x31c, 0x19b);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new Size(0x44, 0x17);
            this.btnExportExcel.TabIndex = 9;
            this.btnExportExcel.Text = "导出Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new EventHandler(this.btnExportExcel_Click);
            this.saveFileDialog1.Filter = "*.xlsx|*.xlsx";
            this.btnDel.Location = new Point(630, 0x1bd);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new Size(0x4b, 0x17);
            this.btnDel.TabIndex = 10;
            this.btnDel.Text = "删  除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new EventHandler(this.btnDel_Click);
            this.btnUpdate.Location = new Point(630, 0x19b);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(0x4b, 0x17);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "修  改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(7, 0x1a0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x29, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "卡 号:";
            this.txtCardNumber.Enabled = false;
            this.txtCardNumber.Location = new Point(0x36, 0x19d);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new Size(100, 0x15);
            this.txtCardNumber.TabIndex = 13;
            this.txtName.Location = new Point(0xcf, 0x19d);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(100, 0x15);
            this.txtName.TabIndex = 15;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(160, 0x1a0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x29, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "姓 名:";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x141, 0x1a0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x29, 12);
            this.label4.TabIndex = 0x10;
            this.label4.Text = "性 别:";
            this.txtAreaName.Location = new Point(0x36, 0x1be);
            this.txtAreaName.Name = "txtAreaName";
            this.txtAreaName.Size = new Size(100, 0x15);
            this.txtAreaName.TabIndex = 0x11;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(7, 0x1c1);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x29, 12);
            this.label5.TabIndex = 0x12;
            this.label5.Text = "小 区:";
            this.txtBuildingNo.Location = new Point(0xcf, 0x1be);
            this.txtBuildingNo.Name = "txtBuildingNo";
            this.txtBuildingNo.Size = new Size(100, 0x15);
            this.txtBuildingNo.TabIndex = 0x12;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(160, 0x1c1);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x29, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "楼 号:";
            this.txtRoomNumber.Location = new Point(0x170, 0x1be);
            this.txtRoomNumber.Name = "txtRoomNumber";
            this.txtRoomNumber.Size = new Size(100, 0x15);
            this.txtRoomNumber.TabIndex = 0x13;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x141, 0x1c1);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x29, 12);
            this.label7.TabIndex = 0x16;
            this.label7.Text = "房 号:";
            this.btnUpdateSave.Enabled = false;
            this.btnUpdateSave.Location = new Point(0x2c9, 0x19b);
            this.btnUpdateSave.Name = "btnUpdateSave";
            this.btnUpdateSave.Size = new Size(0x4b, 0x17);
            this.btnUpdateSave.TabIndex = 20;
            this.btnUpdateSave.Text = "保  存";
            this.btnUpdateSave.UseVisualStyleBackColor = true;
            this.btnUpdateSave.Click += new EventHandler(this.button1_Click_1);
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new Point(0x2c9, 0x1bd);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0x19;
            this.btnCancel.Text = "取  消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.cbxsex.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxsex.FormattingEnabled = true;
            this.cbxsex.Items.AddRange(new object[] { "男", "女" });
            this.cbxsex.Location = new Point(0x170, 0x19d);
            this.cbxsex.Name = "cbxsex";
            this.cbxsex.Size = new Size(100, 20);
            this.cbxsex.TabIndex = 0x10;
            this.button1.Location = new Point(670, 10);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x65, 0x17);
            this.button1.TabIndex = 0x1d;
            this.button1.Text = "母卡延长有效期";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click_2);
            this.dateTimePicker1.Location = new Point(0x1d0, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new Size(0x67, 0x15);
            this.dateTimePicker1.TabIndex = 0x1c;
            this.button2.Location = new Point(0x31c, 0x1bd);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x45, 0x17);
            this.button2.TabIndex = 30;
            this.button2.Text = "导入Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button3.Location = new Point(0x23d, 10);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x59, 0x17);
            this.button3.TabIndex = 0x1f;
            this.button3.Text = "按有效期查询";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.MasterCardCalibrationTime.Location = new Point(0x309, 10);
            this.MasterCardCalibrationTime.Name = "MasterCardCalibrationTime";
            this.MasterCardCalibrationTime.Size = new Size(0x57, 0x17);
            this.MasterCardCalibrationTime.TabIndex = 0x20;
            this.MasterCardCalibrationTime.Text = "母卡校准时间";
            this.MasterCardCalibrationTime.UseVisualStyleBackColor = true;
            this.MasterCardCalibrationTime.Click += new EventHandler(this.MasterCardCalibrationTime_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            base.ClientSize = new Size(0x36e, 0x1dd);
            base.Controls.Add(this.MasterCardCalibrationTime);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.dateTimePicker1);
            base.Controls.Add(this.cbxsex);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnUpdateSave);
            base.Controls.Add(this.txtRoomNumber);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.txtBuildingNo);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.txtAreaName);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.txtName);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.txtCardNumber);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnUpdate);
            base.Controls.Add(this.btnDel);
            base.Controls.Add(this.btnExportExcel);
            base.Controls.Add(this.btnSelect);
            base.Controls.Add(this.txtSelect);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnDelBlack);
            base.Controls.Add(this.btnAddBlacklist);
            base.Controls.Add(this.dvwShowBlacklist);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form5";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.Tag = "用户管理 - 用户数量{0}条";
            this.Text = "用户管理 - 用户数量{0}条";
            base.Load += new EventHandler(this.Form5_Load);
            ((ISupportInitialize) this.dvwShowBlacklist).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadDefaultpassored(int lnghex1)
        {
            try
            {
                byte[] buffer = new byte[] { 0xbd, 6, 0xcb, 0, 0, 0, 0, 0, 0 };
                string blockNumber = Convert.ToString(lnghex1, 0x10);
                ArrayList list = Form1.IntoByte(lnghex1, blockNumber);
                buffer[7] = (byte) list[0];
                byte num = 0;
                for (int i = 0; i < 8; i++)
                {
                    num = (byte) (num + buffer[i]);
                }
                buffer[8] = num;
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(buffer, 0, 9);
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer2 = new byte[count];
                            Form1.comm.Read(buffer2, 0, count);
                            if ((buffer2[0] == 0xbd) && (buffer2[1] == 4))
                            {
                                byte num5 = 0;
                                for (int k = 0; k < 6; k++)
                                {
                                    num5 = (byte) (num5 + buffer2[k]);
                                }
                                if (num5 != buffer2[6])
                                {
                                }
                                if (buffer2[5] != 0)
                                {
                                    MessageBox.Show("初始密码失败");
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (j == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        public int LoadEquipmentPasswrod(int sector)
        {
            try
            {
                byte[] buffer = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
                string blockNumber = Convert.ToString(sector, 0x10);
                ArrayList list = Form1.IntoByte(sector, blockNumber);
                buffer[7] = (byte) list[0];
                byte num = 0;
                int index = 0;
                while (index < 8)
                {
                    num = (byte) (num + buffer[index]);
                    index++;
                }
                buffer[8] = num;
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(buffer, 0, 9);
                int num3 = 0;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer2 = new byte[count];
                            Form1.comm.Read(buffer2, 0, count);
                            if ((buffer2[0] == 0xbd) && (buffer2[1] == 4))
                            {
                                byte num6 = 0;
                                for (index = 0; index < 6; index++)
                                {
                                    num6 = (byte) (num6 + buffer2[index]);
                                }
                                if (num6 != buffer2[6])
                                {
                                    MessageBox.Show("接收数据不完整");
                                    num3 = 0;
                                }
                                if (buffer2[5] == 0)
                                {
                                    num3 = 1;
                                }
                                else
                                {
                                    num3 = 0;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (i == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                        num3 = 0;
                    }
                }
                return num3;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        private void MasterCardCalibrationTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.portname.Equals(""))
                {
                    MessageBox.Show("没有找到设备,请检查设备是否连接");
                }
                else
                {
                    this.OpenSerialPort();
                    Function.Encryption(Form1.comm, this.portname);
                    this.SetOperationButtonState(false);
                    ChangeTime time = new ChangeTime();
                    if (time.ShowDialog() == DialogResult.OK)
                    {
                    }
                    Function.MasterCardCalibrationTime(Form1.comm, this.portname, time.DeviceDateTime.Value);
                    Form1.comm.Close();
                    this.SetOperationButtonState(true);
                    MessageBox.Show("时间写入母卡成功，请到设备上刷卡。", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    time.Dispose();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                this.SetOperationButtonState(true);
            }
        }

        private void OpenSerialPort()
        {
            try
            {
                if (!Form1.comm.IsOpen)
                {
                    Form1.comm.PortName = this.portname;
                    Form1.comm.BaudRate = 0x2580;
                    try
                    {
                        Form1.comm.Open();
                    }
                    catch (Exception exception)
                    {
                        Form1.comm = new SerialPort();
                        MessageBox.Show(exception.Message);
                    }
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
            }
        }

        public int readsanqu()
        {
            try
            {
                int num = 11;
                byte[] buffer = new byte[] { 0xbd, 5, 0xcf, 0, 0, 0x10, 0, 0xa1 };
                byte[] buffer2 = new byte[11];
                Form1.comm.Write(buffer, 0, 8);
                for (int i = 0; i < 3; i++)
                {
                    if (this.Receive() == 11)
                    {
                        Form1.comm.Read(buffer2, 0, 11);
                        if ((buffer2[0] == 0xbd) && (buffer2[1] == 8))
                        {
                            byte num4 = 0;
                            for (int j = 0; j < 10; j++)
                            {
                                num4 = (byte) (num4 + buffer2[j]);
                            }
                            if (num4 != buffer2[10])
                            {
                                MessageBox.Show("验证出错，可能是数据没发完整");
                                Form1.comm.Close();
                            }
                            else
                            {
                                num = buffer2[9];
                            }
                            break;
                        }
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public int Receive()
        {
            try
            {
                this.OpenSerialPort();
                int bytesToRead = 0;
                this.timer1.Interval = 50;
                this.timer1.Enabled = true;
                while (this.timer1.Enabled)
                {
                    if (Form1.comm.BytesToRead > bytesToRead)
                    {
                        bytesToRead = Form1.comm.BytesToRead;
                        this.timer1.Enabled = false;
                        this.timer1.Interval = 10;
                        this.timer1.Enabled = true;
                    }
                    else
                    {
                        Application.DoEvents();
                    }
                }
                return bytesToRead;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public bool SearchCard()
        {
            try
            {
                byte[] buffer = new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 };
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(buffer, 0, 8);
                Array.Clear(buffer, 0, 8);
                bool flag = false;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer2 = new byte[count];
                            Form1.comm.Read(buffer2, 0, count);
                            if ((buffer2[0] == 0xbd) && (buffer2[1] == 4))
                            {
                                byte num3 = 0;
                                for (int j = 0; j < 6; j++)
                                {
                                    num3 = (byte) (num3 + buffer2[j]);
                                }
                                if (num3 != buffer2[6])
                                {
                                    MessageBox.Show("数据接收不完整");
                                    flag = false;
                                }
                                if (buffer2[5] == 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    MessageBox.Show("没有找到卡");
                                    flag = false;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (i == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                        flag = false;
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        private bool SerachSerialport()
        {
            try
            {
                byte[] buffer = new byte[] { 0xbd, 5, 240, 0, 0, 0, 0, 0xb2 };
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(buffer, 0, 8);
                bool flag = false;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer2 = new byte[count];
                            Form1.comm.Read(buffer2, 0, count);
                            if ((buffer2[0] == 0xbd) && (buffer2[1] == 4))
                            {
                                byte num3 = 0;
                                for (int j = 0; j < 6; j++)
                                {
                                    num3 = (byte) (num3 + buffer2[j]);
                                }
                                if (num3 != buffer2[6])
                                {
                                    MessageBox.Show("数据接收不完整");
                                    break;
                                }
                                if (buffer2[5] == 0)
                                {
                                    flag = true;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (i == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        public bool SetEquipment(int lnghex1)
        {
            try
            {
                this.OpenSerialPort();
                byte[] buffer = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
                int num = (lnghex1 * 4) + 3;
                string blockNumber = Convert.ToString(num, 0x10);
                ArrayList list = Form1.IntoByte(num, blockNumber);
                buffer[7] = (byte) list[0];
                byte num2 = 0;
                for (int i = 0; i < 8; i++)
                {
                    num2 = (byte) (num2 + buffer[i]);
                }
                buffer[8] = num2;
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(buffer, 0, 9);
                Array.Clear(buffer, 0, 9);
                bool flag = false;
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer2 = new byte[count];
                            Form1.comm.Read(buffer2, 0, count);
                            if ((buffer2[0] == 0xbd) && (buffer2[1] == 4))
                            {
                                byte num6 = 0;
                                for (int k = 0; k < 6; k++)
                                {
                                    num6 = (byte) (num6 + buffer2[k]);
                                }
                                if (num6 != buffer2[6])
                                {
                                    MessageBox.Show("接收数据不完整");
                                }
                                if (buffer2[5] == 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    MessageBox.Show("修改密码为设备密码失败");
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (j == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        private void SetOperationButtonState(bool State)
        {
            this.btnAddBlacklist.Enabled = State;
            this.btnDelBlack.Enabled = State;
            this.button1.Enabled = State;
            this.MasterCardCalibrationTime.Enabled = State;
        }

        private void stop()
        {
            try
            {
                byte[] buffer = new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d };
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(buffer, 0, 8);
                Array.Clear(buffer, 0, 8);
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer2 = new byte[count];
                            Form1.comm.Read(buffer2, 0, count);
                            if ((buffer2[0] == 0xbd) && (buffer2[1] == 4))
                            {
                                byte num3 = 0;
                                for (int j = 0; j < 6; j++)
                                {
                                    num3 = (byte) (num3 + buffer2[j]);
                                }
                                if (num3 != buffer2[6])
                                {
                                    MessageBox.Show("数据接收不完整");
                                }
                                return;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (i == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timer1.Enabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        public bool VerificationPassword(ref bool isloaddefaultpassword, ref bool isExecutionconinue, ref int i, byte[] password, int lnghex1)
        {
            try
            {
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(password, 0, 9);
                bool flag = false;
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer = new byte[count];
                            Form1.comm.Read(buffer, 0, count);
                            if ((buffer[0] == 0xbd) && (buffer[1] == 4))
                            {
                                byte num3 = 0;
                                for (int k = 0; k < 6; k++)
                                {
                                    num3 = (byte) (num3 + buffer[k]);
                                }
                                if (num3 != buffer[6])
                                {
                                    MessageBox.Show("数据接收不完整");
                                }
                                if (buffer[5] == 0)
                                {
                                    flag = true;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (j == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        public bool VerificationPassword1(ref bool isloaddefaultpassword, ref bool isExecutionconinue, ref int i, byte[] password, int lnghex1)
        {
            try
            {
                Form1.comm.DiscardInBuffer();
                Form1.comm.Write(password, 0, 9);
                bool flag = false;
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        int count = this.Receive();
                        if (count == 7)
                        {
                            byte[] buffer = new byte[count];
                            Form1.comm.Read(buffer, 0, count);
                            if ((buffer[0] == 0xbd) && (buffer[1] == 4))
                            {
                                byte num3 = 0;
                                for (int k = 0; k < 6; k++)
                                {
                                    num3 = (byte) (num3 + buffer[k]);
                                }
                                if (num3 != buffer[6])
                                {
                                    MessageBox.Show("数据接收不完整");
                                }
                                if (buffer[5] == 0)
                                {
                                    flag = true;
                                }
                                else if (this.LoadEquipmentPasswrod(lnghex1) != 1)
                                {
                                    MessageBox.Show("非法卡");
                                }
                                else
                                {
                                    i--;
                                    isExecutionconinue = true;
                                    isloaddefaultpassword = false;
                                    flag = true;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (j == 3)
                    {
                        MessageBox.Show("接收数据失败！", "提示", MessageBoxButtons.OK);
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        private delegate void barinexcel();

        private delegate void inExcel();
    }
}

