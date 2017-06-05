using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TestCreator.Enumerators;
using TestCreator.Helpers;

namespace TestCreator.Database
{
    public class User
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }

        [NotMapped]
        public BitmapImage AvatarBitmap
        {
            get { return BitmapConverter.ConvertBytesToBitmap(Avatar); }
        }

        public virtual ICollection<UserTest> Tests { get; set; }

    }
}
