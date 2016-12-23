using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace SampleMvcApp.Controllers
{
    public class AccountController : Controller
    {
        IOptions<OpenIdConnectOptions> _options;

        public AccountController(IOptions<OpenIdConnectOptions> options)
        {
            _options = options;
        }

        /*
        public IActionResult Login(string returnUrl = "/")
        {
            return new ChallengeResult("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
        }
        */

        public IActionResult Login(string returnUrl = "/account/profile")
        {
            var lockContext = HttpContext.GenerateLockContext(_options.Value, returnUrl);

            return View(lockContext);
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Authentication.SignOutAsync("Auth0");
            HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var profile = new UserProfile
            {
                Id = User.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value,
                Name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Image = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
            };
            return View(profile);
        }

        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the Auth0 ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            //this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return View();
        }
    }
}
