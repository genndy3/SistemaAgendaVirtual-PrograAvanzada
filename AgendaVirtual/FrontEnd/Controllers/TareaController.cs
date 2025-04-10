using System.Text;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class TareaController : Controller
    {
        ITareaHelper _tareaHelper;
        public TareaController(ITareaHelper tareaHelper)
        {
            _tareaHelper = tareaHelper;
        }


        // GET: TareaController
        public ActionResult Index()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario == -1)
            {
                return RedirectToAction("Login", "Login");
            }

            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var result = _tareaHelper.getTareasPersonales(idUsuario);

            // Pasa el idUsuario a la vista
            ViewBag.IdUsuario = idUsuario;

            return View(result);
        }



        // GET: TareaController
        public ActionResult PorEquipo()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario == -1)
            {
                return RedirectToAction("Login", "Login");
            }

            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var result = _tareaHelper.getTareasPorEquipo(idUsuario);
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



        // GET: TareaController/Details/5
        public IActionResult Details(int id)
        {
            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var tarea = _tareaHelper.getTarea(id);
            return Json(tarea);
        }



        // POST: TareaController/Create
        [HttpPost]
        public ActionResult Create([FromBody] TareaViewModel tarea)
        {
            try
            {
                _tareaHelper.Token = HttpContext.Session.GetString("Token");
                _tareaHelper.addTarea(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TareaController/Edit/5
        public ActionResult Edit(int id)
        {
            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var tarea = _tareaHelper.getTarea(id);
            return View(tarea);
        }

        // POST: TareaController/Edit/5
        [HttpPut]
        public IActionResult Edit([FromBody] TareaViewModel tarea)
        {
            try
            {
                _tareaHelper.Token = HttpContext.Session.GetString("Token");
                _tareaHelper.updateTarea(tarea);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: TareaController/Delete/5
        public ActionResult Delete(int id)
        {
            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var tarea = _tareaHelper.getTarea(id);
            return View(tarea);
        }

        // POST: TareaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _tareaHelper.Token = HttpContext.Session.GetString("Token");
                _tareaHelper.deleteTarea(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}