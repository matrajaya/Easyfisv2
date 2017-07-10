using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstAccountArticleType
    {
        public Int32 Id { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public List<Entities.MstAccount> AccountList { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public String ArticleType { get; set; }
        public List<Entities.MstAccountType> ArticleTypeList { get; set; }
    }
}