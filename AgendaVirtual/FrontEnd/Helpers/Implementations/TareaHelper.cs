using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class TareaHelper : ITareaHelper
    {
        IServiceRepository _serviceRepository;
        public string Token { get; set; }

        public TareaHelper(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        TareaViewModel convertir(TareaAPI tareaAPI)
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

        public TareaViewModel addTarea(TareaViewModel tarea)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PostResponse("api/tarea", tarea);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
            }
            return tarea;
        }

        public void deleteTarea(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.DeleteResponse($"api/tarea/{id}");
        }

        public TareaViewModel getTarea(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/tarea/" + id.ToString());
            TareaAPI tarea = new TareaAPI();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tarea = JsonConvert.DeserializeObject<TareaAPI>(content);
            }
            TareaViewModel resultado = convertir(tarea);
            return resultado;
        }

        public List<TareaViewModel> getTareas()
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/tarea");
            List<TareaAPI> tareas = new List<TareaAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tareas = JsonConvert.DeserializeObject<List<TareaAPI>>(content);
            }
            List<TareaViewModel> resultado = new List<TareaViewModel>();
            foreach (var tarea in tareas)
            {
                resultado.Add(convertir(tarea));
            }
            return resultado;
        }

        public List<TareaViewModel> getTareasPersonales(int usuarioId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/tarea/Usuario/"+ usuarioId.ToString());
            List<TareaAPI> tareas = new List<TareaAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tareas = JsonConvert.DeserializeObject<List<TareaAPI>>(content);
            }
            List<TareaViewModel> resultado = new List<TareaViewModel>();
            foreach (var tarea in tareas)
            {
                resultado.Add(convertir(tarea));
            }
            return resultado;
        }

        public List<TareaViewModel> getTareasPorEquipo(int usuarioId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/tarea/Equipo/" + usuarioId.ToString());
            List<TareaAPI> tareas = new List<TareaAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                tareas = JsonConvert.DeserializeObject<List<TareaAPI>>(content);
            }
            List<TareaViewModel> resultado = new List<TareaViewModel>();
            foreach (var tarea in tareas)
            {
                resultado.Add(convertir(tarea));
            }
            return resultado;
        }

        public TareaViewModel updateTarea(TareaViewModel tarea)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PutResponse("api/tarea", tarea);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                tarea = JsonConvert.DeserializeObject<TareaViewModel>(content);
            }
            return tarea;
        }
    }
}