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
        public Int32 BranchId { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 SalesAccountId { get; set; }
        public Int32 CostAccountId { get; set; }
        public Int32 AssetAccountId { get; set; }
        public Int32 ExpenseAccountId { get; set; }
    }
}