using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using Spire.Doc;
using Spire.Doc.Documents;
using TestCreator.Database;
using Document = Spire.Doc.Document;
using Paragraph = Spire.Doc.Documents.Paragraph;

namespace TestCreator.Export
{
    public class SaveDoc
    {
        public void SaveTestToWord(Test test, string path)
        {
            try
            {
                var list = DrawQuestions(test.Questions.ToList(), test.QuestionsLimit);
                Document document = new Document();
                Spire.Doc.Section section = document.AddSection();

                ListStyle listStyle = new ListStyle(document, ListType.Numbered);
                listStyle.Name = "levelstyle";
                listStyle.Levels[0].PatternType = ListPatternType.Arabic;
                listStyle.Levels[1].PatternType = ListPatternType.LowLetter;
                document.ListStyles.Add(listStyle);

                ParagraphStyle style = new ParagraphStyle(document);
                style.Name = "FontStyle";
                style.CharacterFormat.Bold = true;
                document.Styles.Add(style);


                Paragraph paragraph = section.AddParagraph();
                paragraph.AppendText(test.Name);
                paragraph.Format.HorizontalAlignment = HorizontalAlignment.Center;
                paragraph = section.AddParagraph();

                foreach (var question in list)
                {
                    Paragraph para = section.AddParagraph();
                    para.AppendText(question.Text);
                    para.ApplyStyle(style.Name);
                    para.ListFormat.ApplyStyle("levelstyle");

                    foreach (var answer in question.Answers)
                    {
                        para = section.AddParagraph();
                        para.AppendText(answer.Text);
                        para.ListFormat.ListLevelNumber = 1;
                        para.ListFormat.ApplyStyle("levelstyle");
                    }
                    para = section.AddParagraph();
                }

                document.SaveToFile(path + ".docx", FileFormat.Docx);
                RemoveEvaluationText(path + ".docx");

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveTestToPdf(Test test, string path)
        {
            string newPath = path + "_tmp";
            KillWordProcesses();
            SaveTestToWord(test, newPath);
            KillWordProcesses();
            SaveDocxAsPdf(newPath + ".docx");
            File.Delete(newPath + ".docx");
        }

        private void RemoveEvaluationText(string path)
        {
            object wordOMissing = Missing.Value;

            Application wordApp = new Application { Visible = false };
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(path, ReadOnly: false, Visible: false);

            var bufor = "Evaluation Warning: The document was created with Spire.Doc for .NET.";
            doc.Content.Find.Execute(bufor, false, true, false, false, false, true, 1, false,
                "", 2, false, false, false, false);

            doc.Close(WdSaveOptions.wdSaveChanges, wordOMissing, wordOMissing);
            wordApp.Quit(ref wordOMissing, ref wordOMissing, ref wordOMissing);
        }

        private void SaveDocxAsPdf(string path)
        {
            object wordOMissing = Missing.Value;

            Application wordApp = new Application { Visible = false };
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(path, ReadOnly: true, Visible: false);
            string pdfPath = path.Remove(path.LastIndexOf('.')) + ".pdf";
            object target = pdfPath.Replace("_tmp", "");
            object format = WdSaveFormat.wdFormatPDF;

            doc.SaveAs2(ref target, ref format, ref wordOMissing, ref wordOMissing, ref wordOMissing, 
                ref wordOMissing, ref wordOMissing, ref wordOMissing, ref wordOMissing, ref wordOMissing,
                ref wordOMissing, ref wordOMissing, ref wordOMissing, ref wordOMissing, ref wordOMissing, 
                ref wordOMissing);

            doc.Close(WdSaveOptions.wdDoNotSaveChanges, wordOMissing, wordOMissing);
            wordApp.Quit(wordOMissing, wordOMissing, wordOMissing);
        }

        private List<Question> DrawQuestions(List<Database.Question> collection, int limit)
        {
            Random random = new Random();

            List<Question> list = new List<Question>();
            List<int> usedIndexes = new List<int>();

            for (int i = 0; i < limit; i++)
            {
                int number;
                do
                {
                    number = random.Next(0, collection.Count);

                } while (usedIndexes.Contains(number));
                usedIndexes.Add(number);
                list.Add(collection[number]);
            }
            return list;
        }

        private void KillWordProcesses()
        {
            foreach (var arg in Process.GetProcessesByName("WINWORD"))
            {
                arg.Kill();
            }
        }
    }
}
