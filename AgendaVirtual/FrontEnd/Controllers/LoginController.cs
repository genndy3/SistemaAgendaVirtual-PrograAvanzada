using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FrontEnd.Controllers
{
    public class LoginController : Controller
    {
        ISecurityHelper _securityHelper;

        public LoginController(ISecurityHelper securityHelper)
        {
            _securityHelper = securityHelper;
        }


        public ActionResult Login()
        {
            UserViewModel user = new UserViewModel();
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(UserViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var login = _securityHelper
                        .Login(user.UserName, user.Password);
                    if (login.Token != null)
                    {
                        TokenAPI token = new TokenAPI
                        {
                            Token = login.Token.Token,
                            Expiration = login.Token.Expiration
                        };
                        HttpContext.Session.SetString("Token", token.Token);




                        var claims = new List<Claim>()
                         {
                             new Claim(ClaimTypes.NameIdentifier, login.Username as string),
                             new Claim(ClaimTypes.Name, login.Username as string),
                         };
                        var roles = login.Roles;
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        { IsPersistent = false });

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Invalid login attempt.");
                        return View(user);
                    }
                }
                return View(user);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
