using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StateManagement.Models;
using System.Buffers;
using System.Diagnostics;
using System.Text.Json;

namespace StateManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache cache)
        {
            _logger = logger;
            this._cache = cache;//Dependecy Injection? yaptık.
        }
        [HttpGet("create-cookie", Name = "createCookie")]
        public IActionResult CookieSample()
        {
            CookieOptions c = new CookieOptions();
            c.HttpOnly = false; //JS üzerinden cokkie erişimi kapadık.
            c.Expires = DateTime.Now.AddDays(30); //30 gün cookie değerini kullanabiliriz.
            c.Secure = true;//Cookie genelde http protokolü üzerinden çalışır. Cookieler http üzerinden requestde okunabildiğinden dolayı güvenlik açığı oluştururuz. Bundan dolayı sadece--
            //https prolokolü ile ssl ile çalış dedik.
            c.SameSite = SameSiteMode.Strict;//Cookie sadece kendi domaininden gelen isteklerde geçerli olsun.

            //Cookie de sadece key value cinsinden string değer saklarız.
            Response.Cookies.Append("username", "Ali", c);
            Response.Cookies.Append("email", "ali@test.com", c);

            //Cookie oluşturma sonrasında redicrect ile genellikle anasayfaya yönleniriz. 1 redirect sonrası cookie oluşmu olur.

            return RedirectToRoute("getCookie", new { id = 2, name = "Ahmet" });
        }

        [HttpGet("get-cookie", Name = "getCookie")]
        public IActionResult GetCookies([FromQuery] int id = 1, [FromQuery] string name = "First Name")//[FromRoute] get-cookie/{id} -- Detay sayfası için route kullanılabilir.
        {
            var username = Request.Cookies["username"].ToString();
            var email = Request.Cookies["email"].ToString();
            ViewBag.Message = "ViewBag Mesajı";
            return View();
        }

        public IActionResult SetSession()
        {
            //HTTPContext uygulamanın bütün web ile ilgili isteklerini üzerinde barındıran sınıf. ViewContextde ekstradan viewle ilgili şeyler barındırır. Ayrıyetten HttpContexi de barındırır.
            HttpContext.Session.SetString("username", "Ali");

            //Object tabanlı çalışmak için JSONSerializer paketini kullanarak objeyi jsonstring çeviririz.
            var sessionModel = new SessionModel();
            //HttpContext.Session.Id sistem üretir her bir oturum için oturumu takip etmek amaçlı unique bir id üretir.
            sessionModel.SessionId = HttpContext.Session.Id;
            //Obje ile çalışmak için modelimizi jsonStringe çevirip sonra string olarak saklıyoruz.
            var sessionModelJson = JsonSerializer.Serialize(sessionModel);
            //System.Text.Json.JsonSerializer.Serialize(sessionModel);
            HttpContext.Session.SetString("sessionModel", sessionModelJson);

            //Eğer route tanımı yapılmaz ise aynı controller seviyesindeysek sadece action ismi yazmanız yeterli olacaktır.
            return RedirectToAction("GetSession");
        }
        public IActionResult GetSession()
        {
            var sessionJson = HttpContext.Session.GetString("sessionModel");
            if (sessionJson is not null)
            {
                var n = System.Text.Json.JsonSerializer.Deserialize<SessionModel>(sessionJson);
                return View(n);
            }

            return View();
        }

        //Cache application bazlı çalışır. Cookie ve Session oturum bazlı çalışır.
        //Bütün herkesi ilgilendiren veriler performans amaçlı cachede tutulur.
        public IActionResult SetCache()
        {
            //Ids keyinde veri yoksa cachle
            if (_cache.Get("Ids") == null)
            {
                var cacheIds = new List<CacheModel>();
                cacheIds.Add(new CacheModel { Id = Guid.NewGuid().ToString() });
                cacheIds.Add(new CacheModel { Id = Guid.NewGuid().ToString() });
                //Ids key ile 1 günlüğüne cacheIds verisine in memory olarak sunucunun raminde saklamak istiyoruz.
                _cache.Set("Ids", cacheIds, DateTime.Now.AddDays(1));

            }

         
            return Redirect("GetCache");
        }

        public IActionResult GetCache()
        {
            //Cacheden veri okuma yöntemi.
            var model = _cache.Get<List<CacheModel>>("Ids");

            return View(model);
        }



        public IActionResult Index()
        {
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