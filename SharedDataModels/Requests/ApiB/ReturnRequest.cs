using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Requests.ApiB
{
    public class ReturnRequest
    {
        public int rent_Id { get; set; }

        public string client_Id {  get; set; }

        public string platform { get; set; }

        public string email { get; set; }

        public ReturnRequest() { }

        public ReturnRequest(int rent_Id, string client_Id, string platform, string email)
        {
            this.rent_Id = rent_Id;
            this.client_Id = client_Id;
            this.platform = platform;
            this.email = email;
        }
    }
}
