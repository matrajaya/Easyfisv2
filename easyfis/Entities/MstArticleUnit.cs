using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstArticleUnit
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public Int32 UnitId { get; set; }
        public Decimal Multiplier { get; set; }
        public Boolean IsCountUnit { get; set; }
    }
}