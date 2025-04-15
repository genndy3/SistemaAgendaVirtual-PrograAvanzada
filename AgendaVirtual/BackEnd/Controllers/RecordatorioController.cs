using BackEnd.DTO;
using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordatorioController : ControllerBase
    {
        IRecordatorioService _recordatorioService;
        public RecordatorioController(IRecordatorioService recordatorioService)
        {
            _recordatorioService = recordatorioService;
        }

        // GET: api/<TareaController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_recordatorioService.GetAll());
        }

        // GET api/<TareaController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_recordatorioService.Get(id));
        }

        // POST api/<EquipoController>
        [HttpPost]
        public RecordatorioDTO Post([FromBody] RecordatorioDTO recordatorio)
        {
            RecordatorioDTO nuevoRecordatorio = _recordatorioService.Add(recordatorio);
            return nuevoRecordatorio;
        }

        // PUT api/<EquipoController>/5
        [HttpPut]
        public void Put([FromBody] RecordatorioDTO recordatorio)
        {
            _recordatorioService.Update(recordatorio);
        }

        // DELETE api/<TareaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _recordatorioService.Delete(id);
        }

        [HttpGet("Tarea/{idTarea}")]
        public ActionResult GetAllByTarea(int idTarea)
        {
            return Ok(_recordatorioService.getAllByTarea(idTarea));
        }

        [HttpGet("RecordatoriosNotIn/{idTarea}")]
        public ActionResult GetAllNotInTarea(int idTarea)
        {
            return Ok(_recordatorioService.getAllNotInTarea(idTarea));
        }
    }
}
