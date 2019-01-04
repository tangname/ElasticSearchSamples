using ElasticeSearch_Service;
using ElasticeSearch_Service.Dtos;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticSearch_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var input = new ArticeSearchVM()
            {
                Page = 1,
                Size = 50,
                Key = "",
                Sort = 0,
            };

            var provider = new ESProvider();

            var output = provider.Search(input);
            var buckets = provider.Aggs();

            var result = new ArticleResultVM
            {
                Input = input,
                Output = output,
                Buckets=buckets
            };

            return View(result);
        }

        public ActionResult GetArticles()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetArticles(ArticeSearchVM input)
        {
            if (input.Page <= 0) input.Page = 1;
            if (input.Size <= 0) input.Size = 50;

            var provider = new ESProvider();

            var output = provider.Search(input);
            var buckets = provider.Aggs();

            var result = new ArticleResultVM
            {
                Input = input,
                Output = output,
                Buckets = buckets
            };

            return View("Index", result);
        }

        public ActionResult Detail(Guid id, string key)
        {
            var provider = new ESProvider();

            var article = provider.GetArticle(id);
            var explain = provider.Explain(id, key);

            var vm = new DetailVM()
            {
                Article = article,
            };

            if (explain != null)
            {
                vm.Explanation = Newtonsoft.Json.JsonConvert.SerializeObject(explain, Newtonsoft.Json.Formatting.Indented);
            }

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }

    public class ArticeSearchVM : QueryModel
    {

    }

    public class ArticleResultVM
    {
        public ArticeSearchVM Input { get; set; }

        public PageOut Output { get; set; }

        public IReadOnlyCollection<KeyedBucket<string>> Buckets { get; set; }
    }

    public class DetailVM
    {
        public Article Article { get; set; }

        public string Explanation { get; set; }
    }

    public static class Extension
    {
        public static string Date(this DateTime? dateTime)
        {
            if (dateTime.HasValue) return dateTime.Value.ToShortDateString();
            else return "";
        }
    }
}