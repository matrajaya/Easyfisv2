using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstArticleGroup
    {
        public Int32 Id { get; set; }
        public String ArticleGroup { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 SalesAccountId { get; set; }
        public Int32 CostAccountId { get; set; }
        public Int32 AssetAccountId { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}