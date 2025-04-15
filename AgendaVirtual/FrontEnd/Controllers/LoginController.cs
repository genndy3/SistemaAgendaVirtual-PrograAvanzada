using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using FrontEnd.Helpers.Implementations;

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

                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("AdminDashboard", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
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

        // GET: Mostrar la vista del formulario de registro
        public IActionResult Register()
        {
            return View(new UserViewModel());
        }

        // POST: Enviar datos al backend para registrar al usuario
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                _securityHelper.Register(user.UserName, user.Email, user.Password);

                    return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado. " + ex.Message;
                return View(user);
            }
        }
    }
}
