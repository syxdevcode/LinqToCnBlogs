using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LinqToCnBlogs.Web.Models;

namespace LinqToCnBlogs.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public JsonResult Index(SearchCriteria criteria = null)
        {
            var result = PostManager.Posts;
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Title))
                    result = result.Where(
                        p => p.Title.IndexOf(criteria.Title, StringComparison.OrdinalIgnoreCase) >= 0);

                if (!string.IsNullOrEmpty(criteria.Author))
                    result = result.Where(p => p.Author.IndexOf(criteria.Author, StringComparison.OrdinalIgnoreCase) >= 0);

                if (criteria.Start.HasValue)
                    result = result.Where(p => p.Published >= criteria.Start.Value);

                if (criteria.End.HasValue)
                    result = result.Where(p => p.Published <= criteria.End.Value);

                if (criteria.MinComments > 0)
                    result = result.Where(p => p.Comments >= criteria.MinComments);

                if (criteria.MinDiggs > 0)
                    result = result.Where(p => p.Diggs >= criteria.MinDiggs);

                if (criteria.MinViews > 0)
                    result = result.Where(p => p.Diggs >= criteria.MinViews);

                if (criteria.MaxComments > 0)
                    result = result.Where(p => p.Comments <= criteria.MaxComments);

                if (criteria.MaxDiggs > 0)
                    result = result.Where(p => p.Diggs <= criteria.MaxDiggs);

                if (criteria.MaxViews > 0)
                    result = result.Where(p => p.Diggs <= criteria.MaxViews);
            }
            return Json(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
