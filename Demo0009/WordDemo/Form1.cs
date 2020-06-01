using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Words;
using Aspose.Words.Tables;
using DataCrawl;
using OfficeOpenXml;
using Font = Aspose.Words.Font;

namespace WordDemo
{
    public partial class Form1 : Form
    {
        private string dir = @"E:\myjob\Demo\WordDemo\WordDemo\test\";
        private List<Record> dbList = new List<Record>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(dir + "\\s");
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                //var fileName=fileInfo.Name.Replace(fileInfo.Extension, "");

                try
                {
                    string fileName = dir + "\\s\\" + fileInfo.Name;
                    var wrdf = new Document(fileName);

                    //获取第一段的标题信息
                    var p = wrdf.GetChildNodes(NodeType.Paragraph, true)[0] as Paragraph;
                    var title = p.GetText().Replace("\r", "");

                    //生成一个要输出的文档
                    var doc = new Document();
                    var builder = new DocumentBuilder(doc);

                    //将读取的标题信息写入到输出文档
                    var font = builder.Font;
                    font.Size = 14;
                    font.Bold = true;
                    font.Color = Color.Black;
                    font.Name = "Arial";
                    var paragraphFormat = builder.ParagraphFormat;
                    paragraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Writeln(title);


                    var cWidths = new[] { 20, 40, 20, 120, 260, 220, 100, 40, 20, 10 };
                    Table table = builder.StartTable();

                    var sourceTable = wrdf.GetChildNodes(NodeType.Table, true)[0] as Table;
                    var rowLen = sourceTable.Rows.Count;
                    for (int i = 0; i < 10; i++)
                    {
                        var c = builder.InsertCell();
                        c.CellFormat.ClearFormatting();
                        c.CellFormat.Borders[0].LineStyle = LineStyle.Single;
                        c.CellFormat.Borders[1].LineStyle = LineStyle.Single;
                        c.CellFormat.Borders[2].LineStyle = LineStyle.Single;
                        c.CellFormat.Borders[3].LineStyle = LineStyle.Single;
                        c.CellFormat.FitText = false;

                        var cell = sourceTable.Rows[0].Cells[i];//获取第一行的数据
                        var txt = cell.GetText();
                        builder.Font.Size = 12;
                        builder.Font.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.CellFormat.Width = cWidths[i];
                        builder.Write(txt.Replace(" ", ""));

                    }
                    builder.EndRow();

                    var listHosts = new List<Person>();
                    var listAll = new List<Person>();
                    var hostIndex = 1;

                    for (var rIndex = 1; rIndex < rowLen; rIndex++)
                    {
                        var name = sourceTable.Rows[rIndex].Cells[1].GetText().Replace(" ", "").Replace("\a", "");
                        var hostName = sourceTable.Rows[rIndex].Cells[6].GetText().Replace(" ", "").Replace("\a", "");

                        if (name == hostName)
                        {
                            for (int cIndex = 0; cIndex < 10; cIndex++)
                            {
                                var c = builder.InsertCell();
                                c.CellFormat.ClearFormatting();
                                c.CellFormat.Borders[0].LineStyle = LineStyle.Single;
                                c.CellFormat.Borders[1].LineStyle = LineStyle.Single;
                                c.CellFormat.Borders[2].LineStyle = LineStyle.Single;
                                c.CellFormat.Borders[3].LineStyle = LineStyle.Single;
                                c.CellFormat.FitText = false;

                                var cell = sourceTable.Rows[rIndex].Cells[cIndex];
                                var txt = cell.GetText();
                                builder.Font.Size = 12;
                                builder.Font.Bold = false;
                                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                builder.CellFormat.Width = cWidths[cIndex];
                                builder.Write(txt.Replace(" ", ""));
                            }
                            builder.EndRow();
                        }

                        listAll.Add(new Person()
                        {
                            No = rIndex.ToString(),
                            Name = sourceTable.Rows[rIndex].Cells[1].GetText().Replace(" ", "").Replace("\a", ""),
                            Sex = sourceTable.Rows[rIndex].Cells[2].GetText().Replace(" ", "").Replace("\a", ""),
                            Birthday = sourceTable.Rows[rIndex].Cells[3].GetText().Replace(" ", "").Replace("\a", ""),
                            CardNo = sourceTable.Rows[rIndex].Cells[4].GetText().Replace(" ", "").Replace("\a", "").Trim('\a'),
                            Address = sourceTable.Rows[rIndex].Cells[5].GetText().Replace(" ", "").Replace("\a", ""),
                            HostName = sourceTable.Rows[rIndex].Cells[6].GetText().Replace(" ", "").Replace("\a", ""),
                            Relation = sourceTable.Rows[rIndex].Cells[7].GetText().Replace(" ", "").Replace("\a", ""),
                            Percent = sourceTable.Rows[rIndex].Cells[8].GetText().Replace(" ", "").Replace("\a", ""),
                            Remark = sourceTable.Rows[rIndex].Cells[9].GetText().Replace(" ", "").Replace("\a", "")
                        });
                    }

                    builder.EndTable();

                    doc.Save(dir + "\\o\\" + fileInfo.Name.Replace(fileInfo.Extension, "") + ".docx", SaveFormat.Docx);



                    var count = 1;
                    for (int i = 0; i < listAll.Count; i++)
                    {
                        if (listAll[i].CardNo.Length > 18)
                        {
                            listAll[i].CardNo = listAll[i].CardNo.Substring(0, 18);
                        }
                        if (listAll[i].Name == listAll[i].HostName)
                        {
                            int c = 0;
                            for (int j = i; j < listAll.Count; j++)
                            {
                                if (j != i && listAll[j].Name == listAll[j].HostName)
                                {
                                    break;
                                }
                                else
                                {
                                    c = c + 1;
                                }
                            }
                            listAll[i].total = c.ToString();
                            listAll[i].No = (count++).ToString();
                            listAll[i].MobilePhone = getPhone(listAll[i].HostName);
                            listHosts.Add(listAll[i]);
                        }
                    }

                    WriteListToExcel<Person>(new List<string>
                        {
                            "序号", "姓名", "性别", "出生日期", "公民身份证号码", "住址状态", "户主", "与户主关系", "持股份额", "备注", "计数","电话"
                        }, new List<string>
                        {
                            "No", "Name",
                            "Sex", "Birthday",
                            "CardNo", "Address",
                            "HostName", "Relation",
                            "Percent", "Remark", "total","MobilePhone"
                        }, listHosts, dir + "\\o\\persons_host.xlsx", fileInfo.Name.Replace(fileInfo.Extension, ""));
                    WriteListToExcel<Person>(new List<string>
                    {
                        "序号", "姓名", "性别", "出生日期", "公民身份证号码", "住址状态", "户主", "与户主关系", "持股份额", "备注"
                    }, new List<string>
                    {
                        "No", "Name",
                        "Sex", "Birthday",
                        "CardNo", "Address",
                        "HostName", "Relation",
                        "Percent", "Remark"
                    }, listAll, dir + "\\o\\persons_all.xlsx", fileInfo.Name.Replace(fileInfo.Extension, ""));
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }


