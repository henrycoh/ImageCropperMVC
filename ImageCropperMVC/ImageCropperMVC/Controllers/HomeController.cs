using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageCropperMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "ImageCropperMVC";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
