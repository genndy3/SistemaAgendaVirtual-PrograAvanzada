using FrontEnd.Helpers.Implementations;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        IEquipoHelper _equipoHelper;
        IUsuarioHelper _usuariosHelper;
        IUsuarioEquipoHelper _usuarioEquipoHelper;
        ITareaHelper _tareaHelper;
        ISecurityHelper _securityHelper;

        public AdminController(IEquipoHelper equipoHelper, IUsuarioHelper usuariosHelper, IUsuarioEquipoHelper usuarioEquipoHelper, ITareaHelper tareaHelper, ISecurityHelper securityHelper)
        {
            _equipoHelper = equipoHelper;
            _usuariosHelper = usuariosHelper;
            _usuarioEquipoHelper = usuarioEquipoHelper;
            _tareaHelper = tareaHelper;
            _securityHelper = securityHelper;
        }

        // GET: AdminController
        public IActionResult AdminDashboard()
        {
            return View();
        }

        //****************************************************************************************************//
        //___________________________________________ADMINISTRACION USUARIOS__________________________________//
        //****************************************************************************************************//

        public IActionResult admUsuarios()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> crear(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            try
            {
                _securityHelper.Register(user.UserName, user.Email, user.Password);

                return RedirectToAction("admUsuarios", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado. " + ex.Message;
                return View(user);
            }
        }


        public IActionResult UpdateUsuario([FromBody] UserViewModel usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _usuariosHelper.Token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(_usuariosHelper.Token))
                {
                    return Unauthorized("Token no válido");
                }

                if (usuario == null)
                {
                    return BadRequest("Datos de usuario no válidos");
                }

                // Conversión manual
                var usuarioConvertido = new UsuarioViewModel
                {
                    Nombre = usuario.UserName,
                    Correo = usuario.Email ?? "",
                    Rol = "Usuario",
                    FechaRegistro = DateTime.Now
                };

                var usuarioActualizado = _usuariosHelper.UpdateUsuario(usuarioConvertido);
                return Ok(usuarioActualizado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new
                {
                    Message = "Error interno al actualizar el usuario",
                    DetailedError = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }


        [HttpDelete("DeleteUsuario/{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            try
            {
                _usuariosHelper.Token = HttpContext.Session.GetString("Token");

                // 1. Verificar si el usuario existe
                var usuario = _usuariosHelper.GetUsuario(id);
                if (usuario == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"No se encontró el usuario con ID {id}"
                    });
                }

                // 2. Eliminar el usuario
                _usuariosHelper.DeleteUsuario(id);

                return Ok(new
                {
                    Success = true,
                    Message = $"Usuario {usuario.Nombre} eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error interno al eliminar el usuario",
                    Error = ex.Message
                });
            }
        }

        //****************************************************************************************************//
        //___________________________________________ADMINISTRACION EQUIPOS___________________________________//
        //****************************************************************************************************//

        public IActionResult admEquipos()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario == -1)
            {
                return RedirectToAction("Login", "Login");
            }

            _equipoHelper.Token = HttpContext.Session.GetString("Token");
            var equipos = _equipoHelper.GetEquiposPorUsuario(idUsuario);
            foreach (var equipo in equipos)
            {
                var usuarios = _equipoHelper.GetUsuariosPorEquipo(equipo.IdEquipo);
                equipo.ParticipanteList = new List<UsuarioViewModel>();
                foreach (var usuario in usuarios)
                {
                    equipo.ParticipanteList.Add(usuario);
                }
            }

            ViewBag.IdUsuarioAuth = idUsuario;

            return View(equipos);
        }

        [HttpGet]
        public IActionResult GetEquipos()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario == -1)
            {
                return RedirectToAction("Login", "Login");
            }

            _equipoHelper.Token = HttpContext.Session.GetString("Token");
            var equipos = _equipoHelper.GetEquiposPorUsuario(idUsuario);
            foreach (var equipo in equipos)
            {
                var usuarios = _equipoHelper.GetUsuariosPorEquipo(equipo.IdEquipo);
                equipo.ParticipanteList = new List<UsuarioViewModel>();
                foreach (var usuario in usuarios)
                {
                    equipo.ParticipanteList.Add(usuario);
                }
            }

            return Json(equipos);
        }


        // GET: EquipoController/Details/5
        public ActionResult Details(int id)
        {
            _equipoHelper.Token = HttpContext.Session.GetString("Token");

            var equipo = _equipoHelper.getEquipo(id);
            if (equipo == null)
            {
                return NotFound();
            }

            var usuarios = _equipoHelper.GetUsuariosPorEquipo(equipo.IdEquipo);

            equipo.ParticipanteList = usuarios?.ToList() ?? new List<UsuarioViewModel>();

            return Json(equipo);
        }

        [HttpPost]
        public ActionResult AddUsuario([FromBody] UsuarioEquipoViewModel usuarioEquipo)
        {
            try
            {
                _usuarioEquipoHelper.Token = HttpContext.Session.GetString("Token");
                var usuario = _usuarioEquipoHelper.addUsuarioEquipo(usuarioEquipo);

                var equipo = _equipoHelper.getEquipo(usuarioEquipo.IdEquipo);
                var usuarios = _equipoHelper.GetUsuariosPorEquipo(usuarioEquipo.IdEquipo);

                equipo.ParticipanteList = usuarios?.ToList() ?? new List<UsuarioViewModel>();

                return Json(equipo);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error al agregar usuario al equipo: " + ex.Message);
            }
        }


        [HttpDelete("Equipo/DeleteUsuario/{idUsuario}/{idEquipo}")]
        public IActionResult DeleteUsuario(int idUsuario, int idEquipo)
        {
            _usuarioEquipoHelper.deleteUsuarioEquipo(idUsuario, idEquipo);
            return Ok();
        }

        public ActionResult UsuariosNotInEquipo(int id)
        {
            _equipoHelper.Token = HttpContext.Session.GetString("Token");
            var usuarios = _equipoHelper.GetUsuariosNotInEquipo(id);
            return Json(usuarios);
        }

        // GET: EquipoController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            _usuariosHelper.Token = HttpContext.Session.GetString("Token");
            var usuarios = _usuariosHelper.GetUsuarios();
            return Json(usuarios);
        }

        // POST: EquipoController/Create
        [HttpPost]
        public IActionResult Create([FromBody] EquipoViewModel equipo)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Token de autenticación no válido"
                    });
                }

                _equipoHelper.Token = token;

                equipo.ParticipanteList = equipo.ParticipanteList ?? new List<UsuarioViewModel>();
                EquipoViewModel equipoCreado = _equipoHelper.addEquipo(equipo);

                if (equipo.ParticipanteList.Any())
                {
          
                    foreach (var participante in equipo.ParticipanteList)
                    {
                        var usuarioEquipo = new UsuarioEquipoViewModel
                        {
                            IdUsuario = participante.IdUsuario,
                            IdEquipo = equipoCreado.IdEquipo 
                        };

                        _usuarioEquipoHelper.addUsuarioEquipo(usuarioEquipo);
                        Console.WriteLine($"Participante agregado: {participante.IdUsuario}");
                        Console.WriteLine($"EQuipo agregado: {usuarioEquipo.IdEquipo}");
                    }
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Equipo actualizado correctamente",
                    Equipo = equipoCreado
                });
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error al crear equipo: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error interno al crear el equipo",
                    Error = ex.Message
                });
            }
        }


        // GET: EquipoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EquipoController/Edit/5
        [HttpPut]
        public IActionResult Edit([FromBody] EquipoViewModel equipo)
        {
            try
            {
                _equipoHelper.Token = HttpContext.Session.GetString("Token");

                equipo.ParticipanteList = equipo.ParticipanteList ?? new List<UsuarioViewModel>();

                var equipoActualizado = _equipoHelper.updateEquipo(equipo);

                var participantesActuales = _equipoHelper.GetUsuariosPorEquipo(equipo.IdEquipo)
                                          ?? new List<UsuarioViewModel>();

                var idsNuevos = equipo.ParticipanteList.Select(p => p.IdUsuario).ToList();
                var idsActuales = participantesActuales.Select(p => p.IdUsuario).ToList();

                var idsParaEliminar = idsActuales.Except(idsNuevos).ToList();

                foreach (var id in idsParaEliminar)
                {
                    _usuarioEquipoHelper.deleteUsuarioEquipo(id, equipo.IdEquipo);
                }

                var idsParaAgregar = idsNuevos.Except(idsActuales).ToList();

                foreach (var id in idsParaAgregar)
                {
                    var usuarioEquipo = new UsuarioEquipoViewModel
                    {
                        IdUsuario = id,
                        IdEquipo = equipo.IdEquipo
                    };
                    _usuarioEquipoHelper.addUsuarioEquipo(usuarioEquipo);
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Equipo actualizado correctamente",
                    Equipo = equipoActualizado
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Edit: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error al editar el equipo",
                    Error = ex.Message
                });
            }
        }

        // GET: EquipoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EquipoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEquipo(int id)
        {
            try
            {
                _equipoHelper.Token = HttpContext.Session.GetString("Token");

                var usuarios = _equipoHelper.GetUsuariosPorEquipo(id);
                foreach (var usuario in usuarios)
                {
                    _usuarioEquipoHelper.deleteUsuarioEquipo(usuario.IdUsuario, id);
                }

                bool eliminado = _equipoHelper.DeleteEquipo(id);

                if (eliminado)
                {
                    return RedirectToAction(nameof(admEquipos));
                }
                return RedirectToAction(nameof(admEquipos));
            }
            catch
            {
                return RedirectToAction(nameof(admEquipos));
            }
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




        //****************************************************************************************************//
        //___________________________________________ADMINISTRACION TAREAS____________________________________//
        //****************************************************************************************************//


        public IActionResult admTareas()
        {
            {
                int idUsuario = GetUserIdFromToken2();
                if (idUsuario == -1)
                {
                    return RedirectToAction("Login", "Login");
                }

                _tareaHelper.Token = HttpContext.Session.GetString("Token");
                var result = _tareaHelper.GetTareasPersonales(idUsuario);

                ViewBag.IdUsuario = idUsuario;

                return View(result);
            }

        }

        public int GetUserIdFromToken2()
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

        // GET: TareaController
        public ActionResult PorEquipo()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario == -1)
            {
                return RedirectToAction("Login", "Login");
            }

            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var result = _tareaHelper.GetTareasPorEquipo(idUsuario);
            return View(result);
        }



        // GET: TareaController/Details/5
        public IActionResult DetailsTarea(int id)
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
                _tareaHelper.AddTarea(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TareaController/Edit/5
        public ActionResult EditTarea(int id)
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
                _tareaHelper.UpdateTarea(tarea);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: TareaController/Delete/5
        public ActionResult DeleteTarea(int id)
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
                _tareaHelper.DeleteTarea(id);
                return RedirectToAction(nameof(admTareas));
            }
            catch
            {
                return View();
            }
        }

    }
}