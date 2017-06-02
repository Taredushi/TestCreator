using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCreator.Database;
using TestCreator.Enumerators;

namespace TestCreator.ViewModel
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get { return Name + " " + Surname; } }
        public Role Role { get; set; }
        public string RoleName { get { return Enum.GetName(typeof(Role), Role); } }


        public void CreateSimpleModel(User user)
        {
            this.ID = user.UserID;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Role = (Role) user.Role;
        }
    }
}
