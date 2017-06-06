using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemBox.Document;
using TestCreator.Database;

namespace TestCreator.Export
{
    public class SaveDoc
    {
        public void SaveTest(Test test, string path)
        {
            try
            {
                var list = DrawQuestions(test.Questions.ToList(), test.QuestionsLimit);
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");

                DocumentModel document = new DocumentModel();

                //document.Sections.Add(
                //    new Section(document,
                //        new Paragraph(document,
                //            new Run(document, test.Name))));

                ListStyle numberList = new ListStyle(ListTemplateType.NumberWithDot);
                Section section = new Section(document);
                document.Sections.Add(section);

                foreach (var question in list)
                {
                    Paragraph para = new Paragraph(document, question.Text);
                    para.ListFormat.Style = numberList;
                    para.ParagraphFormat.NoSpaceBetweenParagraphsOfSameStyle = true;
                    section.Blocks.Add(para);

                    foreach (var answer in question.Answers)
                    {
                        Paragraph para2a = new Paragraph(document, answer.Text);
                        para2a.ListFormat.Style = numberList;
                        para2a.ListFormat.ListLevelNumber++;
                        para2a.ParagraphFormat.NoSpaceBetweenParagraphsOfSameStyle = true;
                        section.Blocks.Add(para2a);
                    }
                }

                document.Save(@"d:\polibuda\infa_semestr VI\SW\test1.docx");

            }
            catch (Exception e)
            {
                
            }
        }

        private List<Question> DrawQuestions(List<Question> collection, int limit)
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
    }
}
