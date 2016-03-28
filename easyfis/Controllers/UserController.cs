using System.Linq;
using System.Web.Mvc;
using easyfis.Models;

namespace easyfis.Controllers
{
    public class UserAccountController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == username);
                    string fullName = string.Concat(new string[] { user.FullName });
                    string id = string.Concat(new string[] { user.Id });
                    string email = string.Concat(new string[] { user.Email });
                    string userName = string.Concat(new string[] { user.UserName });

                    ViewData.Add("UserId", id);
                    ViewData.Add("FullName", fullName);
                    ViewData.Add("Email", email);
                    ViewData.Add("UserName", userName);
                }
            }
            base.OnActionExecuted(filterContext);
        }
        public UserAccountController()
        { }
    }
}
