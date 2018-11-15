using RCSEntities;
using System;
using System.Web.Mvc;

namespace RCSHaryana.Controllers
{
    public class UnauthorisedController : Controller
    {
        // GET: Unauthorised
        [HandleError]
        public ActionResult Index()
        {
            return View();
        }

        [HandleError]
        public ActionResult Error()
        {
            return View();
        }


        [HandleError]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Error(FormCollection fc)
        {
            if (Convert.ToInt32(Session["RoleId"]) == 1)
            {
                return RedirectToAction("Application", "Society");
            }
            if (Convert.ToInt32(Session["RoleId"]) == 2)
            {
                return RedirectToAction("Dashboard", "ARCS");
            }
            if (Convert.ToInt32(Session["RoleId"]) == 3)
            {
                return RedirectToAction("Dashboard", "Inspector");
            }
            return View();
        }
    }
}