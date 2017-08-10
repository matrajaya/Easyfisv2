using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstArticle
    {
        [Key]
        public Int32 Id { get; set; }
        public String ArticleCode { get; set; }
        public String ManualArticleCode { get; set; }
        public String Article { get; set; }
        public String Category { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public String ArticleType { get; set; }
        public Int32 ArticleGroupId { get; set; }
        public String ArticleGroup { get; set; }
        public Int32 AccountId { get; set; }
        public String AccountCode { get; set; }
        public String Account { get; set; }
        public Int32 SalesAccountId { get; set; }
        public String SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public String CostAccount { get; set; }
        public Int32 AssetAccountId { get; set; }
        public String AssetAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public String ExpenseAccount { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Int32 OutputTaxId { get; set; }
        public String OutputTax { get; set; }
        public Int32 InputTaxId { get; set; }
        public String InputTax { get; set; }
        public Int32 WTaxTypeId { get; set; }
        public String WTaxType { get; set; }
        public Decimal Price { get; set; }
        public Decimal? Cost { get; set; }
        public Boolean IsInventory { get; set; }
        public String Particulars { get; set; }
        public String Address { get; set; }
        public Int32 TermId { get; set; }
        public String Term { get; set; }
        public String ContactNumber { get; set; }
        public String ContactPerson { get; set; }
        public String EmailAddress { get; set; }
        public String TaxNumber { get; set; }
        public Decimal CreditLimit { get; set; }
        public String DateAcquired { get; set; }
        public Decimal UsefulLife { get; set; }
        public Decimal SalvageValue { get; set; }
        public String ManualArticleOldCode { get; set; }
        public Int32 Kitting { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
        public Decimal? LastPurchasePrice { get; set; }
    }
}