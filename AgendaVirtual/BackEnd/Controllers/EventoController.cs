using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        IEventoService _eventoService;
        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        // GET: api/<EventoController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_eventoService.GetAll());
        }

        // GET api/<EventoController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_eventoService.Get(id));
        }

        // POST api/<EventoController>
        [HttpPost]
        public void Post([FromBody] EventoDTO evento)
        {
            _eventoService.Add(evento);
        }

        // PUT api/<EventoController>/5
        [HttpPut("{id}")]
        public void Put([FromBody] EventoDTO evento)
        {
            _eventoService.Update(evento);
        }

        // DELETE api/<EventoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _eventoService.Delete(id);
        }
    }
}
