using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class ComentarioHelper : IComentarioHelper
    {
        IServiceRepository _repository;
        public string Token { get; set; }
        public ComentarioHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        ComentarioViewModel Convertir(ComentarioAPI comentarioAPI)
        {
            return new ComentarioViewModel
            {
                IdComentario = comentarioAPI.IdComentario,
                IdTarea = comentarioAPI.IdTarea,
                IdUsuario = comentarioAPI.IdUsuario,
                Texto = comentarioAPI.Texto,
                FechaHora = comentarioAPI.FechaHora
            };
        }

        ComentarioAPI Convertir(ComentarioViewModel comentario)
        {
            return new ComentarioAPI
            {
                IdComentario = comentario.IdComentario,
                IdTarea = comentario.IdTarea,
                IdUsuario = comentario.IdUsuario,
                Texto = comentario.Texto,
                FechaHora = comentario.FechaHora
            };
        }
        public ComentarioViewModel AddComentario(ComentarioViewModel comentario)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.PostResponse("api/comentario", comentario);
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
            }
            return comentario;
        }

        public void DeleteComentario(int id)
        {

            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.DeleteResponse("api/comentario/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return;
            }
        }

        public List<ComentarioViewModel> GetComentariosByTarea(int tareaId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/comentario/GetAllByTarea/" + tareaId);
            List<ComentarioAPI> comentarios = new List<ComentarioAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                comentarios = JsonConvert.DeserializeObject<List<ComentarioAPI>>(content);
                List<ComentarioViewModel> resultado = new List<ComentarioViewModel>();
                foreach (var comentario in comentarios)
                {
                    resultado.Add(Convertir(comentario));
                }
                return resultado;
            }
            return null;
        }
    }
}
