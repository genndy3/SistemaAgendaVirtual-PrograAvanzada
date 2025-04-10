using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class TareaHelper : ITareaHelper
    {
        IServiceRepository _repository;
        public TareaHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        TareaViewModel Convertir(TareaAPI tarea)
        {
            return new TareaViewModel
            {
                IdTarea = tarea.IdTarea,
                IdUsuario = tarea.IdUsuario,
                IdEquipo = tarea.IdEquipo,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                Prioridad = tarea.Prioridad,
                Estado = tarea.Estado
            };
        }

        TareaAPI Convertir(TareaViewModel tarea)
        {
            return new TareaAPI
            {
                IdTarea = tarea.IdTarea,
                IdUsuario = tarea.IdUsuario,
                IdEquipo = tarea.IdEquipo,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                Prioridad = tarea.Prioridad,
                Estado = tarea.Estado
            };
        }

        public TareaViewModel AddTarea(TareaViewModel tareaViewModel)
        {
            HttpResponseMessage responseMessage = _repository.PostResponse("api/Tarea", Convertir(tareaViewModel));
            if (responseMessage != null)
            {
                var content = responseMessage.Content;
            }
            return tareaViewModel;
        }

        public void DeleteTarea(int id)
        {
            HttpResponseMessage responseMessage = _repository.DeleteResponse("api/Tarea/" + id.ToString());
            if (responseMessage != null)
            {
                var content = responseMessage.Content;
            }

        }

        public List<TareaViewModel> GetAll()
        {
            List<TareaAPI> tareas = new List<TareaAPI>();
            HttpResponseMessage responseMessage = _repository.GetResponse("api/Tarea");

            if (responseMessage != null)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tareas = JsonConvert.DeserializeObject<List<TareaAPI>>(content) ?? new List<TareaAPI>();

            }
            List<TareaViewModel> list = new List<TareaViewModel>();
            foreach (var item in tareas)
            {
                list.Add(Convertir(item));
            }
            return list;
        }

        public TareaViewModel GetById(int id)
        {
            TareaAPI tarea = new TareaAPI();
            HttpResponseMessage responseMessage = _repository.GetResponse("api/Tarea/" + id.ToString());

            if (responseMessage != null)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tarea = JsonConvert.DeserializeObject<TareaAPI>(content);
            }
            return Convertir(tarea);
        }

        public TareaViewModel UpdateTarea(TareaViewModel tareaViewModel)
        {
            HttpResponseMessage responseMessage = _repository.PutResponse("api/Tarea", Convertir(tareaViewModel));
            if (responseMessage != null)
            {
                var content = responseMessage.Content;
            }
            return tareaViewModel;
        }
    }
}
