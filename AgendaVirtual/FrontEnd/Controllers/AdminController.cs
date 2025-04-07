using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;  

namespace FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        IUsuarioHelper _usuarioHelper;
        IEquipoHelper _equipoHelper;
        ITareaHelper _tareaHelper;
        public AdminController(IUsuarioHelper usuarioHelper, IEquipoHelper equipoHelper, ITareaHelper tareaHelper)
        {
            _usuarioHelper = usuarioHelper;
            _equipoHelper = equipoHelper;
            _tareaHelper = tareaHelper;
        }


        // GET: AdminController
        public ActionResult AdminDashboard()
        {
            return View();
        }
        


        public ActionResult admUsuarios()
        {        
            return View(_usuarioHelper.GetAll());
        }



        public ActionResult admEquipos()
        {
            return View(_equipoHelper.GetEquipos());
        }






        public ActionResult admTareas()
        {
            return View(_tareaHelper.GetAll());
        }

        public ActionResult crearTareas()
        {
            TareaViewModel tarea = new TareaViewModel();
            tarea.Usuarios= _usuarioHelper.GetAll();
            tarea.Equipos = _equipoHelper.GetEquipos();
            return View(tarea);
        }

        [HttpPost]
        public ActionResult crearTareas(TareaViewModel tarea)
        {
            try
            {
                _tareaHelper.AddTarea(tarea);
                return RedirectToAction("admTareas");
            }
            catch
            {
                return View(tarea);
            }
        }

        [HttpGet]
        public IActionResult EditarTareas(int id)
        {
            var tarea = _tareaHelper.GetById(id);
            if (tarea == null)
            {
                return NotFound();
            }

            var model = new TareaViewModel
            {
                IdTarea = tarea.IdTarea,
                IdUsuario = tarea.IdUsuario,
                IdEquipo = tarea.IdEquipo,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                Prioridad = tarea.Prioridad,
                Estado = tarea.Estado
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarTareas(TareaViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                _tareaHelper.UpdateTarea(model);
                return RedirectToAction("admTareas"); 
            }

            return View(model);
        }

        public IActionResult EliminarTarea(int id)
        {
            TareaViewModel tarea = _tareaHelper.GetById(id);
            tarea.IdUsuario = tarea.IdUsuario;
            tarea.IdEquipo = tarea.IdEquipo;
            return View(tarea);
        }


    }
}
