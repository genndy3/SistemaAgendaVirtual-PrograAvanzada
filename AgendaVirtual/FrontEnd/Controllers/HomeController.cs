using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IRecordatorioHelper _recordatorioHelper;
        ITareaHelper _tareaHelper;

        public HomeController(ILogger<HomeController> logger, IRecordatorioHelper recordatorioHelper, ITareaHelper tareaHelper)
        {
            _logger = logger;
            _recordatorioHelper = recordatorioHelper;
            _tareaHelper = tareaHelper;
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

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }
            else { 
            _recordatorioHelper.Token = HttpContext.Session.GetString("Token");
            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var recordatorios = _recordatorioHelper.GetRecordatorios();
            var usuarioId = GetUserIdFromToken();
            recordatorios = recordatorios.Where(r => r.IdUsuario == usuarioId).ToList();

            var tareas = _tareaHelper.GetAll();

            foreach (var recordatorio in recordatorios)
{
                if (recordatorio.IdTarea == null)
                {
                    recordatorio.TituloTarea = "Tarea no asignada"; 
                    continue;
                }

                var tarea = tareas.FirstOrDefault(t => t.IdTarea == recordatorio.IdTarea);
                recordatorio.TituloTarea = tarea?.Titulo ?? "Sin título"; 
            }
            

            return View(recordatorios);
            }
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
