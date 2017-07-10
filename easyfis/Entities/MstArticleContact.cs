using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstArticleContact
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public List<Entities.MstArticle> ArticleList { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public String Remarks { get; set; }
    }
}