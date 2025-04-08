using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        public IActionResult AdminDashboard()
        {
            return View(); 
        }

        public IActionResult admUsuarios()
        {
            return View();
        }

        public IActionResult admEquipos()
        {
            return View();
        }

        public IActionResult admTareas()
        {
            return View();
        }
    }
}
