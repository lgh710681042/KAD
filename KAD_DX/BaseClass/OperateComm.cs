namespace KAD_DX.BaseClass
{
    using System;
    using System.IO.Ports;
    using System.Timers;
    using System.Windows.Forms;

    public class OperateComm
    {
        public static SerialPort mycomm = new SerialPort();
        public static string P_str_baud = "4800";
        public static string P_str_com = "COM1";

        public bool ChangeBaud(int nBaud)
        {
            try
            {
                mycomm.BaudRate = Convert.ToInt32(nBaud);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CloseComm()
        {
            try
            {
                if (mycomm.IsOpen)
                {
                    mycomm.ReadExisting();
                    mycomm.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int comnumber()
        {
            return SerialPort.GetPortNames().Length;
        }

        public void Flish()
        {
            mycomm.DiscardInBuffer();
        }

        public bool OpenComm()
        {
            try
            {
                if (mycomm.IsOpen)
                {
                    mycomm.Close();
                }
                mycomm.PortName = P_str_com;
                mycomm.BaudRate = Convert.ToInt32(P_str_baud);
                mycomm.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] ReadData(int nlen)
        {
            byte[] buffer = new byte[nlen];
            if (mycomm.IsOpen)
            {
                try
                {
                    mycomm.Read(buffer, 0, nlen);
                    return buffer;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public int Receive(System.Windows.Forms.Timer timeReceive, int nOverTime)
        {
            int bytesToRead = 0;
            timeReceive.Interval = nOverTime;
            timeReceive.Enabled = true;
            while (timeReceive.Enabled)
            {
                try
                {
                    if (mycomm.BytesToRead > bytesToRead)
                    {
                        bytesToRead = mycomm.BytesToRead;
                        timeReceive.Enabled = false;
                        timeReceive.Interval = 50;
                        timeReceive.Enabled = true;
                    }
                    else
                    {
                        Application.DoEvents();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            return bytesToRead;
        }

        public int Receive2(System.Timers.Timer timeReceive, int nOverTime)
        {
            int bytesToRead = 0;
            timeReceive.Interval = nOverTime;
            timeReceive.AutoReset = false;
            timeReceive.Enabled = true;
            while (timeReceive.Enabled)
            {
                try
                {
                    if (mycomm.BytesToRead > bytesToRead)
                    {
                        bytesToRead = mycomm.BytesToRead;
                        timeReceive.Enabled = false;
                        timeReceive.Interval = 100.0;
                        timeReceive.Enabled = true;
                    }
                    else
                    {
                        Application.DoEvents();
                    }
                }
                catch
                {
                }
            }
            return bytesToRead;
        }

        public void SendData(byte[] buf, int nlen)
        {
            if (mycomm.IsOpen)
            {
                mycomm.DiscardInBuffer();
                mycomm.Write(buf, 0, nlen);
            }
        }
    }
}

