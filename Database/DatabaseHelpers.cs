using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCreator.Enumerators;
using TestCreator.ViewModel;

namespace TestCreator.Database
{
    public class DatabaseHelpers
    {
        public static void SaveTestToDb(TestViewModel testView)
        {
            var db = new MyDbContext();
            var test = testView.GetTest();
            db.Tests.AddOrUpdate(test);
            db.SaveChanges();
        }

        public static void SaveTestToDb(Test test)
        {
            var db = new MyDbContext();
            db.Tests.AddOrUpdate(test);
            db.SaveChanges();
        }

        public static void RemoveTestByID(int id)
        {
            var db = new MyDbContext();

            if (db.Tests.Any(x => x.TestID == id))
            {
                foreach (var arg in db.Tests.Where(x => x.TestID == id))
                {
                    db.Tests.Remove(arg);
                }
            }
            db.SaveChanges();
        }


        public static void SaveQuestionToDb(TestViewModel testView)
        {
            var db = new MyDbContext();
            foreach (var arg in testView.GetQuestions())
            {
                db.Questions.AddOrUpdate(arg);
            }
            db.SaveChanges();
        }

        public static List<Test> GetAllTest()
        {
            var db = new MyDbContext();
            return db.Tests.ToList();
        }

        public static Test GetTestByID(int id)
        {
            var db = new MyDbContext();
            return db.Tests.Single(x => x.TestID == id);
        }

        public static Question GetQuestionByID(int id)
        {
            var db = new MyDbContext();
            return db.Questions.Single(x => x.QuestionID == id);
        }

        public static Answer GetAnswerByID(int id)
        {
            var db = new MyDbContext();
            return db.Answers.Single(x => x.AnswerID == id);
        }

        public static List<User> GetAllUsers()
        {
            var db = new MyDbContext();
            return db.Users.ToList();
        }

        public static User GetUserByID(int id)
        {
            var db = new MyDbContext();
            return db.Users.Single(x => x.UserID == id);
        }

        public static void SaveUser(User user)
        {
            var db = new MyDbContext();
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
        }

        public static int GetAllUsersCount()
        {
            var db = new MyDbContext();
            return db.Users.Count();
        }

        public static int GetUsersCount()
        {
            var db = new MyDbContext();
            return db.Users.Count(x=>x.Role == (int)Role.User);
        }

        public static int GetAdminsCount()
        {
            var db = new MyDbContext();
            return db.Users.Count(x => x.Role == (int)Role.Admin);
        }
    }
}
