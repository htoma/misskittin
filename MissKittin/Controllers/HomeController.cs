using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MissKittin.Models;
using Newtonsoft.Json;

namespace MissKittin.Controllers
{
    public class HomeController : Controller
    {
        private const string CatLocation = "https://latelier.co/data/cats.json";

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            try
            {
                var cats = GetCats();
                return View(cats);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public string UpCatLove(string id)
        {
            return "3";
        }

        private List<Cat> GetCats()
        {
            var catJson = new WebClient().DownloadString(CatLocation);
            return JsonConvert.DeserializeObject<JsonData>(catJson).Images;
        }
    }
}
