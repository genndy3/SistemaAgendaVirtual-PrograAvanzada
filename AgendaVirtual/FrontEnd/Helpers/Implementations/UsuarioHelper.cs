using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class UsuarioHelper : IUsuarioHelper
    {
        IServiceRepository _repository;
        public string Token { get; set; }

        public UsuarioHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        UsuarioViewModel Convertir(UsuarioAPI usuarioAPI)
        {
            return new UsuarioViewModel
            {
                IdUsuario = usuarioAPI.IdUsuario,
                Nombre = usuarioAPI.Nombre,
                Correo = usuarioAPI.Correo,
                Rol = usuarioAPI.Rol,
                FechaRegistro = usuarioAPI.FechaRegistro
            };
        }

        UsuarioAPI Convertir(UsuarioViewModel usuario)
        {
            return new UsuarioAPI
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public UsuarioViewModel AddUsuario(UsuarioViewModel usuario)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PostResponse("api/usuario", Convertir(usuario));
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                usuario = Convertir(JsonConvert.DeserializeObject<UsuarioAPI>(content));
            }
            return usuario;
        }

        public void DeleteUsuario(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.DeleteResponse($"api/usuario/{id}");
        }

        public List<UsuarioViewModel> GetUsuarios()
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/usuario");
            List<UsuarioAPI> usuarios = new List<UsuarioAPI>();
            
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                usuarios = JsonConvert.DeserializeObject<List<UsuarioAPI>>(content);
            }
            
            List<UsuarioViewModel> resultado = new List<UsuarioViewModel>();
            foreach (var usuario in usuarios)
            {
                resultado.Add(Convertir(usuario));
            }
            return resultado;
        }

        public UsuarioViewModel GetUsuario(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/usuario/{id}");
            UsuarioAPI usuario = new UsuarioAPI();
            
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                usuario = JsonConvert.DeserializeObject<UsuarioAPI>(content);
            }
            return Convertir(usuario);
        }

        public UsuarioViewModel UpdateUsuario(UsuarioViewModel usuario)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PutResponse($"api/usuario/{usuario.IdUsuario}", Convertir(usuario));
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                usuario = Convertir(JsonConvert.DeserializeObject<UsuarioAPI>(content));
            }
            return usuario;
        }
    }
}