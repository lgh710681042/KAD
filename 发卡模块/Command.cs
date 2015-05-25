namespace 发卡模块
{
    using System;
    using System.Diagnostics;

    public class Command
    {
        private Process proc = null;

        public Command()
        {
            this.proc = new Process();
        }

        public void RunCmd(string cmd)
        {
            this.proc.StartInfo.CreateNoWindow = true;
            this.proc.StartInfo.FileName = "cmd.exe";
            this.proc.StartInfo.UseShellExecute = false;
            this.proc.StartInfo.RedirectStandardError = true;
            this.proc.StartInfo.RedirectStandardInput = true;
            this.proc.StartInfo.RedirectStandardOutput = true;
            this.proc.Start();
            this.proc.StandardInput.WriteLine(cmd);
            this.proc.Close();
        }

        public void RunProgram(string programName)
        {
            this.RunProgram(programName, "");
        }

        public void RunProgram(string programName, string cmd)
        {
            Process process = new Process {
                StartInfo = { CreateNoWindow = true, FileName = programName, UseShellExecute = false, RedirectStandardError = true, RedirectStandardInput = true, RedirectStandardOutput = true }
            };
            process.Start();
            if (cmd.Length != 0)
            {
                process.StandardInput.WriteLine(cmd);
            }
            process.Close();
        }
    }
}

