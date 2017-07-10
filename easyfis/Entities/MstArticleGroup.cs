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
        public String ArticleType { get; set; }
        public List<Entities.MstArticleType> ArticleTypeList { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public List<Entities.MstAccount> AccountList { get; set; }
        public Int32 SalesAccountId { get; set; }
        public String SalesAccount { get; set; }
        public List<Entities.MstAccount> SalesAccountList { get; set; }
        public Int32 CostAccountId { get; set; }
        public String CostAccount { get; set; }
        public List<Entities.MstAccount> CostAccountList { get; set; }
        public Int32 AssetAccountId { get; set; }
        public String AssetAccount { get; set; }
        public List<Entities.MstAccount> AssetAccountList { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public String ExpenseAccount { get; set; }
        public List<Entities.MstAccount> ExpenseAccountList { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public List<Entities.MstUser> CreatedByUserList { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public List<Entities.MstUser> UpdatedByUserList { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}