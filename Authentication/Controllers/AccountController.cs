using Authentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authentication.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("login", Name = "loginRoute")]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginInputModel();
            //HttpContext.Request.Path;

            ViewBag.ReturnUrl = ReturnUrl;

            return View(model);
        }

        [HttpPost("login", Name = "loginRoute")]
        public async Task<IActionResult> Login(LoginInputModel model, string ReturnUrl)
        {
            if (model.Email == "test@test.com" && model.Password == "pass123")
            {
                //Claim Based Authentication
                //Kullanıcı hesabına ait özellikler
                //Email, username, userId, Admin, showReports, downloadFile..
                var claim = new List<Claim>();
                claim.Add(new Claim("UserName", "test"));
                claim.Add(new Claim("Email", model.Email));
                claim.Add(new Claim("UserId", Guid.NewGuid().ToString()));
                claim.Add(new Claim("Role", "Admin"));
                claim.Add(new Claim("Role", "Manager"));
                //claim.Add(new Claim("User","Delete")); //Permission

                //ClaimIdentity bu sınıf üzerinden yukaridaki claimler ile login olabiliriz
                //Oturum açacak kullanıcı kimlik bilgisi.
                var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme, nameType: "UserName", roleType: "Role");
                var principle = new ClaimsPrincipal(claimsIdentity);

                var authProps = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,//Outurum kalıcı olsun. false dersek oturum session bazlı tutulur. Siteden ayrılma tekrar login olmamız gerekir.
                    AllowRefresh = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, authProps);

                //if (returnUrl)
                //{
                //    return Redirect(returnUrl);
                //}

                //var returnUrl = HttpContext.Request.Query["ReturnUrl"];
                return Redirect(ReturnUrl);

            }


            return Redirect("/");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);//Oturumdan güvenli çıkış yapmamızı sağlar.
            return Redirect("/login");
        }

        [HttpGet("unauthorized")]
        public IActionResult UnAuthorized()
        {
            return View();
        }
    }
}
