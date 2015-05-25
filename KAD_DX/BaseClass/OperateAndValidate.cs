namespace KAD_DX.BaseClass
{
    using CrystalDecisions.CrystalReports.Engine;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    internal class OperateAndValidate
    {
        private BaseOperate boperate = new BaseOperate();
        public const int LOCALE_SLONGDATE = 0x20;
        public const int LOCALE_SSHORTDATE = 0x1f;
        public const int LOCALE_STIME = 0x1003;

        public void autoNum(string P_str_sqlstr, string P_str_table, string P_str_tbColumn, string P_str_codeIndex, string P_str_codeNum, TextBox txt)
        {
            string str = "";
            DataSet set = this.boperate.getds(P_str_sqlstr, P_str_table);
            str = set.Tables[0].Rows[0][0].ToString();
            if (str.Trim() == "")
            {
                txt.Text = P_str_codeIndex + P_str_codeNum;
            }
            else
            {
                str = P_str_codeIndex + ((Convert.ToInt32(str.Substring(2, 7)) + 1)).ToString();
                txt.Text = str;
            }
            set.Dispose();
        }

        public void ChangeSkin(string P_str_skin)
        {
        }

        public ReportDocument CrystalReports(string P_str_creportName, string P_str_sql)
        {
            ReportDocument document = new ReportDocument();
            string str = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf(@"\")).LastIndexOf(@"\")) + @"\SumManage\CReportFile\" + P_str_creportName;
            document.Load(str);
           // document.get_DataDefinition().set_RecordSelectionFormula(P_str_sql);
            return document;
        }

        public byte DexToHex(string P_str_dex)
        {
            if (P_str_dex.Length == 1)
            {
                P_str_dex = "0" + P_str_dex;
            }
            int num = (Convert.ToByte(P_str_dex.Substring(0, 1)) * 0x10) + Convert.ToByte(P_str_dex.Substring(1, 1));
            return (byte) num;
        }

        public string FullFour(string P_str_four)
        {
            switch (P_str_four.Length)
            {
                case 1:
                    P_str_four = "000" + P_str_four;
                    return P_str_four;

                case 2:
                    P_str_four = "00" + P_str_four;
                    return P_str_four;

                case 3:
                    P_str_four = "0" + P_str_four;
                    return P_str_four;
            }
            return P_str_four;
        }

        [DllImport("kernel32.dll")]
        public static extern int GetSystemDefaultLCID();
        public void LockSystem(Form frmLock)
        {
            frmLock.Hide();
        }

        [DllImport("kernel32.dll", EntryPoint="SetLocaleInfoA")]
        public static extern int SetLocaleInfo(int Locale, int LCType, string lpLCData);
        public void SystemDateFormat()
        {
            try
            {
                int systemDefaultLCID = GetSystemDefaultLCID();
                SetLocaleInfo(systemDefaultLCID, 0x1003, "HH:mm:ss");
                SetLocaleInfo(systemDefaultLCID, 0x1f, "yyyy-MM-dd");
                SetLocaleInfo(systemDefaultLCID, 0x20, "yyyy-MM-dd");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        public void unLockSystem(Form frmLock)
        {
            frmLock.Show();
        }

        public bool validateEmail(string P_str_email)
        {
            return Regex.IsMatch(P_str_email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        public bool validateFax(string P_str_fax)
        {
            return Regex.IsMatch(P_str_fax, @"86-\d{2,3}-\d{7,8}");
        }

        public bool validateNAddress(string P_str_naddress)
        {
            return Regex.IsMatch(P_str_naddress, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        public bool validateNum(string P_str_num)
        {
            return Regex.IsMatch(P_str_num, "^[0-9]*$");
        }

        public bool validatePhone(string P_str_phone)
        {
            return Regex.IsMatch(P_str_phone, @"\d{3,4}-\d{7,8}");
        }

        public bool validatePostCode(string P_str_postcode)
        {
            return Regex.IsMatch(P_str_postcode, @"\d{6}");
        }
    }
}

