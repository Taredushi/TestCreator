using System.Collections.Generic;
using System.Security.Policy;
using TestCreator.Database;
using TestCreator.Enumerators;

namespace TestCreator.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TestCreator.Database.MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestCreator.Database.MyDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var admin = new User()
            {
                Email = "admin@gmail.com",
                Login = "admin",
                Name = "Admin",
                Password = Encryptor.MD5Hash("admin"),
                Role = (int) Enums.Role.Admin,
                Surname = "Root",
                UserID = 1
            };

            var user = new User()
            {
                Email = "user@gmail.com",
                Login = "user",
                Name = "User",
                Password = Encryptor.MD5Hash("user"),
                Role = (int)Enums.Role.User,
                Surname = "Normal",
                UserID = 2
            };

            context.Users.AddOrUpdate(admin);
            context.Users.AddOrUpdate(user);
            context.SaveChanges();
            CreateTests(context);
        }

        private void CreateTests(MyDbContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                var test = new Test()
                {
                    Name = "Test" + i,
                    TestID = i,
                };

                List<Question> questions = new List<Question>();
                for (int j = 1; j <= 5; j++)
                {
                    var question = new Question()
                    {
                        Text = "Pytanie "+ j,
                    };

                    List<Answer> answers = new List<Answer>();
                    for (int k = 1; k <= 4; k++)
                    {
                        var answer = new Answer()
                        {
                            IsCorrect = k%2 == 0,
                            Text = "Odpowiedz " + k
                        };
                        answers.Add(answer);
                    }
                    question.Answers = answers;
                    questions.Add(question);
                }
                test.Questions = questions;
                context.Tests.AddOrUpdate(test);
            }
            context.SaveChanges();
        }
    }
}
