using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {

        IComentarioService _comentarioService;
        public ComentarioController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        // GET: api/<ComentarioController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_comentarioService.GetAll());
        }

        // GET api/<ComentarioController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_comentarioService.Get(id));
        }

        // POST api/<ComentarioController>
        [HttpPost]
        public void Post([FromBody] ComentarioDTO comentario)
        {
            _comentarioService.Add(comentario);
        }

        // PUT api/<ComentarioController>/5
        [HttpPut("{id}")]
        public void Put([FromBody] ComentarioDTO comentario)
        {
            _comentarioService.Update(comentario);
        }

        // DELETE api/<ComentarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _comentarioService.Delete(id);
        }
    }
}
