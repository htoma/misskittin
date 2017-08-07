using System;
using System.Collections.Concurrent;
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
        private const string TableName = "cats";

        private static readonly ConcurrentDictionary<string, Cat> CatLikes = new ConcurrentDictionary<string, Cat>();
        
        private static readonly object Lock = new object();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            try
            {
                LoadCache();
                return View(CatLikes.Values.OrderByDescending(x => x.Likes).ToList());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public string UpCatLove(string id)
        {
            lock (Lock)
            {
                Cat cat;
                if (CatLikes.TryGetValue(id, out cat))
                {
                    cat.Likes++;
                    TableManager.Insert<CatEntity>(TableName, new CatEntity(id, cat.Url, cat.Likes));
                    return cat.Likes.ToString();
                }
            }
            return "0";
        }

        private List<Cat> GetCats()
        {
            var catJson = new WebClient().DownloadString(CatLocation);
            return JsonConvert.DeserializeObject<JsonData>(catJson).Images;
        }

        private void LoadCache()
        {
            if (CatLikes.IsEmpty)
            {
                lock (Lock)
                {
                    if (CatLikes.IsEmpty)
                    {
                        var cats = GetCats();
                        var catData = TableManager.GetAll<CatEntity>(TableName).ToDictionary(x => x.Id, x => x.Likes);
                        foreach (var cat in cats)
                        {
                            int likes;
                            if (catData.TryGetValue(cat.Id, out likes))
                            {
                                cat.Likes = likes;
                            }
                            CatLikes.TryAdd(cat.Id, cat);
                        }
                    }
                }
            }
        }
    }
}
