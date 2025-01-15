using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Requests.ApiB
{
    public class AskPrice
    {
        public int car_Id { get; set; }

        public int driversLicenceDuration { get; set; }

        public int age { get; set; }

        public string start { get; set; }

        public string Return { get; set; }

        public string extraInfo { get; set; }
    }
}
