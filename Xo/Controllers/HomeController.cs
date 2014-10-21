using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xo.Areas.Infrastructure.Alerts;

namespace Xo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View().WithInfo("Your application description page.");
        }

        public ActionResult Contact()
        {
            return View().WithInfo("Your contact page.");
        }
    }
}