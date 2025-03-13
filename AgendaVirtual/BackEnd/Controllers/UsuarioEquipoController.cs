using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioEquipoController : ControllerBase
    {
        IUsuarioEquipoService _usuarioEquipoService;
        public UsuarioEquipoController(IUsuarioEquipoService usuarioEquipoService)
        {
            _usuarioEquipoService = usuarioEquipoService;
        }

        // GET: api/<UsuarioEquipoController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_usuarioEquipoService.GetAll());
        }

        // GET api/<UsuarioEquipoController>/5
        [HttpGet("{idUsuario}/{idEquipo}")]
        public ActionResult Get(int idUsuario, int idEquipo)
        {
            return Ok(_usuarioEquipoService.Get(idUsuario, idEquipo));
        }

        // POST api/<UsuarioEquipoController>
        [HttpPost]
        public void Post([FromBody] UsuarioEquipoDTO usuarioEquipo)
        {
            _usuarioEquipoService.Add(usuarioEquipo);
        }

        // PUT api/<UsuarioEquipoController>/5
        [HttpPut]
        public void Put([FromBody] UsuarioEquipoDTO usuarioEquipo)
        {
            _usuarioEquipoService.Update(usuarioEquipo);
        }

        // DELETE api/<UsuarioEquipoController>/5
        [HttpDelete("{idUsuario}/{idEquipo}")]
        public void Delete(int idUsuario, int idEquipo)
        {
            _usuarioEquipoService.Delete(idUsuario, idEquipo);
        }
    }
}
