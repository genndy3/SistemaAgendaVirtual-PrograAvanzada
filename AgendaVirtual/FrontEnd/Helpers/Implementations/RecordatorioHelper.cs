using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class RecordatorioHelper : IRecordatorioHelper
    {

        IServiceRepository _repository;
        public string Token { get; set; }

        public RecordatorioHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        RecordatorioViewModel Convertir(RecordatorioAPI recordatorioAPI)
        {
            return new RecordatorioViewModel
            {
                IdRecordatorio = recordatorioAPI.IdRecordatorio,
                IdTarea = recordatorioAPI.IdTarea,
                IdUsuario = recordatorioAPI.IdUsuario,
                FechaHora = recordatorioAPI.FechaHora,
                Mensaje = recordatorioAPI.Mensaje
            };
        }
        RecordatorioAPI Convertir(RecordatorioViewModel recordatorio)
        {
            return new RecordatorioAPI
            {
                IdRecordatorio = recordatorio.IdRecordatorio,
                IdTarea = recordatorio.IdTarea,
                IdUsuario = recordatorio.IdUsuario,
                FechaHora = recordatorio.FechaHora,
                Mensaje = recordatorio.Mensaje
            };
        }

        public RecordatorioViewModel AddRecordatorio(RecordatorioViewModel recordatorio)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PostResponse("api/recordatorio", Convertir(recordatorio));
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                recordatorio = Convertir(JsonConvert.DeserializeObject<RecordatorioAPI>(content));
            }
            return recordatorio;
        }

        public void DeleteRecordatorio(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.DeleteResponse($"api/recordatorio/{id}");
        }

        public RecordatorioViewModel GetRecordatorio(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/recordatorio/{id}");
            RecordatorioAPI recordatorio = new RecordatorioAPI();
            
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                recordatorio = JsonConvert.DeserializeObject<RecordatorioAPI>(content);
            }
            return Convertir(recordatorio);
        }

        public List<RecordatorioViewModel> GetRecordatorios()
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/recordatorio");
            List<RecordatorioAPI> recordatorios = new List<RecordatorioAPI>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                recordatorios = JsonConvert.DeserializeObject<List<RecordatorioAPI>>(content);
            }

            List<RecordatorioViewModel> resultado = new List<RecordatorioViewModel>();
            foreach (var recordatorio in recordatorios)
            {
                resultado.Add(Convertir(recordatorio));
            }
            return resultado;
        }

        public RecordatorioViewModel UpdateRecordatorio(RecordatorioViewModel recordatorio)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PutResponse($"api/recordatorio/{recordatorio.IdRecordatorio}", Convertir(recordatorio));
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                recordatorio = Convertir(JsonConvert.DeserializeObject<RecordatorioAPI>(content));
            }
            return recordatorio;
        }

        public List<RecordatorioViewModel> GetRecordatoriosByTarea(int idTarea)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/recordatorio/tarea/{idTarea}");
            List<RecordatorioAPI> recordatorios = new List<RecordatorioAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                recordatorios = JsonConvert.DeserializeObject<List<RecordatorioAPI>>(content);
            }
            List<RecordatorioViewModel> resultado = new List<RecordatorioViewModel>();
            foreach (var recordatorio in recordatorios)
            {
                resultado.Add(Convertir(recordatorio));
            }
            return resultado;
        }

        public List<RecordatorioViewModel> GetRecordatoriosNotInTarea(int idTarea)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/recordatorio/RecordatoriosNotIn/{idTarea}");
            List<RecordatorioAPI> recordatorios = new List<RecordatorioAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                recordatorios = JsonConvert.DeserializeObject<List<RecordatorioAPI>>(content);
            }
            List<RecordatorioViewModel> resultado = new List<RecordatorioViewModel>();
            foreach (var recordatorio in recordatorios)
            {
                resultado.Add(Convertir(recordatorio));
            }
            return resultado;
        }
    }
}
