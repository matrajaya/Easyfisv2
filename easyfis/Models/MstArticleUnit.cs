using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstArticleUnit
    {
         [Key]

        public Int32 Id { get; set; }
         public Int32 ArticleId { get; set; }
         public String Article { get; set; }
         public Int32 UnitId { get; set; }
         public String Unit { get; set; }
         public Decimal Multiplier { get; set; }
         public Boolean? IsCountUnit { get; set; }

    }
}