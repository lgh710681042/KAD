namespace 发卡模块
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using 发卡模块.Properties;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Settings.Default.flag == 0)
            {
                if (MessageBox.Show("第一次使用要安装USB接口驱动,是否安装", "提示", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                {
                    Process.Start(@".\PL2303_Prolific_DriverInstaller_v130.exe");
                }
                Settings.Default.flag = 1;
                Settings.Default.Save();
            }
            Form8 form = new Form8();
            if (form.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1(form.Userid));
            }
        }
    }
}

