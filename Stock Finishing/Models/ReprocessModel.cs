using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Models
{
    public partial class ReprocessModel : ObservableObject
    {
        public int ReprocessID { get; set; }
        public string Style { get; set; }
        public string StyleName { get; set; }
        public string Lote { get; set; }
        public int Beam { get; set; }
        public int Loom { get; set; }
        public decimal Meters { get; set; }
        public int MainOriginID { get; set; }
        public int OriginID { get; set; }
        public int OriginRuloID { get; set; }
        public int? DestinationRuloID { get; set; }
    }
}
