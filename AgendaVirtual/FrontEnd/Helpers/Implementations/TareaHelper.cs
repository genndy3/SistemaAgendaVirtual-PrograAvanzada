    using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class TareaHelper : ITareaHelper
    {
        IServiceRepository _repository;
        public string Token { get; set; }

        public TareaHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        TareaViewModel Convertir(TareaAPI tareaAPI)
        {
            return new TareaViewModel
            {
                IdTarea = tareaAPI.IdTarea,
                Titulo = tareaAPI.Titulo,
                Descripcion = tareaAPI.Descripcion,
                FechaLimite = tareaAPI.FechaLimite,
                Prioridad = tareaAPI.Prioridad,
                Estado = tareaAPI.Estado,
                IdEquipo = tareaAPI.IdEquipo,
                IdUsuario = tareaAPI.IdUsuario
            };
        }

        public TareaViewModel AddTarea(TareaViewModel tarea)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PostResponse("api/tarea", tarea);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                tarea = Convertir(JsonConvert.DeserializeObject<TareaAPI>(content));
            }
            return tarea;
        }

        public TareaViewModel getTarea(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/tarea/" + id.ToString());
            TareaAPI tarea = new TareaAPI();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tarea = JsonConvert.DeserializeObject<TareaAPI>(content);
            }
            TareaViewModel resultado = Convertir(tarea);
            return resultado;
        }
        public void DeleteTarea(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.DeleteResponse($"api/tarea/{id}");
        }

        public TareaViewModel GetById(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/tarea/{id}");
            TareaAPI tarea = new TareaAPI();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tarea = JsonConvert.DeserializeObject<TareaAPI>(content);
            }
            return Convertir(tarea);
        }

        public List<TareaViewModel> GetAll()
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/tarea");
            List<TareaAPI> tareas = new List<TareaAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tareas = JsonConvert.DeserializeObject<List<TareaAPI>>(content) ?? new List<TareaAPI>();
            }
            List<TareaViewModel> resultado = new List<TareaViewModel>();
            foreach (var tarea in tareas)
            {
                resultado.Add(Convertir(tarea));
            }
            return resultado;
        }

        public TareaViewModel UpdateTarea(TareaViewModel tarea)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PutResponse("api/tarea", tarea);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                tarea = JsonConvert.DeserializeObject<TareaViewModel>(content);
            }
            return tarea;
        }

        public List<TareaViewModel> GetTareasPersonales(int usuarioId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/tarea/Usuario/{usuarioId}");
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
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/tarea/Equipo/{equipoId}");
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