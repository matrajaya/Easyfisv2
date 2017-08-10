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
                    string userId = string.Concat(new string[] { user.Id });
                    string email = string.Concat(new string[] { user.Email });
                    string userName = string.Concat(new string[] { user.UserName });

                    Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
                    string branchId = string.Concat((from d in db.MstUsers where d.UserId == userId select d.BranchId).SingleOrDefault());
                    string branch = string.Concat((from d in db.MstUsers where d.UserId == userId select d.MstBranch.Branch).SingleOrDefault());
                    string company = string.Concat((from d in db.MstUsers where d.UserId == userId select d.MstBranch.MstCompany.Company).SingleOrDefault());
                    string mstUserId = string.Concat((from d in db.MstUsers where d.UserId == userId select d.Id).SingleOrDefault());
                    string officialReceiptName = string.Concat((from d in db.MstUsers where d.UserId == userId select d.OfficialReceiptName).SingleOrDefault());
                    string inventoryType = string.Concat((from d in db.MstUsers where d.UserId == userId select d.InventoryType).SingleOrDefault());
                    string defaultSalesInvoiceDiscount = string.Concat((from d in db.MstUsers where d.UserId == userId select d.MstDiscount.Discount).SingleOrDefault());

                    ViewData.Add("UserId", userId);
                    ViewData.Add("FullName", fullName);
                    ViewData.Add("Email", email);
                    ViewData.Add("UserName", userName);

                    ViewData.Add("BranchId", branchId);
                    ViewData.Add("Branch", branch);
                    ViewData.Add("Company", company);
                    ViewData.Add("MstUserId", mstUserId);
                    ViewData.Add("OfficialReceiptName", officialReceiptName);
                    ViewData.Add("InventoryType", inventoryType);
                    ViewData.Add("DefaultSalesInvoiceDiscount", defaultSalesInvoiceDiscount);
                }
            }
            base.OnActionExecuted(filterContext);
        }
        //public UserAccountController()
        //{ }
    }
}
