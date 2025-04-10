using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class TareaHelper : ITareaHelper
    {
        private readonly IServiceRepository _repository;
        public string Token { get; set; }

        public TareaHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        private TareaViewModel Convertir(TareaAPI tarea)
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

        private TareaAPI Convertir(TareaViewModel tarea)
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
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.PostResponse("api/Tarea", Convertir(tareaViewModel));
            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
            }
            return tareaViewModel;
        }

        public void DeleteTarea(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.DeleteResponse("api/Tarea/" + id.ToString());
            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        public List<TareaViewModel> GetAll()
        {
            List<TareaAPI> tareas = new List<TareaAPI>();
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/Tarea");

            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
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
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/Tarea/" + id.ToString());

            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tarea = JsonConvert.DeserializeObject<TareaAPI>(content);
            }
            return Convertir(tarea);
        }

        public TareaViewModel UpdateTarea(TareaViewModel tareaViewModel)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.PutResponse("api/Tarea", Convertir(tareaViewModel));
            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
            }
            return tareaViewModel;
        }

        public List<TareaViewModel> GetTareasPersonales(int usuarioId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/Tarea/Usuario/" + usuarioId.ToString());
            List<TareaAPI> tareas = new List<TareaAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tareas = JsonConvert.DeserializeObject<List<TareaAPI>>(content);
            }
            List<TareaViewModel> resultado = new List<TareaViewModel>();
            foreach (var tarea in tareas)
            {
                resultado.Add(Convertir(tarea));
            }
            return resultado;
        }

        public List<TareaViewModel> GetTareasPorEquipo(int equipoId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/Tarea/Equipo/" + equipoId.ToString());
            List<TareaAPI> tareas = new List<TareaAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tareas = JsonConvert.DeserializeObject<List<TareaAPI>>(content);
            }
            List<TareaViewModel> resultado = new List<TareaViewModel>();
            foreach (var tarea in tareas)
            {
                resultado.Add(Convertir(tarea));
            }
            return resultado;
        }
    }
}
