using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstArticlePrice
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public String PriceDescription { get; set; }
        public Decimal Price { get; set; }
        public String Remarks { get; set; }
    }
}