        /// <summary>
        /// 将列表数据导出到excel中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="titles"></param>
        /// <param name="properties"></param>
        /// <param name="list"></param>
        /// <param name="fullFilePath"></param>
        /// <param name="sheetName"></param>
        private void WriteListToExcel<T>(IEnumerable<string> titles, IEnumerable<string> properties,
            IEnumerable<T> list, string fullFilePath, string sheetName)
        {
            var file = new FileInfo(fullFilePath);
            var tempTitles = titles.ToList();
            var tempProperties = properties.ToList();
            using (var package = new ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);

                var i = 1;
                foreach (var title in tempTitles)
                {
                    worksheet.Cells[1, i++].Value = title;
                }

                Type t = typeof(T);
                var rowCount = 2;
                foreach (var ta in list)
                {
                    var columnCount = 1;
                    foreach (var p in tempProperties)
                    {
                        if (p == "total")
                        {
                            worksheet.Cells[rowCount, columnCount++].Value = Convert.ToInt32(t.GetProperty(p)?.GetValue(ta));
                        }
                        else
                        {
                            worksheet.Cells[rowCount, columnCount++].Value = t.GetProperty(p)?.GetValue(ta);
                        }
                    }

                    rowCount = rowCount + 1;
                }

                package.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //using (var db = new testEntities())
            //{
            //    var list = (from u in db.RecorderInfo
            //                select new
            //                {
            //                    u.HzUserName,
            //                    u.HzPhone
            //                }).ToList();
            //}

            //int a = 5;
            //var r = string.Format("{0:0000}", a);

            var str = "06马山头小组，共 13 户，共 57人-删减后";
            var x = str.Split('，')[0].Substring(2).Replace("小组","");
        }

        private string getPhone(string username)
        {
            var l = (from x in dbList
                     where x.HzUserName.Equals(username)
                     select x).ToList();
            if (l.Count > 0)
            {
                return l[0].HzPhone;
            }
            else
            {
                return string.Empty;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var db = new testEntities())
            {
                var list = (from u in db.RecorderInfo
                            select new
                            {
                                u.HzUserName,
                                u.HzPhone
                            }).ToList();
                foreach (var r in list)
                {
                    dbList.Add(new Record()
                    {
                        HzUserName = r.HzUserName,
                        HzPhone = r.HzPhone
                    });
                }
            }
        }

