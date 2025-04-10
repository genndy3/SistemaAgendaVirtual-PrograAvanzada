using BackEnd.DTO;
using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        IEquipoService _equipoService;
        public EquipoController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        // GET: api/<EquipoController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_equipoService.GetAll());
        }

        // GET api/<EquipoController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_equipoService.Get(id));
        }

        // POST api/<EquipoController>
        [HttpPost]
        public EquipoDTO Post([FromBody] EquipoDTO equipo)
        {
            EquipoDTO newEquipo = _equipoService.Add(equipo);
            return newEquipo;
        }

        // PUT api/<EquipoController>/5
        [HttpPut]
        public void Put([FromBody] EquipoDTO equipo)
        {
            _equipoService.Update(equipo);
        }

        // DELETE api/<EquipoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _equipoService.Delete(id);
        }

        [HttpGet("Usuario/{idUsuario}")]
        public ActionResult GetAllByUser(int idUsuario)
        {
            return Ok(_equipoService.GetAllByUser(idUsuario));
        }

        [HttpGet("Equipo/{idEquipo}")]
        public ActionResult GetUsuariosByEquipo(int idEquipo)
        {
            return Ok(_equipoService.GetUsuariosByEquipo(idEquipo));
        }

        [HttpGet("UsuariosNotIn/{idEquipo}")]
        public ActionResult GetUsuariosNotInEquipo(int idEquipo)
        {
            return Ok(_equipoService.GetUsuariosNotIntEquipo(idEquipo));
        }

        [HttpGet("EquipoPorUsuario/{idUsuario}")]
        public ActionResult GetEquipoPorUsuario(int idUsuario)
        {
            return Ok(_equipoService.GetEquipoPorUsuario(idUsuario));
        }
    }
}