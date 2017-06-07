using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TestCreator.Database;

namespace TestCreator.Helpers
{
    public class XmlGenerator
    {
        public static TestXml ConvertTestToTestXml(Test test)
        {
            var xml = new TestXml
            {
                ID = test.TestID,
                Name = test.Name,
                QuestionsLimit = test.QuestionsLimit,
                Questions = GetTestQuestionArray(test.Questions.ToList())
            };
            return xml;
        }

        public static void SaveXml(TestXml xml, string path)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(TestXml));
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, xml);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(path + ".xml");
                    stream.Close();
                }
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
        }

        private static TestQuestion[] GetTestQuestionArray(List<Question> collection)
        {
            TestQuestion[] array = new TestQuestion[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                array[i] = new TestQuestion
                {
                    Text = collection[i].Text,
                    ID = collection[i].TestID,
                    Answers = GetTestQuestionAnswerArray(collection[i].Answers.ToList())
                };
            }
            return array;
        }

        private static TestQuestionAnswer[] GetTestQuestionAnswerArray(List<Answer> collection)
        {
            TestQuestionAnswer[] array = new TestQuestionAnswer[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                array[i] = new TestQuestionAnswer()
                {
                    Text = collection[i].Text,
                    ID = collection[i].AnswerID,
                    IsCorrect = collection[i].IsCorrect ? 1 : 0
                };
            }
            return array;
        }

        public static Test ImportXml(string path)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(TestXml));
                using (XmlReader reader = XmlReader.Create(path))
                {
                    TestXml xml = (TestXml)ser.Deserialize(reader);
                    return ConvertTestXmlToTest(xml);
                }
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
        }

        private static Test ConvertTestXmlToTest(TestXml xml)
        {
            var test = new Test();
            test.Name = xml.Name;
            test.TestID = xml.ID;
            test.QuestionsLimit = xml.QuestionsLimit;
            test.Questions = GetQuestionList(xml.Questions);
            return test;
        }

        private static List<Question> GetQuestionList(TestQuestion[] collection)
        {
            List<Question> list = new List<Question>();
            foreach (var item in collection)
            {
                var question = new Question()
                {
                    Text = item.Text,
                    QuestionID = item.ID,
                    Answers = GetTestAnswerList(item.Answers)
                };
                list.Add(question);
            }
            return list;
        }

        private static List<Answer> GetTestAnswerList(TestQuestionAnswer[] collection)
        {
            List<Answer> list = new List<Answer>();
            foreach (var item in collection)
            {
                var answer = new Answer()
                {
                    Text = item.Text,
                    AnswerID = item.ID,
                    IsCorrect = item.IsCorrect == 1
                };
                list.Add(answer);
            }
            return list;
        }
    }
}
