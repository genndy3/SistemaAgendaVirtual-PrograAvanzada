using FrontEnd.Helpers.Implementations;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace FrontEnd.Controllers
{
    public class UsuarioController : Controller
    {


        IUsuarioHelper _usuarioHelper;

        public UsuarioController(IUsuarioHelper usuarioHelper)
        {

            _usuarioHelper = usuarioHelper;
        }

        // GET: UsuarioController
        public ActionResult Index()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario == -1)
            {
                return RedirectToAction("Login", "Login");
            }

            _usuarioHelper.Token = HttpContext.Session.GetString("Token");
            var result = _usuarioHelper.GetUsuario(idUsuario);

            return View(result);
        }

        public int GetUserIdFromToken()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return -1;
            }

            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                return -1;
            }

            var payload = parts[1];
            int padding = payload.Length % 4 == 0 ? 0 : 4 - payload.Length % 4;
            payload = payload.PadRight(payload.Length + padding, '=');

            var jsonBytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(jsonBytes);
            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            if (!jsonObject.ContainsKey("id_usuario"))
            {
                return -1;
            }

            int idUsuario;
            if (!int.TryParse(jsonObject["id_usuario"].ToString(), out idUsuario))
            {
                return -1;
            }

            return idUsuario;
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UsuarioViewModel usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(usuario);
                }

                _usuarioHelper.Token = HttpContext.Session.GetString("Token");
                var usuarioActualizado = _usuarioHelper.UpdateUsuario(usuario);

                TempData["SuccessMessage"] = "Perfil actualizado correctamente";
                return View(usuarioActualizado);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el perfil: " + ex.Message);
                return View(usuario);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Eliminar la cookie de autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Remove("Token");

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarCuenta()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario != -1)
            {
                _usuarioHelper.Token = HttpContext.Session.GetString("Token");
                _usuarioHelper.DeleteUsuario(idUsuario);
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Login", "Login");
        }

    }
}
