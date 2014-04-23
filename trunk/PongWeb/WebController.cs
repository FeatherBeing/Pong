using System;
using System.Web;
using System.Web.Mvc;

namespace PongWeb
{
    public class WebController : Controller
    {
        //
        // GET: /Index/
        public ActionResult Index()
        {
            return View();
        }
    }
}