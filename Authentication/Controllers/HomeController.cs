using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Authentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Login olan herkes girebilir.
        [Authorize]//Aspect orianted programing, cross cutting layer. Hata işmeli exception, veri doğrulama validation, authentication authrization, logging
        public IActionResult AuthenticatedUserPage()
        {
            return View();
        }

        //Sadece admin rolu olan girebilir.
        [Authorize(Roles ="Admin")]
        public IActionResult AdminPage()
        {
            return View();
        }

        //Sadece manager rolu olan girebilir.
        [Authorize(Roles = "Manager")]
        public IActionResult ManagerPage()
        {
            return View();
        }

        //Sadece user delete permissionuna sahip olan girebilir.
        [Authorize(Policy ="UserDeletePolicy")] //Özel bir kural tanımı yapılmalı. Poliçe bazlı kurallar uygulanırken bu poliçeler uygulamanın program dosyasında oluşturulur ve buradaki isimlerle çağırılır.
        public IActionResult UserDeletePage()
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