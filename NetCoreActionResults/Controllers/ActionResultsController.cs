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
            return Content("<p>Content Result</p>", "text/html");
        }


        //Partialview ise tanımlanmış olan bir htmli arayüzde bir parça halinde göstermek için kullanılır. Partial view html içeriği kendi sayfasında üretilir.
        [HttpGet("partial-result")]
        public PartialViewResult Partial()
        {
            var model = new PartialViewModel();
            //return PartialView("~/Views/Home/Partials/_TestPartial.cshtml",model);
            //Shared klasörü altından klasör ismi view ismi şeklinde çağırabiliriz.
            return PartialView("Partials/_TestPartial", model);
        }


        //ViewComponentler Shared/Components klasörü altından çağırılırlar.
        //ViewComponentler partialview'e çok benzer fakat partialviewden daha bağımsız yapılardır.
        [HttpGet("view-component-result")]
        public ViewComponentResult ViewComponentResult()
        {
            return ViewComponent("Test");
        }

        //Uygulama AJAX gibi yapılar ile sayfa yenilenmeden js kodu ile arayüzde güncelleme yapmamız gereken durumlarda tercih edilen bir result tipidir. Uygulama içinde yazılmış başka uygulamaların-
        //besleneceği bir servis gibi de kullanılabilir.
        //Like, comment, paylaş gibi arayüzde sayfa yenilenmeden yapılması gereken işlemler için tercih ederiz.
        //JsonResultların kendine ait bir view'ı olmaz. Bir view içerisinden viewcomponent gibi veya partial gibi çağırılırlar. Tek değişiklik js olarak çağırılmalarıdır.
        //AJAX için FromBody yazmayı unutmayalım!!
        //Fetch işlemlerinde post ederken FromBody attribute yazmamıza gerek yok.
        [HttpPost("json-result")]
        public JsonResult JsonResult([FromBody] UserInputModel model)
        {
            //Veri tabanı işlemleri.
            var result = new JsonViewModel
            {
                IsSucceded = true,
                Message = "İşlem Başarılı"
            };

            return Json(result);
        }


        [HttpGet("users",Name ="Users")]
        public ViewResult Users()
        {
            var users = new List<UserViewModel>();
            users.Add(new UserViewModel { Id = 1, Email = "email1@gmail.com", UserName = "User1" });
            users.Add(new UserViewModel { Id = 2, Email = "email2@gmail.com", UserName = "User2" });
            users.Add(new UserViewModel { Id = 3, Email = "email3@gmail.com", UserName = "User3" });


            return View(users);
        }



    }
}
