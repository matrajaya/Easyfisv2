using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class SoftwareController : Controller
    {
        // GET: Software
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}