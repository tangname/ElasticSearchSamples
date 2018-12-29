using ElasticeSearch_Service;
using ElasticeSearch_Service.Dtos;
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

            var output = new ESProvider().Search(input);

            var result = new ArticleResultVM
            {
                Input = input,
                Output = output
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

            var output = new ESProvider().Search(input);
            var result = new ArticleResultVM
            {
                Input = input,
                Output = output
            };

            return View("Index", result);
        }

        public ActionResult Detail(Guid id)
        {
            var article = new ESProvider().GetArticle(id);

            return View(article);
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
    }
}