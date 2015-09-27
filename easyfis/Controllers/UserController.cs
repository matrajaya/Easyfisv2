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
                    string fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
                    string address = string.Concat(new string[] { user.Address });
                    string email = string.Concat(new string[] { user.Email });
                    string userName = string.Concat(new string[] { user.UserName });

                    ViewData.Add("FullName", fullName);
                    ViewData.Add("Address", address);
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
