using BackEnd.DTO;
using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TareaController : ControllerBase
    {
        ITareaService _tareaService;
        public TareaController(ITareaService tareaService)
        {
            _tareaService = tareaService;
        }

        // GET: api/<TareaController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_tareaService.GetAll());
        }

        // GET api/<TareaController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_tareaService.Get(id));
        }

        // POST api/<EquipoController>
        [HttpPost]
        public void Post([FromBody] TareaDTO tarea)
        {
            _tareaService.Add(tarea);
        }

        // PUT api/<EquipoController>/5
        [HttpPut]
        public void Put([FromBody] TareaDTO tarea)
        {
            _tareaService.Update(tarea);
        }

        // DELETE api/<TareaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _tareaService.Delete(id);
        }

        [HttpGet("Usuario/{idUsuario}")]
        public ActionResult GetAllByUser(int idUsuario)
        {
            return Ok(_tareaService.GetAllByUser(idUsuario));
        }

        [HttpGet("Equipo/{idUsuario}")]
        public ActionResult GetAllByEquipoAndUser(int idUsuario)
        {
            return Ok(_tareaService.GetAllByEquipoAndUser(idUsuario));
        }
    }
}
