using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnCollectionLine
    {
        public Int32 Id { get; set; }
        public Int32 ORId { get; set; }
        public Int32 BranchId { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 ArticleId { get; set; }
        public Int32 SIId { get; set; }
        public String Particulars { get; set; }
        public Decimal Amount { get; set; }
        public Int32 PayTypeId { get; set; }
        public String CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public String CheckBank { get; set; }
        public Int32 DepositoryBankId { get; set; }
        public Boolean IsClear { get; set; }
    }
}