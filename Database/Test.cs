using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.Database
{
    public class Test
    {
        public int TestID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserTest> Tests { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
