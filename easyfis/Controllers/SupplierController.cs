using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult SupplierDetail(Int32 id)
        {
            return View();
        }
    }
}