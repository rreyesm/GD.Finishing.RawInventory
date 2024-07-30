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
        public int OriginRuloID { get; set; }
        public string Style { get; set; }
        public string StyleName { get; set; }
        public string Lote { get; set; }
        public int Beam { get; set; }
        public int Loom { get; set; }
        public string? PieceID { get; set; }
        public decimal Meters { get; set; }
        public string? DefinitionProcess { get; set; }
        public int DefinitionProcessID { get; set; }
        //Rulos
        public string MainOrigin { get; set; }
        public int MainOriginID { get; set; }
        public string Origin { get; set; }
        public int OriginID { get; set; }
        //Reprocess
        public string RepMainOrigin { get; set; }
        public int RepMainOriginID { get; set; }
        public string RepOrigin { get; set; }
        public int RepOriginID { get; set; }
        public int? PackingListNo { get; set; }
        public string RollObs { get; set; }
    }
}
