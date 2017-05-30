using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCreator.ViewModel;

namespace TestCreator.Database
{
    public class DatabaseHelpers
    {
        public static void SaveTestToDb(TestViewModel testView)
        {
            var db = new MyDbContext();
            db.Tests.AddOrUpdate(testView.GetTest());
            db.SaveChanges();
        }

        public static List<Test> GetAllTest()
        {
            var db = new MyDbContext();
            return db.Tests.ToList();
        }
    }
}
