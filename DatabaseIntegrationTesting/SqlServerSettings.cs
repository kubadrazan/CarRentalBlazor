using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatabaseIntegrationTesting
{
    public static class SqlServerSettings
    {
        public const string Username = "NewSA";
        public const string Password = "Password123123!";
        public const string Host = "localhost";
        public const string ConnectionString = "Password=Password123123!;Persist Security Info=True;User ID=NewSA;Initial Catalog=master;Data Source=localhost;Integrated Security = True; Encrypt=True;Trust Server Certificate=True";
    }
}
