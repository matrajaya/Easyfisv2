using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnCollectionLine
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 ORId { get; set; }
        public String OR { get; set; }
        public String ORDate { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public Int32? SIId { get; set; }
        public String SI { get; set; }
        public String Particulars { get; set; }
        public Decimal Amount { get; set; }
        public Int32 PayTypeId { get; set; }
        public String PayType { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public Int32? DepositoryBankId { get; set; }
        public String DepositoryBank { get; set; }
        public Boolean IsClear { get; set; }
    }

}