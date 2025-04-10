using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/<UsuarioController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_usuarioService.GetAll());
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_usuarioService.Get(id));
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public void Post([FromBody] UsuarioDTO usuario)
        {
            _usuarioService.Add(usuario);
        }

        // PUT api/<UsuarioController>/5
        [HttpPut]
        public void Put([FromBody] UsuarioDTO usuario)
        {
            _usuarioService.Update(usuario);
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _usuarioService.Delete(id);
        }
        
    }
}
