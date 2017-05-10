using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.Database
{
    public class UserTest
    {
        public int UserTestID { get; set; }
        public int Result { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int TestID { get; set; }
        public Test Test { get; set; }

        [NotMapped]
        public int MaxPoints { get; set; }
    }
}
