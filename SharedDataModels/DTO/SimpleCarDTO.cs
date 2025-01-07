using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.DTO
{
    public class SimpleCarDTO
    {
        public int ID { get; set; }

        public string ModelName { get; set; }

        public string BrandName { get; set; }

        public int ProductionYear { get; set; }
    }
}
