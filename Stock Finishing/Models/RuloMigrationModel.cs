using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Models
{
    public class RuloMigrationModel
    {
        public int? WarehouseCategoryID { get; set; }
        public bool IsRejected { get; set; }
        public int RuloMigrationID { get; set; }
        public string Style { get; set; }
        public string StyleName { get; set; }
        public string Lote { get; set; }
        public int Beam { get; set; }
        public int Loom { get; set; }
        public DateTime CreationDate { get; set; }
        public string? BeamStop { get; set; }
        public int PieceNo { get; set; }
        public string? IsToyota { get; set; }
        public decimal SizingMeters { get; set; }
        public decimal Meters { get; set; }
        public string? Observations { get; set; }
        public int? PackingListID { get; set; }
        public int? SizingSequence { get; set; }
    }
}
