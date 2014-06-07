using System.Collections.Generic;
using GolGuru.Data;
using GolGuru.Models;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace GolGuru.Controllers
{
    public class EventsController : Controller
    {
        [HttpGet]
        public ActionResult GolEvent()
        {
            return View();

        }
        [HttpPost]
        public ActionResult GolEvent(Event gol)
        {
            GolGuru.Instance.BrodcastMatchLiveEvents("Gol", gol);
            return RedirectToAction("MainMenu", "Home");

        }
        [HttpGet]
        public ActionResult ExpulsionEvent()
        {
            return View();

        }

        [HttpPost]
        public ActionResult ExpulsionEvent(Event exp)
        {
            GolGuru.Instance.BrodcastMatchLiveEvents("Expulsion", exp);
            return RedirectToAction("MainMenu", "Home");

        }
        [HttpGet]
        public ActionResult CambioEvent()
        {

            return View();

        }
        [HttpPost]
        public ActionResult CambioEvent(Event cambio)
        {
            GolGuru.Instance.BrodcastMatchLiveEvents("Cambio", cambio);
            return RedirectToAction("MainMenu", "Home");

        }

    }
}
