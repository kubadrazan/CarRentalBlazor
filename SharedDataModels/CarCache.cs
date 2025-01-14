using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class CarCache
    {
        public int CarID { get; set; }

        public string BrandName { get; set; }

        public string ModelName { get; set; }

        public int ProductionYear { get; set; }

        public int SourceApiID { get; set; }
    }
}
