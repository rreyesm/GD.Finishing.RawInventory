using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Stock_Finishing.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }

        public override string ToString()
        {
            return $"{Name} {LastName}";
        }

        public int UserID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }

    }
}
