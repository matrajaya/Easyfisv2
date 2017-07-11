using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class SupplierController : Controller
    {
        // TODO List: 
        // * Chages the return url pages to another controller for forbidden page...
        // * Do not use the default software forbidden page...

        // data database context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // supplier list page
        [Authorize]
        public ActionResult Index()
        {
            // current user
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            // check if user found
            if (currentUser.Any())
            {
                // user forms
                var userForms = from d in db.MstUserForms
                                where d.UserId == currentUser.FirstOrDefault().Id
                                && d.SysForm.FormName.Equals("SupplierList")
                                select d;

                // check if user form found
                if (userForms.Any())
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Forbidden", "Software");
                }
            }
            else
            {
                // TODO: Change this return URL to Not Current User Found Page ...
                return RedirectToAction("Forbidden", "Software");
            }
        }

        // supplier detail page
        [Authorize]
        public ActionResult SupplierDetail(Int32? id)
        {
            if (id != null)
            {
                // current user
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                // check if user found
                if (currentUser.Any())
                {
                    // user forms
                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUser.FirstOrDefault().Id
                                    && d.SysForm.FormName.Equals("SupplierDetail")
                                    select d;

                    // check if user form found
                    if (userForms.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Forbidden", "Software");
                    }
                }
                else
                {
                    // TODO: Change this return URL to Not Current User Found Page ...
                    return RedirectToAction("Forbidden", "Software");
                }
            }
            else
            {
                // TODO: Change this return URL to Not Found Page ...
                return RedirectToAction("Forbidden", "Software");
            }
        }
    }
}