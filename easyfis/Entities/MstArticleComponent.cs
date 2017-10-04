using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstArticleComponent
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public Int32 ComponentArticleId { get; set; }
        public Decimal Quantity { get; set; }
        public String Particulars { get; set; }
    }
}