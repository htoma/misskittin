using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MissKittin.Azure;
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
                var catData = TableManager.GetAll<CatEntity>("cats").ToDictionary(x => x.Id, x => x.Likes);
                foreach (var cat in cats)
                {
                    int likes;
                    if (catData.TryGetValue(cat.Id, out likes))
                    {
                        cat.Likes = likes;
                    }
                }
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