        private void GetSourceData()
        {
            var list = new List<Info>();
            var sourceFilePath = @"E:\myjob\Demo\WordDemo\WordDemo\test\d.xlsx";

            var file = new FileInfo(sourceFilePath);
            using (var package = new ExcelPackage(file))
            {

                var sheetCount = package.Workbook.Worksheets.Count;
                for (int sheet = 1; sheet <= sheetCount; sheet++)
                {
                    list.Clear();
                    var worksheet = package.Workbook.Worksheets[sheet];
                    var workSheetName = worksheet.Name;
                    var rowCount = worksheet.Dimension.Rows;
                    for (var row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 1].Value == null)
                        {
                            break;
                        }

                        var sourceInfo = new Info()
                        {
                            No = (row - 1).ToString(),
                            Name = GetCellValue(worksheet.Cells, row, 2),
                            GuNo = "350782100209" + $"{sheet:00}" + $"{(row - 1):0000}",
                            Count = GetCellValue(worksheet.Cells, row, 3),
                        };
                        list.Add(sourceInfo);
                    }

                    WriteToWord(list,workSheetName);

                }

            }
        }

        private string GetCellValue(ExcelRange cells, int row, int column)
        {
            if (cells[row, column] != null && cells[row, column].Value != null)
            {
                var val = cells[row, column].Value.ToString().Trim().Replace(" ", "");
                return val;
            }

            return string.Empty;
        }

        private void WriteToWord(List<Info> list, string docName)
        {
            //var doc = new Document();
            //var builder = new DocumentBuilder(doc);

            //WriteCell(builder, "序号");
            //WriteCell(builder, "股权户代表\r\n姓  名",90);
            //WriteCell(builder, "股权证号", 80);
            //WriteCell(builder, "户持股数",80);
            //WriteCell(builder, "领证人\r\n签字",70);
            //WriteCell(builder, "领证\r\n时间",70);
            //builder.EndRow();

            //foreach (var item in list)
            //{
            //    WriteCell(builder,  item.No);
            //    WriteCell(builder,  item.Name,90);
            //    WriteCell(builder,  item.GuNo,80);
            //    WriteCell(builder,  item.Count,80);
            //    WriteCell(builder, string.Empty,70);
            //    WriteCell(builder, string.Empty,70);
            //    builder.EndRow();
            //}

            //builder.EndTable();
            //doc.Save(dir + "\\p\\" + docName + ".docx", SaveFormat.Docx);

            

            var doc = new Document(dir + "t.docx");
            String[] fieldNames = new String[] {"countryName"};
            Object[] fieldValues = new Object[] {docName.Split('，')[0].Substring(2).Replace("小组", "")};
            //合并模版，相当于页面的渲染
            doc.MailMerge.Execute(fieldNames, fieldValues);

            var outputFile = dir + "\\p\\" + docName + ".docx";
            doc.Save(outputFile, SaveFormat.Docx);

            DataTable table = new DataTable("UserList");
            table.Columns.Add("No");
            table.Columns.Add("Name");
            table.Columns.Add("GuNo");
            table.Columns.Add("Count");
            DataRow dr;
            foreach (var info in list)
            {
                dr = table.NewRow();
                dr[0] = info.No;
                dr[1] = info.Name;
                dr[2] = info.GuNo;
                dr[3] = info.Count;
                table.Rows.Add(dr);
            }
            var doc2 = new Document(outputFile);
            doc2.MailMerge.ExecuteWithRegions(table);

            doc2.Save(outputFile, SaveFormat.Docx);

        }

        private static void WriteCell(DocumentBuilder builder,  string val,int width=20)
        {
            var c = builder.InsertCell();
            c.CellFormat.ClearFormatting();
            c.CellFormat.Borders[0].LineStyle = LineStyle.Single;
            c.CellFormat.Borders[1].LineStyle = LineStyle.Single;
            c.CellFormat.Borders[2].LineStyle = LineStyle.Single;
            c.CellFormat.Borders[3].LineStyle = LineStyle.Single;
            c.CellFormat.FitText = false;

            builder.Font.Size = 12;
            builder.Font.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.Width = width;
            builder.Write(val);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetSourceData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fileName = dir + "t.docx";
            var doc = new Document(fileName);

            String[] fieldNames = new String[] { "countryName" };
            Object[] fieldValues = new Object[] { "张三" };
            //合并模版，相当于页面的渲染
            doc.MailMerge.Execute(fieldNames, fieldValues);

            doc.Save(dir + "ta.docx", SaveFormat.Docx);

            var fName = dir + "ta.docx";
            DataTable table = new DataTable("UserList");
            table.Columns.Add("UserName");
            var r=table.NewRow();
            r[0] = "张三aa";
            table.Rows.Add(r);
             r = table.NewRow();
            r[0] = "李四";
            table.Rows.Add(r);

            var doc2=new Document(fName);
            doc2.MailMerge.ExecuteWithRegions(table);

            doc2.Save(dir + "taa.docx", SaveFormat.Docx);

        }
    }
}
