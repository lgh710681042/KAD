namespace 发卡模块
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO.Ports;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class Function
    {
        public static List<byte> Readbuffer = new List<byte>();
        private static int ReceiveCompleteFlag = 0;
        public static string UserCardCode = string.Empty;

        private static void _SerialPost_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int bytesToRead = ((SerialPort) sender).BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                ((SerialPort) sender).Read(buffer, 0, bytesToRead);
                Readbuffer.AddRange(buffer);
                ((SerialPort) sender).DiscardInBuffer();
                if (((Readbuffer.Count > 5) && (Readbuffer[0] == 0xbd)) && (Readbuffer.Count >= (Readbuffer[1] + 3)))
                {
                    ReceiveCompleteFlag = 1;
                    Console.WriteLine("Received Success!!");
                    Console.WriteLine("EventArgs Return Data" + BitConverter.ToString(Readbuffer.ToArray()));
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Reading Error:" + exception);
                throw new Exception(exception.Message);
            }
        }

        public static void Beel(SerialPort COM, string COMPort)
        {
            List<byte> list = new List<byte>();
            if (!CheckReturnFCS(DataSender(COM, new byte[] { 0xbd, 5, 240, 0, 0, 0, 0, 0xb2 })))
            {
                throw new Exception("通讯错误!");
            }
        }

        public static bool CheckReturnFCS(byte[] Content)
        {
            try
            {
                byte num = 0;
                for (int i = 0; i <= (Content.Length - 1); i++)
                {
                    num = (byte) (num + Content[i]);
                }
                return (num == Content[Content.Length]);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        public static bool CheckReturnFCS(List<byte[]> Content)
        {
            try
            {
                byte num = 0;
                for (int i = 0; i <= (Content[0].Length - 2); i++)
                {
                    num += Content[0][i];
                }
                return (num == Content[0][Content[0].Length - 1]);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        public static bool CheckReturnFCS(List<byte> Content)
        {
            try
            {
                byte num = 0;
                for (int i = 0; i <= (Content.Count - 2); i++)
                {
                    num += Content[i];
                }
                if (num == Content[Content.Count - 1])
                {
                    return true;
                }
                Console.WriteLine("Check return FCS Failure");
                return false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        public static List<byte> DataSender(SerialPort _SerialPort, byte[] _Data)
        {
            List<byte> list;
            _SerialPort.ReadTimeout = 0xbb8;
            _SerialPort.WriteTimeout = 0x3e8;
            int num = 0;
            _SerialPort.DataReceived += new SerialDataReceivedEventHandler(Function._SerialPost_DataReceived);
            try
            {
                int num2;
                bool flag;
                Readbuffer.Clear();
                _SerialPort.DiscardInBuffer();
                _SerialPort.DiscardOutBuffer();
                ReceiveCompleteFlag = 0;
                _SerialPort.Write(_Data, 0, _Data.Length);
                goto Label_018D;
            Label_005F:
                num2 = 15;
                while (num2 != 0)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                    if (ReceiveCompleteFlag == 1)
                    {
                        if ((Readbuffer[1] + 3) > Readbuffer.Count)
                        {
                            for (int i = Readbuffer.Count - (Readbuffer[1] + 3); i >= 0; i--)
                            {
                                Readbuffer.RemoveAt(Readbuffer.Count);
                            }
                        }
                        _SerialPort.DataReceived -= new SerialDataReceivedEventHandler(Function._SerialPost_DataReceived);
                        return Readbuffer;
                    }
                    num2--;
                }
                num++;
                if (num > 2)
                {
                    Readbuffer.Add(0xff);
                    _SerialPort.DataReceived -= new SerialDataReceivedEventHandler(Function._SerialPost_DataReceived);
                    return Readbuffer;
                }
                Readbuffer.Clear();
                _SerialPort.DiscardInBuffer();
                _SerialPort.DiscardOutBuffer();
                _SerialPort.Write(_Data, 0, _Data.Length);
            Label_018D:
                flag = true;
                goto Label_005F;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Received Failure:" + exception.ToString());
                throw new Exception(exception.Message);
            }
            finally
            {
                _SerialPort.DataReceived -= new SerialDataReceivedEventHandler(Function._SerialPost_DataReceived);
            }
            return list;
        }

        public static byte Dex2Hex(string P_str_dex)
        {
            try
            {
                if (P_str_dex.Length == 1)
                {
                    P_str_dex = "0" + P_str_dex;
                }
                int num = (Convert.ToByte(P_str_dex.Substring(0, 1)) * 0x10) + Convert.ToByte(P_str_dex.Substring(1, 1));
                return (byte) num;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                return 0xff;
            }
        }

        public static void Encryption(SerialPort COM, string COMPort)
        {
            new List<byte>().Clear();
            List<byte> content = DataSender(COM, new byte[] { 0xbd, 6, 160, 0, 0, 0, 0, 1, 100 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("非法卡");
            }
        }

        public static string FamilyExtendTheValidity(SerialPort COM, string COMPort, byte To_year, byte To_Mon, byte To_Day, byte UseCountData, List<string> FamilyCardList, string CardOperationCommand, List<string> SelectFamilyCardCodeList, Label DisplayCardCode)
        {
            List<byte> content = new List<byte>();
            OpenSerialPort(COM, COMPort);
            byte num = 0;
            int num2 = 0;
            string str = string.Empty;
            str = ReadCardInformation(COM, COMPort);
            DisplayCardCode.Text = UserCardCode;
            if (str.Split(new char[] { '-' }).Length < 15)
            {
                throw new Exception("读卡失败!");
            }
            byte num3 = 0;
            string str3 = CardOperationCommand;
            if (str3 != null)
            {
                if (!(str3 == "Choose_ExtendTheValidity"))
                {
                    if (str3 == "Choose_AddToBlacklist")
                    {
                        num3 = 3;
                        goto Label_00B7;
                    }
                    if (str3 == "Choose_RemoveToBlacklist")
                    {
                        num3 = 5;
                        goto Label_00B7;
                    }
                }
                else
                {
                    num3 = 1;
                    goto Label_00B7;
                }
            }
            throw new Exception("非法操作!");
        Label_00B7:
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xcf, 0, 0, 0x10, 0, 0xa1 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num = content[9];
            byte[] buffer2 = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = num;
            byte[] context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
            context[7] = num;
            buffer2[8] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("没有找到卡！");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num2 |= content[8] << 0x10;
            num2 |= content[7] << 8;
            num2 |= content[6];
            Console.WriteLine("CardCode:{0}", num2.ToString("D8"));
            content.Clear();
            buffer2 = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = num;
            context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
            context[7] = num;
            buffer2[8] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            buffer2 = new byte[0x19];
            buffer2[0] = 0xbd;
            buffer2[1] = 0x16;
            buffer2[2] = 0xb9;
            buffer2[7] = (byte) ((num * 4) + 1);
            buffer2[9] = num3;
            buffer2[0x10] = (byte) String2Byte(str.Split(new char[] { '-' })[13]);
            buffer2[0x11] = (byte) String2Byte(str.Split(new char[] { '-' })[14]);
            buffer2[0x12] = (byte) String2Byte(str.Split(new char[] { '-' })[15]);
            buffer2[0x13] = (byte) String2Byte(str.Split(new char[] { '-' })[0x10]);
            buffer2[20] = UseCountData;
            buffer2[0x15] = To_year;
            buffer2[0x16] = To_Mon;
            buffer2[0x17] = To_Day;
            context = new byte[0x18];
            context[0] = 0xbd;
            context[1] = 0x16;
            context[2] = 0xb9;
            context[7] = (byte) ((num * 4) + 1);
            context[9] = num3;
            context[0x10] = (byte) String2Byte(str.Split(new char[] { '-' })[13]);
            context[0x11] = (byte) String2Byte(str.Split(new char[] { '-' })[14]);
            context[0x12] = (byte) String2Byte(str.Split(new char[] { '-' })[15]);
            context[0x13] = (byte) String2Byte(str.Split(new char[] { '-' })[0x10]);
            context[20] = UseCountData;
            context[0x15] = To_year;
            context[0x16] = To_Mon;
            context[0x17] = To_Day;
            buffer2[0x18] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("没有找到卡！");
            }
            content.Clear();
            buffer2 = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = (byte) ((num * 4) + 3);
            context = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0 };
            context[7] = (byte) ((num * 4) + 3);
            buffer2[8] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (SelectFamilyCardCodeList.Count != 0)
            {
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                buffer2 = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num + 3);
                context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num + 3);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("没有找到卡！");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                num2 |= content[8] << 0x10;
                num2 |= content[7] << 8;
                num2 |= content[6];
                Console.WriteLine("CardCode:{0}", num2.ToString("D8"));
                content.Clear();
                buffer2 = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num + 3);
                context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num + 3);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                buffer2 = new byte[] { 
                    0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0
                 };
                buffer2[7] = (byte) (((num * 4) + 3) + 1);
                byte[] buffer = buffer2;
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(":" + i);
                    if (i < 5)
                    {
                        if (i < SelectFamilyCardCodeList.Count)
                        {
                            buffer[7] = (byte) (((num + 3) * 4) + 1);
                            buffer[((i * 3) + 7) + 1] = (byte) (int.Parse(SelectFamilyCardCodeList[i]) >> 0x10);
                            buffer[((i * 3) + 7) + 2] = (byte) (int.Parse(SelectFamilyCardCodeList[i]) >> 8);
                            buffer[((i * 3) + 7) + 3] = (byte) int.Parse(SelectFamilyCardCodeList[i]);
                        }
                        else
                        {
                            buffer[7] = (byte) (((num + 3) * 4) + 1);
                            buffer[((i * 3) + 7) + 1] = 0;
                            buffer[((i * 3) + 7) + 2] = 0;
                            buffer[((i * 3) + 7) + 3] = 0;
                        }
                    }
                    else
                    {
                        if (i == 5)
                        {
                            buffer[buffer.Length - 1] = 0;
                            buffer[buffer.Length - 1] = FCSCreate(buffer);
                            content.Clear();
                            content = DataSender(COM, buffer);
                            if (!CheckReturnFCS(content))
                            {
                                throw new Exception("通讯错误!");
                            }
                        }
                        if (i < SelectFamilyCardCodeList.Count)
                        {
                            buffer[7] = (byte) (((num + 3) * 4) + 2);
                            buffer[(((i - 5) * 3) + 7) + 1] = (byte) (int.Parse(SelectFamilyCardCodeList[i]) >> 0x10);
                            buffer[(((i - 5) * 3) + 7) + 2] = (byte) (int.Parse(SelectFamilyCardCodeList[i]) >> 8);
                            buffer[(((i - 5) * 3) + 7) + 3] = (byte) int.Parse(SelectFamilyCardCodeList[i]);
                        }
                        else
                        {
                            buffer[7] = (byte) (((num + 3) * 4) + 2);
                            buffer[(((i - 5) * 3) + 7) + 1] = 0;
                            buffer[(((i - 5) * 3) + 7) + 2] = 0;
                            buffer[(((i - 5) * 3) + 7) + 3] = 0;
                        }
                        if (i == 9)
                        {
                            buffer[buffer.Length - 1] = 0;
                            buffer[buffer.Length - 1] = FCSCreate(buffer);
                            content.Clear();
                            content = DataSender(COM, buffer);
                            if (!CheckReturnFCS(content))
                            {
                                throw new Exception("通讯错误!");
                            }
                        }
                    }
                }
                content.Clear();
                buffer2 = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (((num + 3) * 4) + 2);
                context = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0 };
                context[7] = (byte) (((num + 3) * 4) + 2);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                content.Clear();
                if (!CheckReturnFCS(DataSender(COM, new byte[] { 0xbd, 5, 240, 0, 0, 0, 0, 0xb2 })))
                {
                    throw new Exception("通讯错误!");
                }
            }
            return num2.ToString();
        }

        public static byte FCSCreate(byte[] Context)
        {
            try
            {
                byte num = 0;
                for (int i = 0; i <= (Context.Length - 1); i++)
                {
                    num = (byte) (num + Context[i]);
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

        public static void FindCheckNode(TreeNode node, List<TreeNode> ListNodes)
        {
            if (node.Checked)
            {
                ListNodes.Add(node);
            }
            foreach (TreeNode node2 in node.Nodes)
            {
                FindCheckNode(node2, ListNodes);
            }
        }

        public static TreeNode FindNode_NonRecursive(TreeNode tnParent, string strValue)
        {
            if (tnParent != null)
            {
                if (tnParent.Text == strValue)
                {
                    return tnParent;
                }
                if (tnParent.Nodes.Count == 0)
                {
                    return null;
                }
                TreeNode parent = tnParent;
                TreeNode firstNode = parent.FirstNode;
                while ((firstNode != null) && (firstNode != tnParent))
                {
                    while (firstNode != null)
                    {
                        if (firstNode.Text == strValue)
                        {
                            return firstNode;
                        }
                        if (firstNode.Nodes.Count > 0)
                        {
                            parent = firstNode;
                            firstNode = firstNode.FirstNode;
                        }
                        else
                        {
                            if (firstNode == parent.LastNode)
                            {
                                break;
                            }
                            firstNode = firstNode.NextNode;
                        }
                    }
                    while ((firstNode != tnParent) && (firstNode == parent.LastNode))
                    {
                        firstNode = parent;
                        parent = parent.Parent;
                    }
                    if (firstNode != tnParent)
                    {
                        firstNode = firstNode.NextNode;
                    }
                }
            }
            return null;
        }

        public static TreeNode FindNode_Recursion(TreeNode tnParent, string strValue)
        {
            if (tnParent == null)
            {
                return null;
            }
            if (tnParent.Text == strValue)
            {
                return tnParent;
            }
            TreeNode node = null;
            foreach (TreeNode node2 in tnParent.Nodes)
            {
                node = FindNode_Recursion(node2, strValue);
                if (node != null)
                {
                    return node;
                }
            }
            return node;
        }

        public static TreeNode FindTreeNodeInTreeView(TreeView _TreeView, string TreeNodeText)
        {
            TreeNode node = null;
            foreach (TreeNode node2 in _TreeView.Nodes)
            {
                node = FindNode_NonRecursive(node2, TreeNodeText);
                if (node != null)
                {
                    return node;
                }
            }
            return new TreeNode("Not Find TreeNode!");
        }

        public static void GetAllNodeText(TreeNodeCollection tnc, List<string> _SelectNodeList)
        {
            foreach (TreeNode node in tnc)
            {
                if (node.Nodes.Count != 0)
                {
                    GetAllNodeText(node.Nodes, _SelectNodeList);
                }
                if (node.Text.IndexOf("设备编码") > 1)
                {
                    _SelectNodeList.Add(node.Text + ":" + node.Checked.ToString());
                }
            }
        }

        public static void GetCardDeviceNumber(SerialPort COM, string COMPort, TreeView _TreeView)
        {
            int num3;
            string str;
            int num7;
            bool flag2;
            List<byte> content = new List<byte>();
            List<byte[]> list2 = new List<byte[]>();
            List<byte> list3 = new List<byte>();
            SerialPort port = OpenSerialPort(COM, COMPort);
            byte num = 0;
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xcf, 0, 0, 0x10, 0, 0xa1 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num = content[9];
            int index = 1;
            while (index < 4)
            {
                byte[] buffer2 = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num + index);
                byte[] context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num + index);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                content.Clear();
                buffer2 = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num + index);
                context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num + index);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                num3 = 0;
                while (num3 < 3)
                {
                    content.Clear();
                    buffer2 = new byte[] { 0xbd, 6, 0xb7, 0, 0, 0, 0, 0, 0 };
                    buffer2[7] = (byte) (((num + index) * 4) + num3);
                    context = new byte[] { 0xbd, 6, 0xb7, 0, 0, 0, 0, 0 };
                    context[7] = (byte) (((num + index) * 4) + num3);
                    buffer2[8] = FCSCreate(context);
                    content = DataSender(COM, buffer2);
                    if (!CheckReturnFCS(content))
                    {
                        throw new Exception("通讯错误!");
                    }
                    Console.WriteLine("Device Number:" + BitConverter.ToString(content.ToArray()));
                    list2.Add(content.ToArray());
                    num3++;
                }
                index++;
            }
            Console.WriteLine("Read Card Device Number Success!");
            list2.RemoveAt(8);
            if (list2.Count != 8)
            {
                throw new Exception("通讯错误!");
            }
            foreach (byte[] buffer in list2)
            {
                index = 5;
                while (index < 0x15)
                {
                    list3.Add(buffer[index]);
                    Console.WriteLine("i Value:{0},DeviceNumberBlock:{1},:{2}", index, buffer.Length, BitConverter.ToString(buffer));
                    index++;
                }
            }
            Console.WriteLine("CardDeviceNumberData Value = " + BitConverter.ToString(list3.ToArray()));
            int num4 = GetNodeCount(_TreeView) + 1;
            Console.WriteLine("Device Node Count = " + num4);
            int num5 = 0;
            int num6 = 0;
            num5 = num4 / 8;
            num6 = num4 % 8;
            List<string> list4 = new List<string>();
            GetAllNodeText(_TreeView.Nodes, list4);
            Console.WriteLine("ReadCardDeviceNumberByteCount = {0}", num5);
            Console.WriteLine("ReadCardDeviceNumberBitCount = {0}", num6);
            for (index = 0; index < num5; index++)
            {
                for (num3 = 0; num3 < 8; num3++)
                {
                    str = string.Empty;
                    num7 = 0;
                    while (num7 < list4.Count)
                    {
                        Console.WriteLine(string.Format("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!(设备编码:{0})", (index * 8) + num3));
                        if (list4[num7].Contains(string.Format("(设备编码:{0})", (index * 8) + num3)))
                        {
                            str = list4[num7].Split(new char[] { ':' })[0] + ":" + list4[num7].Split(new char[] { ':' })[1];
                            break;
                        }
                        num7++;
                    }
                    FindTreeNodeInTreeView(_TreeView, str).Checked = (list3[index] & (((int) 0x80) >> num3)) >= 1;
                    Console.WriteLine(string.Format("{0}(设备编码:{0})", (index * 8) + num3) + ".Checked = " + (((list3[index] & (((int) 0x80) >> num3)) >= 1) ? (flag2 = true).ToString() : (flag2 = false).ToString()) + "  TempNodeName:" + str);
                }
            }
            for (index = 0; index < num6; index++)
            {
                str = string.Empty;
                for (num7 = 0; num7 < list4.Count; num7++)
                {
                    if (list4[num7].Contains(string.Format("(设备编码:{0})", index + (num5 * 8))))
                    {
                        str = list4[num7].Split(new char[] { ':' })[0] + ":" + list4[num7].Split(new char[] { ':' })[1];
                        break;
                    }
                }
                FindTreeNodeInTreeView(_TreeView, str).Checked = (list3[num5] & (((int) 0x80) >> index)) >= 1;
                Console.WriteLine(string.Format("{0}(设备编码:{0})", index + (num5 * 8)) + ".Checked = " + (((list3[num5] & (((int) 0x80) >> index)) >= 1) ? (flag2 = true).ToString() : (flag2 = false).ToString()));
            }
            content.Clear();
            if (!CheckReturnFCS(DataSender(COM, new byte[] { 0xbd, 5, 240, 0, 0, 0, 0, 0xb2 })))
            {
                throw new Exception("通讯错误!");
            }
        }

        public static int GetNodeCount(TreeView _TreeView)
        {
            int num = 0;
            foreach (TreeNode node in _TreeView.Nodes)
            {
                num += node.Nodes.Count;
            }
            return num;
        }

        public static void MasterCardCalibrationTime(SerialPort COM, string COMPort, DateTime DeviceTimeDate)
        {
            byte num4;
            string[] strArray = DeviceTimeDate.ToString("yyyy-MM-dd HH:mm:ss").Split(new char[] { ' ' })[1].Split(new char[] { ':' });
            string[] strArray2 = DeviceTimeDate.ToString("yyyy-MM-dd HH:mm:ss").Split(new char[] { ' ' })[0].Split(new char[] { '-' });
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday":
                    num4 = 1;
                    break;

                case "Tuesday":
                    num4 = 2;
                    break;

                case "Wednesday":
                    num4 = 3;
                    break;

                case "Thursday":
                    num4 = 4;
                    break;

                case "Friday":
                    num4 = 5;
                    break;

                case "Saturday":
                    num4 = 6;
                    break;

                case "Sunday":
                    num4 = 0;
                    break;

                default:
                    num4 = 0;
                    break;
            }
            byte num = Dex2Hex(strArray2[0].Substring(2, 2));
            byte num2 = Dex2Hex(strArray2[1]);
            byte num3 = Dex2Hex(strArray2[2]);
            byte num5 = Dex2Hex(strArray[0]);
            byte num6 = Dex2Hex(strArray[1]);
            byte num7 = Dex2Hex(strArray[2]);
            List<byte> content = new List<byte>();
            OpenSerialPort(COM, COMPort);
            byte num8 = 0;
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xcf, 0, 0, 0x10, 0, 0xa1 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num8 = content[9];
            byte[] buffer = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
            buffer[7] = num8;
            byte[] context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
            context[7] = num8;
            buffer[8] = FCSCreate(context);
            content = DataSender(COM, buffer);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("没有找到卡。");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            buffer = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
            buffer[7] = num8;
            context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
            context[7] = num8;
            buffer[8] = FCSCreate(context);
            content = DataSender(COM, buffer);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            buffer = new byte[] { 
                0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0
             };
            buffer[7] = (byte) ((num8 * 4) + 1);
            buffer[0x10] = num;
            buffer[0x11] = num2;
            buffer[0x12] = num3;
            buffer[0x13] = num4;
            buffer[20] = num5;
            buffer[0x15] = num6;
            buffer[0x16] = num7;
            context = new byte[] { 
                0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0
             };
            context[7] = (byte) ((num8 * 4) + 1);
            context[0x10] = num;
            context[0x11] = num2;
            context[0x12] = num3;
            context[0x13] = num4;
            context[20] = num5;
            context[0x15] = num6;
            context[0x16] = num7;
            buffer[0x18] = FCSCreate(context);
            content = DataSender(COM, buffer);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            buffer = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
            buffer[7] = (byte) ((num8 * 4) + 3);
            context = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0 };
            context[7] = (byte) ((num8 * 4) + 3);
            buffer[8] = FCSCreate(context);
            content = DataSender(COM, buffer);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            if (!CheckReturnFCS(DataSender(COM, new byte[] { 0xbd, 5, 240, 0, 0, 0, 0, 0xb2 })))
            {
                throw new Exception("通讯错误!");
            }
        }

        [DllImport("msvcrt.dll")]
        private static extern IntPtr memcmp(byte[] b1, byte[] b2, IntPtr count);
        public static int MemoryCompare(byte[] b1, byte[] b2)
        {
            return memcmp(b1, b2, new IntPtr(b1.Length)).ToInt32();
        }

        public static SerialPort OpenSerialPort(SerialPort comm, string COMPort)
        {
            SerialPort port;
            try
            {
                if (comm.IsOpen)
                {
                    return comm;
                }
                comm.PortName = COMPort;
                comm.BaudRate = 0x2580;
                try
                {
                    comm.Open();
                }
                catch (Exception exception)
                {
                    comm = new SerialPort();
                    throw new Exception(exception.Message);
                }
                port = comm;
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message.ToString());
                Console.WriteLine(exception2.ToString());
                throw exception2;
            }
            return port;
        }

        public static string ReadCardCode(SerialPort COM, string COMPort)
        {
            List<byte> content = new List<byte>();
            byte num = 0;
            int num2 = 0;
            SerialPort port = OpenSerialPort(COM, COMPort);
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xcf, 0, 0, 0x10, 0, 0xa1 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num = content[9];
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("没有找到卡！");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num2 |= content[8] << 0x10;
            num2 |= content[7] << 8;
            num2 |= content[6];
            Console.WriteLine("CardCode:{0}", num2.ToString("D8"));
            return num2.ToString("D8");
        }

        public static string ReadCardInformation(SerialPort COM, string COMPort)
        {
            List<byte> content = new List<byte>();
            byte num = 0;
            int num2 = 0;
            SerialPort port = OpenSerialPort(COM, COMPort);
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xcf, 0, 0, 0x10, 0, 0xa1 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num = content[9];
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("没有找到卡！");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num2 |= content[8] << 0x10;
            num2 |= content[7] << 8;
            num2 |= content[6];
            Console.WriteLine("CardCode:{0}", num2.ToString("D8"));
            UserCardCode = num2.ToString("D8");
            content.Clear();
            byte[] buffer = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
            buffer[7] = num;
            byte[] context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
            context[7] = num;
            buffer[8] = FCSCreate(context);
            content = DataSender(COM, buffer);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            buffer = new byte[] { 0xbd, 6, 0xb7, 0, 0, 0, 0, 0, 0 };
            buffer[7] = (byte) ((num * 4) + 1);
            context = new byte[] { 0xbd, 6, 0xb7, 0, 0, 0, 0, 0 };
            context[7] = (byte) ((num * 4) + 1);
            buffer[8] = FCSCreate(context);
            content = DataSender(COM, buffer);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            return BitConverter.ToString(content.ToArray());
        }

        public static string SendCard(SerialPort COM, string COMPort, byte UseCount, byte Start_Year, byte Start_Mon, byte Start_Day, byte End_Year, byte End_Mon, byte End_Day, byte TimeFragment, TreeView _TreeView)
        {
            // This item is obfuscated and can not be translated.
            List<byte> content = new List<byte>();
            int nodeCount = GetNodeCount(_TreeView);
            List<string> list2 = new List<string>();
            byte[] buffer = new byte[0x70];
            int num2 = 0;
            OpenSerialPort(COM, COMPort);
            byte num3 = 0;
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xcf, 0, 0, 0x10, 0, 0xa1 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num3 = content[9];
            byte[] buffer2 = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = num3;
            byte[] context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
            context[7] = num3;
            buffer2[8] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("没有找到卡！");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            num2 |= content[8] << 0x10;
            num2 |= content[7] << 8;
            num2 |= content[6];
            Console.WriteLine("CardCode:{0}", num2.ToString("D8"));
            content.Clear();
            buffer2 = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = num3;
            context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
            context[7] = num3;
            buffer2[8] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("非法卡!");
            }
            content.Clear();
            buffer2 = new byte[0x19];
            buffer2[0] = 0xbd;
            buffer2[1] = 0x16;
            buffer2[2] = 0xb9;
            buffer2[7] = (byte) ((num3 * 4) + 1);
            buffer2[0x10] = Start_Year;
            buffer2[0x11] = Start_Mon;
            buffer2[0x12] = Start_Day;
            buffer2[0x13] = TimeFragment;
            buffer2[20] = UseCount;
            buffer2[0x15] = End_Year;
            buffer2[0x16] = End_Mon;
            buffer2[0x17] = End_Day;
            context = new byte[] { 
                0xbd, 0x16, 0xb9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0
             };
            context[7] = (byte) ((num3 * 4) + 1);
            context[0x10] = Start_Year;
            context[0x11] = Start_Mon;
            context[0x12] = Start_Day;
            context[0x13] = TimeFragment;
            context[20] = UseCount;
            context[0x15] = End_Year;
            context[0x16] = End_Mon;
            context[0x17] = End_Day;
            buffer2[0x18] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            buffer2 = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = (byte) ((num3 * 4) + 3);
            context = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0 };
            context[7] = (byte) ((num3 * 4) + 3);
            buffer2[8] = FCSCreate(context);
            if (!CheckReturnFCS(DataSender(COM, buffer2)))
            {
                throw new Exception("通讯错误!");
            }
            GetAllNodeText(_TreeView.Nodes, list2);
            list2.Sort();
            for (int i = 0; i < (nodeCount + 1); i++)
            {
                Console.WriteLine("(j / 8 ) Value = {0},j Value = {1} , TreeViewCount Value = {2}", i / 8, i, nodeCount);
                for (int j = 0; j < list2.Count; j++)
                {
                    if (!list2[j].Contains(string.Format("(设备编码:{0}):True", i)) && ((i % 8) != 0 || true))
                    {
                    }
                    (buffer[i / 8]) = (byte) (0 | ((byte) (((int) 0x80) >> (i % 8))));
                }
            }
            buffer2 = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = (byte) (num3 + 1);
            context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
            context[7] = (byte) (num3 + 1);
            buffer2[8] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            buffer2 = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = (byte) (num3 + 1);
            context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
            context[7] = (byte) (num3 + 1);
            buffer2[8] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            content.Clear();
            buffer2 = new byte[0x19];
            buffer2[0] = 0xbd;
            buffer2[1] = 0x16;
            buffer2[2] = 0xb9;
            buffer2[7] = (byte) ((num3 + 1) * 4);
            buffer2[8] = buffer[0];
            buffer2[9] = buffer[1];
            buffer2[10] = buffer[2];
            buffer2[11] = buffer[3];
            buffer2[12] = buffer[4];
            buffer2[13] = buffer[5];
            buffer2[14] = buffer[6];
            buffer2[15] = buffer[7];
            buffer2[0x10] = buffer[8];
            buffer2[0x11] = buffer[9];
            buffer2[0x12] = buffer[10];
            buffer2[0x13] = buffer[11];
            buffer2[20] = buffer[12];
            buffer2[0x15] = buffer[13];
            buffer2[0x16] = buffer[14];
            buffer2[0x17] = buffer[15];
            context = new byte[0x18];
            context[0] = 0xbd;
            context[1] = 0x16;
            context[2] = 0xb9;
            context[7] = (byte) ((num3 + 1) * 4);
            context[8] = buffer[0];
            context[9] = buffer[1];
            context[10] = buffer[2];
            context[11] = buffer[3];
            context[12] = buffer[4];
            context[13] = buffer[5];
            context[14] = buffer[6];
            context[15] = buffer[7];
            context[0x10] = buffer[8];
            context[0x11] = buffer[9];
            context[0x12] = buffer[10];
            context[0x13] = buffer[11];
            context[20] = buffer[12];
            context[0x15] = buffer[13];
            context[0x16] = buffer[14];
            context[0x17] = buffer[15];
            buffer2[0x18] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            buffer2 = new byte[0x19];
            buffer2[0] = 0xbd;
            buffer2[1] = 0x16;
            buffer2[2] = 0xb9;
            buffer2[7] = (byte) (((num3 + 1) * 4) + 1);
            buffer2[8] = buffer[0x10];
            buffer2[9] = buffer[0x11];
            buffer2[10] = buffer[0x12];
            buffer2[11] = buffer[0x13];
            buffer2[12] = buffer[20];
            buffer2[13] = buffer[0x15];
            buffer2[14] = buffer[0x16];
            buffer2[15] = buffer[0x17];
            buffer2[0x10] = buffer[0x18];
            buffer2[0x11] = buffer[0x19];
            buffer2[0x12] = buffer[0x1a];
            buffer2[0x13] = buffer[0x1b];
            buffer2[20] = buffer[0x1c];
            buffer2[0x15] = buffer[0x1d];
            buffer2[0x16] = buffer[30];
            buffer2[0x17] = buffer[0x1f];
            context = new byte[0x18];
            context[0] = 0xbd;
            context[1] = 0x16;
            context[2] = 0xb9;
            context[7] = (byte) (((num3 + 1) * 4) + 1);
            context[8] = buffer[0x10];
            context[9] = buffer[0x11];
            context[10] = buffer[0x12];
            context[11] = buffer[0x13];
            context[12] = buffer[20];
            context[13] = buffer[0x15];
            context[14] = buffer[0x16];
            context[15] = buffer[0x17];
            context[0x10] = buffer[0x18];
            context[0x11] = buffer[0x19];
            context[0x12] = buffer[0x1a];
            context[0x13] = buffer[0x1b];
            context[20] = buffer[0x1c];
            context[0x15] = buffer[0x1d];
            context[0x16] = buffer[30];
            context[0x17] = buffer[0x1f];
            buffer2[0x18] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            buffer2 = new byte[0x19];
            buffer2[0] = 0xbd;
            buffer2[1] = 0x16;
            buffer2[2] = 0xb9;
            buffer2[7] = (byte) (((num3 + 1) * 4) + 2);
            buffer2[8] = buffer[0x20];
            buffer2[9] = buffer[0x21];
            buffer2[10] = buffer[0x22];
            buffer2[11] = buffer[0x23];
            buffer2[12] = buffer[0x24];
            buffer2[13] = buffer[0x25];
            buffer2[14] = buffer[0x26];
            buffer2[15] = buffer[0x27];
            buffer2[0x10] = buffer[40];
            buffer2[0x11] = buffer[0x29];
            buffer2[0x12] = buffer[0x2a];
            buffer2[0x13] = buffer[0x2b];
            buffer2[20] = buffer[0x2c];
            buffer2[0x15] = buffer[0x2d];
            buffer2[0x16] = buffer[0x2e];
            buffer2[0x17] = buffer[0x2f];
            context = new byte[0x18];
            context[0] = 0xbd;
            context[1] = 0x16;
            context[2] = 0xb9;
            context[7] = (byte) (((num3 + 1) * 4) + 2);
            context[8] = buffer[0x20];
            context[9] = buffer[0x21];
            context[10] = buffer[0x22];
            context[11] = buffer[0x23];
            context[12] = buffer[0x24];
            context[13] = buffer[0x25];
            context[14] = buffer[0x26];
            context[15] = buffer[0x27];
            context[0x10] = buffer[40];
            context[0x11] = buffer[0x29];
            context[0x12] = buffer[0x2a];
            context[0x13] = buffer[0x2b];
            context[20] = buffer[0x2c];
            context[0x15] = buffer[0x2d];
            context[0x16] = buffer[0x2e];
            context[0x17] = buffer[0x2f];
            buffer2[0x18] = FCSCreate(context);
            content = DataSender(COM, buffer2);
            if (!CheckReturnFCS(content))
            {
                throw new Exception("通讯错误!");
            }
            if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
            {
                throw new Exception("无效卡");
            }
            content.Clear();
            buffer2 = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
            buffer2[7] = (byte) (((num3 + 1) * 4) + 3);
            context = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0 };
            context[7] = (byte) (((num3 + 1) * 4) + 3);
            buffer2[8] = FCSCreate(context);
            if (!CheckReturnFCS(DataSender(COM, buffer2)))
            {
                throw new Exception("通讯错误!");
            }
            if (nodeCount >= 0x180)
            {
                buffer2 = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num3 + 2);
                context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num3 + 2);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                content.Clear();
                buffer2 = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num3 + 2);
                context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num3 + 2);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                content.Clear();
                buffer2 = new byte[0x19];
                buffer2[0] = 0xbd;
                buffer2[1] = 0x16;
                buffer2[2] = 0xb9;
                buffer2[7] = (byte) ((num3 + 2) * 4);
                buffer2[8] = buffer[0x30];
                buffer2[9] = buffer[0x31];
                buffer2[10] = buffer[50];
                buffer2[11] = buffer[0x33];
                buffer2[12] = buffer[0x34];
                buffer2[13] = buffer[0x35];
                buffer2[14] = buffer[0x36];
                buffer2[15] = buffer[0x37];
                buffer2[0x10] = buffer[0x38];
                buffer2[0x11] = buffer[0x39];
                buffer2[0x12] = buffer[0x3a];
                buffer2[0x13] = buffer[0x3b];
                buffer2[20] = buffer[60];
                buffer2[0x15] = buffer[0x3d];
                buffer2[0x16] = buffer[0x3e];
                buffer2[0x17] = buffer[0x3f];
                context = new byte[0x18];
                context[0] = 0xbd;
                context[1] = 0x16;
                context[2] = 0xb9;
                context[7] = (byte) ((num3 + 2) * 4);
                context[8] = buffer[0x30];
                context[9] = buffer[0x31];
                context[10] = buffer[50];
                context[11] = buffer[0x33];
                context[12] = buffer[0x34];
                context[13] = buffer[0x35];
                context[14] = buffer[0x36];
                context[15] = buffer[0x37];
                context[0x10] = buffer[0x38];
                context[0x11] = buffer[0x39];
                context[0x12] = buffer[0x3a];
                context[0x13] = buffer[0x3b];
                context[20] = buffer[60];
                context[0x15] = buffer[0x3d];
                context[0x16] = buffer[0x3e];
                context[0x17] = buffer[0x3f];
                buffer2[0x18] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                buffer2 = new byte[0x19];
                buffer2[0] = 0xbd;
                buffer2[1] = 0x16;
                buffer2[2] = 0xb9;
                buffer2[7] = (byte) (((num3 + 2) * 4) + 1);
                buffer2[8] = buffer[0x40];
                buffer2[9] = buffer[0x41];
                buffer2[10] = buffer[0x42];
                buffer2[11] = buffer[0x43];
                buffer2[12] = buffer[0x44];
                buffer2[13] = buffer[0x45];
                buffer2[14] = buffer[70];
                buffer2[15] = buffer[0x47];
                buffer2[0x10] = buffer[0x48];
                buffer2[0x11] = buffer[0x49];
                buffer2[0x12] = buffer[0x4a];
                buffer2[0x13] = buffer[0x4b];
                buffer2[20] = buffer[0x4c];
                buffer2[0x15] = buffer[0x4d];
                buffer2[0x16] = buffer[0x4e];
                buffer2[0x17] = buffer[0x4f];
                context = new byte[0x18];
                context[0] = 0xbd;
                context[1] = 0x16;
                context[2] = 0xb9;
                context[7] = (byte) (((num3 + 2) * 4) + 1);
                context[8] = buffer[0x40];
                context[9] = buffer[0x41];
                context[10] = buffer[0x42];
                context[11] = buffer[0x43];
                context[12] = buffer[0x44];
                context[13] = buffer[0x45];
                context[14] = buffer[70];
                context[15] = buffer[0x47];
                context[0x10] = buffer[0x48];
                context[0x11] = buffer[0x49];
                context[0x12] = buffer[0x4a];
                context[0x13] = buffer[0x4b];
                context[20] = buffer[0x4c];
                context[0x15] = buffer[0x4d];
                context[0x16] = buffer[0x4e];
                context[0x17] = buffer[0x4f];
                buffer2[0x18] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                buffer2 = new byte[0x19];
                buffer2[0] = 0xbd;
                buffer2[1] = 0x16;
                buffer2[2] = 0xb9;
                buffer2[7] = (byte) (((num3 + 2) * 4) + 2);
                buffer2[8] = buffer[80];
                buffer2[9] = buffer[0x51];
                buffer2[10] = buffer[0x52];
                buffer2[11] = buffer[0x53];
                buffer2[12] = buffer[0x54];
                buffer2[13] = buffer[0x55];
                buffer2[14] = buffer[0x56];
                buffer2[15] = buffer[0x57];
                buffer2[0x10] = buffer[0x58];
                buffer2[0x11] = buffer[0x59];
                buffer2[0x12] = buffer[90];
                buffer2[0x13] = buffer[0x5b];
                buffer2[20] = buffer[0x5c];
                buffer2[0x15] = buffer[0x5d];
                buffer2[0x16] = buffer[0x5e];
                buffer2[0x17] = buffer[0x5f];
                context = new byte[0x18];
                context[0] = 0xbd;
                context[1] = 0x16;
                context[2] = 0xb9;
                context[7] = (byte) (((num3 + 2) * 4) + 2);
                context[8] = buffer[80];
                context[9] = buffer[0x51];
                context[10] = buffer[0x52];
                context[11] = buffer[0x53];
                context[12] = buffer[0x54];
                context[13] = buffer[0x55];
                context[14] = buffer[0x56];
                context[15] = buffer[0x57];
                context[0x10] = buffer[0x58];
                context[0x11] = buffer[0x59];
                context[0x12] = buffer[90];
                context[0x13] = buffer[0x5b];
                context[20] = buffer[0x5c];
                context[0x15] = buffer[0x5d];
                context[0x16] = buffer[0x5e];
                context[0x17] = buffer[0x5f];
                buffer2[0x18] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                buffer2 = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (((num3 + 2) * 4) + 3);
                context = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0 };
                context[7] = (byte) (((num3 + 2) * 4) + 3);
                buffer2[8] = FCSCreate(context);
                if (!CheckReturnFCS(DataSender(COM, buffer2)))
                {
                    throw new Exception("通讯错误!");
                }
                if (nodeCount < 0x300)
                {
                    return num2.ToString("D8");
                }
                buffer2 = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num3 + 3);
                context = new byte[] { 0xbd, 6, 0xca, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num3 + 3);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xbb, 0, 0, 0, 0, 0x7d });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb1, 0, 0, 0, 0, 0x73 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                content = DataSender(COM, new byte[] { 0xbd, 5, 0xb3, 0, 0, 0, 0, 0x75 });
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                content.Clear();
                buffer2 = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (num3 + 3);
                context = new byte[] { 0xbd, 6, 0xb5, 0, 0, 0, 0, 0 };
                context[7] = (byte) (num3 + 3);
                buffer2[8] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                content.Clear();
                buffer2 = new byte[0x19];
                buffer2[0] = 0xbd;
                buffer2[1] = 0x16;
                buffer2[2] = 0xb9;
                buffer2[7] = (byte) ((num3 + 3) * 4);
                buffer2[8] = buffer[0x60];
                buffer2[9] = buffer[0x61];
                buffer2[10] = buffer[0x62];
                buffer2[11] = buffer[0x63];
                buffer2[12] = buffer[100];
                buffer2[13] = buffer[0x65];
                buffer2[14] = buffer[0x66];
                buffer2[15] = buffer[0x67];
                buffer2[0x10] = buffer[0x68];
                buffer2[0x11] = buffer[0x69];
                buffer2[0x12] = buffer[0x6a];
                buffer2[0x13] = buffer[0x6b];
                buffer2[20] = buffer[0x6c];
                buffer2[0x15] = buffer[0x6d];
                buffer2[0x16] = buffer[110];
                buffer2[0x17] = buffer[0x6f];
                context = new byte[0x18];
                context[0] = 0xbd;
                context[1] = 0x16;
                context[2] = 0xb9;
                context[7] = (byte) ((num3 + 3) * 4);
                context[8] = buffer[0x60];
                context[9] = buffer[0x61];
                context[10] = buffer[0x62];
                context[11] = buffer[0x63];
                context[12] = buffer[100];
                context[13] = buffer[0x65];
                context[14] = buffer[0x66];
                context[15] = buffer[0x67];
                context[0x10] = buffer[0x68];
                context[0x11] = buffer[0x69];
                context[0x12] = buffer[0x6a];
                context[0x13] = buffer[0x6b];
                context[20] = buffer[0x6c];
                context[0x15] = buffer[0x6d];
                context[0x16] = buffer[110];
                context[0x17] = buffer[0x6f];
                buffer2[0x18] = FCSCreate(context);
                content = DataSender(COM, buffer2);
                if (!CheckReturnFCS(content))
                {
                    throw new Exception("通讯错误!");
                }
                if (MemoryCompare(new byte[] { 0xbd, 4, 0, 0, 0, 0, 0xc1 }, content.ToArray()) != 0)
                {
                    throw new Exception("无效卡");
                }
                content.Clear();
                buffer2 = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0, 0 };
                buffer2[7] = (byte) (((num3 + 3) * 4) + 3);
                context = new byte[] { 0xbd, 6, 0xc9, 0, 0, 0, 0, 0 };
                context[7] = (byte) (((num3 + 3) * 4) + 3);
                buffer2[8] = FCSCreate(context);
                if (!CheckReturnFCS(DataSender(COM, buffer2)))
                {
                    throw new Exception("通讯错误!");
                }
            }
            return num2.ToString("D8");
        }

        public static int String2Byte(string _Byte)
        {
            try
            {
                return Convert.ToInt32(byte.Parse(_Byte, NumberStyles.AllowHexSpecifier));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public static byte[] String2ByteArray(string ConvertData)
        {
            string[] strArray = ConvertData.Split(new char[] { '-' });
            byte[] buffer = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                buffer[i] = byte.Parse(strArray[i], NumberStyles.AllowHexSpecifier);
            }
            return buffer;
        }
    }
}

