using System.Text;
using FrontEnd.Helpers.Implementations;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class TareaController : Controller
    {
        ITareaHelper _tareaHelper;
        IRecordatorioHelper _recordatorioHelper;
        IComentarioHelper _comentarioHelper;
        IEquipoHelper _equipoHelper;
        IUsuarioHelper _usuarioHelper;
        public TareaController(ITareaHelper tareaHelper, IRecordatorioHelper recordatorioHelper, IComentarioHelper comentarioHelper, IEquipoHelper equipoHelper, IUsuarioHelper usuarioHelper)
        {
            _tareaHelper = tareaHelper;
            _recordatorioHelper = recordatorioHelper;
            _comentarioHelper = comentarioHelper;
            _equipoHelper = equipoHelper;
            _usuarioHelper = usuarioHelper;
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
            var result = _tareaHelper.GetTareasPersonales(idUsuario);

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
            var result = _tareaHelper.GetTareasPorEquipo(idUsuario);

            ViewBag.IdUsuario = idUsuario;

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
            try
            {
                _tareaHelper.Token = HttpContext.Session.GetString("Token");

                var tarea = _tareaHelper.GetById(id);
                if (tarea == null)
                {
                    return NotFound();
                }

                tarea.RecordatoriosList = _recordatorioHelper.GetRecordatoriosByTarea(id) ?? new List<RecordatorioViewModel>();

                tarea.ComentariosList = _comentarioHelper.GetComentariosByTarea(id) ?? new List<ComentarioViewModel>();

                tarea.ComentariosList = tarea.ComentariosList.OrderByDescending(c => c.FechaHora).ToList();

                return Ok(tarea);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error al obtener los detalles de la tarea",
                    Error = ex.Message
                });
            }
        }



        // POST: TareaController/Create
        [HttpPost]
        public IActionResult Create([FromBody] TareaViewModel tarea)
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

                _tareaHelper.Token = token;
                _recordatorioHelper.Token = token;
                _comentarioHelper.Token = token;

                var nuevaTarea = _tareaHelper.AddTarea(tarea);

                tarea.RecordatoriosList = tarea.RecordatoriosList ?? new List<RecordatorioViewModel>();
                foreach (var recordatorio in tarea.RecordatoriosList)
                {
                    var nuevoRecordatorio = new RecordatorioViewModel
                    {
                        IdTarea = nuevaTarea.IdTarea,
                        IdUsuario = recordatorio.IdUsuario,
                        Mensaje = recordatorio.Mensaje,
                        FechaHora = recordatorio.FechaHora
                    };
                    _recordatorioHelper.AddRecordatorio(nuevoRecordatorio);
                }

                tarea.ComentariosList = tarea.ComentariosList ?? new List<ComentarioViewModel>();
                foreach (var comentario in tarea.ComentariosList)
                {
                    var nuevoComentario = new ComentarioViewModel
                    {
                        IdTarea = nuevaTarea.IdTarea,
                        IdUsuario = comentario.IdUsuario,
                        Texto = comentario.Texto,
                        FechaHora = DateTime.Now
                    };
                    _comentarioHelper.AddComentario(nuevoComentario);
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Tarea creada correctamente",
                    Tarea = nuevaTarea
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear tarea: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error interno al crear la tarea",
                    Error = ex.Message
                });
            }
        }

        // GET: TareaController/Edit/5
        public ActionResult Edit(int id)
        {
            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var tarea = _tareaHelper.getTarea(id);
            return View(tarea);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] TareaViewModel tarea)
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

                _tareaHelper.Token = token;
                _recordatorioHelper.Token = token;
                _comentarioHelper.Token = token;

                var tareaActualizada = _tareaHelper.UpdateTarea(tarea);

                tarea.RecordatoriosList = tarea.RecordatoriosList ?? new List<RecordatorioViewModel>();
                var recordatoriosActuales = _recordatorioHelper.GetRecordatoriosByTarea(tarea.IdTarea)
                                          ?? new List<RecordatorioViewModel>();

                var idsNuevos = tarea.RecordatoriosList.Select(r => r.IdRecordatorio).ToList();
                var idsActuales = recordatoriosActuales.Select(r => r.IdRecordatorio).ToList();

                var idsParaEliminar = idsActuales.Except(idsNuevos).ToList();
                foreach (var id in idsParaEliminar)
                {
                    _recordatorioHelper.DeleteRecordatorio(id);
                }

                foreach (var recordatorio in tarea.RecordatoriosList)
                {
                    if (recordatorio.IdRecordatorio == 0)
                    {
                        var nuevoRecordatorio = new RecordatorioViewModel
                        {
                            IdTarea = tarea.IdTarea,
                            IdUsuario = tarea.IdUsuario,
                            Mensaje = recordatorio.Mensaje,
                            FechaHora = recordatorio.FechaHora
                        };
                        _recordatorioHelper.AddRecordatorio(nuevoRecordatorio);
                    }
                    else
                    {
                        _recordatorioHelper.UpdateRecordatorio(recordatorio);
                    }
                }

                tarea.ComentariosList = tarea.ComentariosList ?? new List<ComentarioViewModel>();
                var comentariosActuales = _comentarioHelper.GetComentariosByTarea(tarea.IdTarea) ?? new List<ComentarioViewModel>();

                var idsNuevosComentarios = tarea.ComentariosList.Where(c => c.IdComentario == 0).Select(c => c.IdComentario).ToList();
                var idsActualesComentarios = comentariosActuales.Select(c => c.IdComentario).ToList();

                foreach (var id in idsActualesComentarios.Except(idsNuevosComentarios))
                {
                    _comentarioHelper.DeleteComentario(id);
                }

                foreach (var comentario in tarea.ComentariosList)
                {
                    if (comentario.IdComentario == 0)
                    {
                        try
                        {
                            var nuevoComentario = new ComentarioViewModel
                            {
                                IdTarea = tarea.IdTarea,
                                IdUsuario = tarea.IdUsuario,
                                Texto = comentario.Texto,
                                FechaHora = DateTime.Now
                            };
                            _comentarioHelper.AddComentario(nuevoComentario);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error agregando comentario: {ex.Message}");
                        }
                    }
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Tarea actualizada correctamente",
                    Tarea = tareaActualizada
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar tarea: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error interno al actualizar la tarea",
                    Error = ex.Message
                });
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
                var token = HttpContext.Session.GetString("Token");
                _tareaHelper.Token = token;
                _recordatorioHelper.Token = token;
                var recordatorios = _recordatorioHelper.GetRecordatoriosByTarea(id);
                foreach (var recordatorio in recordatorios)
                {
                    _recordatorioHelper.DeleteRecordatorio(recordatorio.IdRecordatorio);
                }
                _tareaHelper.DeleteTarea(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult GetRecordatorios()
        {
            _tareaHelper.Token = HttpContext.Session.GetString("Token");
            var recordatorios = _tareaHelper.GetTareasPersonales(GetUserIdFromToken());
            return Json(recordatorios);
        }

        public ActionResult GetRecordatoriosNotInTarea(int id)
        {
            _recordatorioHelper.Token = HttpContext.Session.GetString("Token");
            var recordatorios = _recordatorioHelper.GetRecordatoriosNotInTarea(id);
            return Json(recordatorios);
        }

        public ActionResult GetRecordatoriosByTarea(int id)
        {
            _recordatorioHelper.Token = HttpContext.Session.GetString("Token");
            var recordatorios = _recordatorioHelper.GetRecordatoriosByTarea(id);
            return Json(recordatorios);
        }

        public ActionResult GetComentariosByTarea(int id)
        {
            _comentarioHelper.Token = HttpContext.Session.GetString("Token");
            var comentarios = _comentarioHelper.GetComentariosByTarea(id);
            return Json(comentarios);
        }

        public ActionResult AddComentario([FromBody] ComentarioViewModel comentario)
        {
            try
            {
                _comentarioHelper.Token = HttpContext.Session.GetString("Token");
                var comentarioCreado = _comentarioHelper.AddComentario(comentario);
                return Json(comentarioCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error al agregar el comentario",
                    Error = ex.Message
                });
            }
        }

        public IActionResult GetUsuarios()
        {
            try {
                _usuarioHelper.Token = HttpContext.Session.GetString("Token");
                var usuario = _usuarioHelper.GetUsuarios();
                if (usuario == null)
                {
                    return NotFound();
                }
                return Json(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error al obtener el usuario",
                    Error = ex.Message
                });
            }
        }
        public ActionResult DeleteComentario(int id)
        {
            try
            {
                _comentarioHelper.Token = HttpContext.Session.GetString("Token");
                _comentarioHelper.DeleteComentario(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Comentario eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error al eliminar el comentario",
                    Error = ex.Message
                });
            }
        }

        public IActionResult GetEquipos()
        {
            _equipoHelper.Token = HttpContext.Session.GetString("Token");
            var id = GetUserIdFromToken();
            var equipos = _equipoHelper.GetEquiposPorUsuario(id);
            return Json(equipos);
        }
    }
}