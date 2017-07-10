using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstUserForm
    {
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public String UserFullName { get; set; }
        public List<Entities.MstUser> UserList { get; set; }
        public Int32 FormId { get; set; }
        public String Form { get; set; }
        public List<Entities.SysForm> FormList { get; set; }
        public Boolean CanAdd { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }
        public Boolean CanLock { get; set; }
        public Boolean CanUnlock { get; set; }
        public Boolean CanPrint { get; set; }
    }
}