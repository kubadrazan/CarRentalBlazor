using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class ApiEntity
    {
        public int ID { get; set; }

        public int ApiID { get; set; }

        public string URL { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }
    }
}
