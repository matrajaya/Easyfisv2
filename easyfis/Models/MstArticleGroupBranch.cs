using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstArticleGroupBranch
    {
        public Int32 Id { get; set; }
        public Int32 ArticleGroupId { get; set; }
        public String ArticleGroup { get; set; }
        public Int32 CompanyId { get; set; }
        public String Company { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public Int32 AccountId { get; set; }
        public String AccountCode { get; set; }
        public String Account { get; set; }
        public Int32 SalesAccountId { get; set; }
        public String SalesAccountCode { get; set; }
        public String SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public String CostAccountCode { get; set; }
        public String CostAccount { get; set; }
        public Int32 AssetAccountId { get; set; }
        public String AssetAccountCode { get; set; }
        public String AssetAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public String ExpenseAccountCode { get; set; }
        public String ExpenseAccount { get; set; }
    }
}