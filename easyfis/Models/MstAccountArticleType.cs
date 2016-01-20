using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstAccountArticleType
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public String ArticleType { get; set; }
    }
}