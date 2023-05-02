using Microsoft.AspNetCore.Mvc;
using NetCoreActionResults.Models;
using System.Diagnostics;

namespace NetCoreActionResults.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //View'i döndürürken eğer view içerisinde tanımlı bir partial varsa view modele partial modelleri eklememiz gerekecek. Bu da geliştirme yaparken bir sorun teşkil edecek.
        public IActionResult Index()
        {
            var model = new HomeViewModel();
            model.PartialModel=new PartialViewModel();


            return View(model);
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