using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.Database
{
    public class MyDbContext : DbContext
    {
        // Your context has been configured to use a 'DB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DB' 
        // connection string in the application configuration file.
        public MyDbContext()
            : base("wpfProjekt")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTest> UserTests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.Configuration.LazyLoadingEnabled = false;
        }

    }
}
