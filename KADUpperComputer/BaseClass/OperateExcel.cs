namespace KADUpperComputer.BaseClass
{
    using Microsoft.Office.Interop.Excel;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    internal class OperateExcel
    {
        public bool DSToExcel(string Path, OleDbDataReader sqlread)
        {
            string str = "";
            try
            {
                Microsoft.Office.Interop.Excel.Application application = new ApplicationClass();
                if (application == null)
                {
                    return false;
                }
                application.Application.Workbooks.Add(true);
                int num = 2;
                int fieldCount = sqlread.FieldCount;
                int ordinal = 0;
                while (ordinal < fieldCount)
                {
                    application.Cells[1, ordinal + 1] = sqlread.GetName(ordinal);
                    ordinal++;
                }
                while (sqlread.Read())
                {
                    for (ordinal = 0; ordinal < fieldCount; ordinal++)
                    {
                        if (ordinal == 12)
                        {
                            try
                            {
                                object obj2 = sqlread.GetValue(ordinal);
                                if (Convert.ToString(obj2) != "")
                                {
                                    byte[] inArray = (byte[]) obj2;
                                    str = Convert.ToBase64String(inArray);
                                    int length = str.Length;
                                }
                                else
                                {
                                    str = "no image";
                                }
                            }
                            catch
                            {
                                str = "no image";
                            }
                        }
                        else
                        {
                            str = sqlread.GetValue(ordinal).ToString();
                        }
                        if ((((ordinal == 3) || (ordinal == 5)) || (ordinal == 6)) || (ordinal == 12))
                        {
                            application.Cells[num, ordinal + 1] = "'" + str;
                        }
                        else
                        {
                            application.Cells[num, ordinal + 1] = str;
                        }
                    }
                    num++;
                }
                application.Visible = true;
                application.Quit();
                application = null;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public DataSet ExcelToDS(string Path)
        {
            try
            {
                string str;
                if (Path.ToLower().Substring(Path.Length - 3, 3) == "xls")
                {
                    str = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";
                }
                else
                {
                    str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + Path + "';Extended Properties='Excel 12.0;HDR=NO;IMEX=1'";
                }
                OleDbConnection selectConnection = new OleDbConnection(str);
                selectConnection.Open();
                object[] restrictions = new object[4];
                restrictions[3] = "Table";
                System.Data.DataTable oleDbSchemaTable = selectConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
                string[] strArray = new string[oleDbSchemaTable.Rows.Count];
                for (int i = 0; i < oleDbSchemaTable.Rows.Count; i++)
                {
                    strArray[i] = oleDbSchemaTable.Rows[i]["TABLE_NAME"].ToString();
                }
                DataSet dataSet = new DataSet();
                new OleDbDataAdapter("select * from[" + strArray[0] + "]", selectConnection).Fill(dataSet);
                return dataSet;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
                Console.WriteLine(exception.ToString());
                return null;
            }
        }

        public void ExportToExcel(string fileName, DataGridView _DataGridView, string _WorkNamespace)
        {
            bool flag = File.Exists(fileName);
            object updateLinks = Missing.Value;
            ApplicationClass class2 = null;
            Workbook workbook = null;
            Worksheet worksheet = null;
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                class2 = new ApplicationClass();
                if (flag)
                {
                    workbook = class2.Workbooks.Open(fileName, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
                    worksheet = (Worksheet) workbook.Worksheets.Add(updateLinks, updateLinks, 1, updateLinks);
                    worksheet.Name = _WorkNamespace + workbook.Sheets.Count;
                }
                else
                {
                    workbook = class2.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    worksheet = (Worksheet) workbook.Sheets[1];
                    worksheet.Name = _WorkNamespace;
                }
                int num = 0;
                int num2 = 0;
                int count = _DataGridView.Columns.Count;
                _DataGridView.SuspendLayout();
                foreach (DataGridViewColumn column in _DataGridView.Columns)
                {
                    if (column.Index != 0)
                    {
                        Range range = worksheet.get_Range(worksheet.Cells[num + 1, ++num2], worksheet.Cells[num + 1, num2]);
                        range.Columns.ColumnWidth = column.Width / 10;
                        range.Interior.ColorIndex = 0x10;
                        range.Interior.Pattern = Constants.xlBoth;
                        range.Font.ColorIndex = 2;
                        range.HorizontalAlignment = Constants.xlCenter;
                        range.VerticalAlignment = Constants.xlCenter;
                        range.set_Item(1, 1, column.HeaderText);
                    }
                }
                num++;
                foreach (DataGridViewRow row in (IEnumerable) _DataGridView.Rows)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (i != 0)
                        {
                            worksheet.get_Range(worksheet.Cells[num + 1, i], worksheet.Cells[num + 1, i + 1]).set_Item(1, 1, row.Cells[i].Value);
                        }
                    }
                    num++;
                }
                worksheet.UsedRange.Borders[XlBordersIndex.xlEdgeTop].Weight = 3;
                worksheet.UsedRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = 3;
                worksheet.UsedRange.Borders[XlBordersIndex.xlEdgeLeft].Weight = 3;
                worksheet.UsedRange.Borders[XlBordersIndex.xlEdgeRight].Weight = 3;
                worksheet.UsedRange.Borders[XlBordersIndex.xlInsideHorizontal].Weight = 2;
                worksheet.UsedRange.Borders[XlBordersIndex.xlInsideVertical].Weight = 2;
                workbook.Saved = true;
                if (flag)
                {
                    workbook.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, updateLinks, updateLinks, updateLinks, updateLinks, XlSaveAsAccessMode.xlExclusive, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
                }
                else
                {
                    workbook.SaveCopyAs(fileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                _DataGridView.ResumeLayout();
                workbook.Close(false, null, null);
                class2.Quit();
                Cursor.Current = current;
            }
        }

        public void OpenFile(string fileName)
        {
            if (MessageBox.Show("你想打开这个文件吗?", "导出到...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    new Process { StartInfo = { FileName = fileName, Verb = "Open", WindowStyle = ProcessWindowStyle.Normal } }.Start();
                }
                catch
                {
                    MessageBox.Show("你的计算机中未安装Excel,不能打开该文档!", "错误!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }
    }
}

