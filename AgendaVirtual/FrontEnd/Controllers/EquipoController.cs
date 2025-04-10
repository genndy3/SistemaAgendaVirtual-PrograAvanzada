using System.Text;
using FrontEnd.Helpers.Implementations;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class EquipoController : Controller
    {
        IEquipoHelper _equipoHelper;
        IUsuarioHelper _usuariosHelper;
        IUsuarioEquipoHelper _usuarioEquipoHelper;
        public EquipoController(IEquipoHelper equipoHelper, IUsuarioHelper usuariosHelper, IUsuarioEquipoHelper usuarioEquipoHelper)
        {
            _equipoHelper = equipoHelper;
            _usuariosHelper = usuariosHelper;
            _usuarioEquipoHelper = usuarioEquipoHelper;
        }
        // GET: EquipoController
        public ActionResult Index()
        {
            int idUsuario = GetUserIdFromToken();
            if (idUsuario == -1)
            {
                return RedirectToAction("Login", "Login");
            }

            _equipoHelper.Token = HttpContext.Session.GetString("Token");
            var equipos = _equipoHelper.getEquiposPorUsuario(idUsuario);
            foreach (var equipo in equipos)
            {
                var usuarios = _equipoHelper.getUsuariosPorEquipo(equipo.IdEquipo);
                equipo.participanteList = new List<UsuarioViewModel>();
                foreach (var usuario in usuarios)
                {
                    equipo.participanteList.Add(usuario);
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
            var equipos = _equipoHelper.getEquiposPorUsuario(idUsuario);
            foreach (var equipo in equipos)
            {
                var usuarios = _equipoHelper.getUsuariosPorEquipo(equipo.IdEquipo);
                equipo.participanteList = new List<UsuarioViewModel>();
                foreach (var usuario in usuarios)
                {
                    equipo.participanteList.Add(usuario);
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

            var usuarios = _equipoHelper.getUsuariosPorEquipo(equipo.IdEquipo);

            equipo.participanteList = usuarios?.ToList() ?? new List<UsuarioViewModel>();

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
                var usuarios = _equipoHelper.getUsuariosPorEquipo(usuarioEquipo.IdEquipo);

                equipo.participanteList = usuarios?.ToList() ?? new List<UsuarioViewModel>();

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
            var usuarios = _equipoHelper.getUsuariosNotInEquipo(id);
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
            var usuarios = _usuariosHelper.getUsuarios();
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

                equipo.participanteList = equipo.participanteList ?? new List<UsuarioViewModel>();
                EquipoViewModel equipoCreado = _equipoHelper.addEquipo(equipo);

                if (equipo.participanteList.Any())
                {
                    // Agregar los nuevos participantes al equipo
                    foreach (var participante in equipo.participanteList)
                    {
                        var usuarioEquipo = new UsuarioEquipoViewModel
                        {
                            IdUsuario = participante.IdUsuario,
                            IdEquipo = equipoCreado.IdEquipo  // Asegúrate de asignar el IdEquipo correcto
                        };

                        // Llamada al helper para agregar el participante
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

                equipo.participanteList = equipo.participanteList ?? new List<UsuarioViewModel>();

                var equipoActualizado = _equipoHelper.updateEquipo(equipo);

                var participantesActuales = _equipoHelper.getUsuariosPorEquipo(equipo.IdEquipo)
                                          ?? new List<UsuarioViewModel>();

                var idsNuevos = equipo.participanteList.Select(p => p.IdUsuario).ToList();
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
    }
}