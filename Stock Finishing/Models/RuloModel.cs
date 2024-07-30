using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Models
{
    public partial class RuloModel : ObservableObject
    {
        public int RuloID { get; set; }
        public string Lote { get; set; }
        public int Beam { get; set; }
        public string BeamStop { get; set; }
        public int Loom { get; set; }
        public bool IsToyota { get; set; }
        public string Style { get; set; }
        public string StyleName { get; set; }
        public decimal Width { get; set; }
        public decimal WeavingLength { get; set; }

        public decimal EntranceLength { get; set; }
        public decimal ExitLength { get; set; }
        public decimal Shrinkage { get; set; }
        public int Shift { get; set; }
        public bool IsWaitingAnswerFromTest { get; set; }
        public int? TestResultID { get; set; }
        public int? TestResultAuthorizer { get; set; }
        public int MainOriginID { get; set; }
        public int OriginID { get; set; }
        public string Observations { get; set; }
        public int FolioNumber { get; set; }
        public DateTime? SentDate { get; set; }
        public int? SenderID { get; set; }
        public int? SentAuthorizerID { get; set; }
        public int PieceCount { get; set; }
        public int PeriodID { get; set; }
        public bool IsTestStyle { get; set; }
        public int? WarehouseCategoryID { get; set; }
        public bool IsFailFinishing { get; set; }
        public bool IsTestWeaving { get; set; }
        public int? PackingListNo { get; set; }
        public DateTime? AccountingDate { get; set; }
    }
}
