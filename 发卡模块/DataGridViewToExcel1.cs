namespace 发卡模块
{
    using Microsoft.Office.Interop.Excel;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class DataGridViewToExcel1
    {
        private Microsoft.Office.Interop.Excel.Application m_App = null;
        private Worksheet m_CurrentSheet = null;
        private int m_index = 0;
        private Workbooks m_Workbks = null;
        private Workbook m_WorkBook = null;

        public void Dispose()
        {
            if (this.m_WorkBook != null)
            {
                this.m_WorkBook.Close(false, Missing.Value, Missing.Value);
                this.m_WorkBook = null;
            }
            if (this.m_App != null)
            {
                this.m_App.Quit();
                Marshal.ReleaseComObject(this.m_App);
                this.m_App = null;
            }
            if (this.m_Workbks != null)
            {
                this.m_Workbks.Close();
                Marshal.ReleaseComObject(this.m_Workbks);
                this.m_Workbks = null;
            }
        }

        private void ExportToExcel(DataGridView gridView)
        {
            this.GetWorkSheet();
            this.InitHeder(gridView, 1);
            int num = 2;
            foreach (DataGridViewRow row in (IEnumerable) gridView.Rows)
            {
                int num2 = 1;
                for (int i = 1; i <= gridView.Columns.Count; i++)
                {
                    if (gridView.Columns[i - 1].Visible)
                    {
                        Range range = (Range) this.m_CurrentSheet.Cells[num, num2];
                        try
                        {
                            range.Value2 = row.Cells[i - 1].Value.ToString();
                        }
                        catch
                        {
                            range.Value2 = Convert.ToString(row.Cells[i - 1].Value.ToString());
                        }
                        range.Borders.Weight = XlBorderWeight.xlThin;
                        range.Borders.Color = Color.Black.ToArgb();
                        range.EntireColumn.AutoFit();
                        range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                        range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                        System.Windows.Forms.Application.DoEvents();
                        num2++;
                    }
                }
                num++;
                if (num == 0xffff)
                {
                    this.GetWorkSheet();
                    num = 1;
                }
            }
        }

        public void ExportToExcel(DataGridView[] gridViews, string strFileName)
        {
            if ((gridViews != null) && (gridViews.Length != 0))
            {
                this.m_App = new ApplicationClass();
                this.m_Workbks = this.m_App.Workbooks;
                this.m_WorkBook = this.m_Workbks.Add(Missing.Value);
                this.m_App.Visible = false;
                foreach (DataGridView view in gridViews)
                {
                    this.ExportToExcel(view);
                }
                ((Worksheet) this.m_WorkBook.Sheets[1]).Activate();
                this.m_WorkBook.SaveAs(strFileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                this.Dispose();
            }
        }

        ~DataGridViewToExcel1()
        {
            this.Dispose();
        }

        private void GetWorkSheet()
        {
            this.m_index++;
            if (this.m_index <= this.m_WorkBook.Worksheets.Count)
            {
                this.m_CurrentSheet = (Worksheet) this.m_WorkBook.Worksheets[this.m_index];
            }
            else
            {
                this.m_CurrentSheet = (Worksheet) this.m_WorkBook.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                this.m_CurrentSheet.Move(Missing.Value, this.m_WorkBook.Worksheets[this.m_WorkBook.Worksheets.Count]);
            }
        }

        private void InitHeder(DataGridView gridView, int row)
        {
            int num = 1;
            for (int i = 1; i <= gridView.ColumnCount; i++)
            {
                if (gridView.Columns[i - 1].Visible)
                {
                    Range range = (Range) this.m_CurrentSheet.Cells[row, num];
                    range.Value2 = gridView.Columns[i - 1].HeaderText;
                    range.Font.Bold = true;
                    num++;
                }
            }
        }
    }
}

