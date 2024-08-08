using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Models
{
    public class FabricInformationDTO
    {
        public int ID { get; set; }
        public string Lote { get; set; }
        public int Beam { get; set; }
        public int Loom { get; set; }
        public string Style { get; set; }
        public string StyleName { get; set; }
        public decimal Meters { get; set; }
        public string Type { get; set; }
    }
}
