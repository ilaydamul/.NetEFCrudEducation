using Microsoft.AspNetCore.Mvc;
using NetCoreActionResults.Models;
using System.Security.Cryptography.X509Certificates;

namespace NetCoreActionResults.Controllers
{
    public class ActionResultsController : Controller
    {
        //String html static bir içeriği ekranda göstermek için kullanılan result
        [HttpGet("html-result")]
        public ContentResult HtmlContent()
        {
            return Content("<p>Content Result</p>","text/html");
        }


        //Partialview ise tanımlanmış olan bir htmli arayüzde bir parça halinde göstermek için kullanılır. Partial view html içeriği kendi sayfasında üretilir.
        [HttpGet("partial-result")]
        public PartialViewResult Partial()
        {
            var model = new PartialViewModel();
            //return PartialView("~/Views/Home/Partials/_TestPartial.cshtml",model);
            //Shared klasörü altından klasör ismi view ismi şeklinde çağırabiliriz.
            return PartialView("Partials/_TestPartial",model);
        }


        //ViewComponentler Shared/Components klasörü altından çağırılırlar.
        //ViewComponentler partialview'e çok benzer fakat partialviewden daha bağımsız yapılardır.
        [HttpGet("view-component-result")]
        public ViewComponentResult ViewComponentResult()
        {
            return ViewComponent("Test");
        }
    }
}
