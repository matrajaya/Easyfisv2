using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstArticleComponent
    {

        [Key]

        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public Int32 ComponentArticleId { get; set; }
        public String ComponentArticle { get; set; }
        public Decimal Quantity { get; set; }
        public String Particulars { get; set; }
   }
}