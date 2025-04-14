using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

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
            _repository.Client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage response = _repository.DeleteResponse($"api/Usuario/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                throw new Exception($"Error al eliminar usuario: {response.StatusCode} - {errorContent}");
            }
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
            try
            {
                _repository.Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);

                HttpResponseMessage response = _repository.GetResponse($"api/usuario/{id}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al obtener usuario: {response.StatusCode}");
                }

                var content = response.Content.ReadAsStringAsync().Result;
                return Convertir(JsonConvert.DeserializeObject<UsuarioAPI>(content));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetUsuario: {ex.Message}");
                throw;
            }
        }

        public UsuarioViewModel UpdateUsuario(UsuarioViewModel usuario)
        {
            try
            {
                _repository.Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);

                HttpResponseMessage response = _repository.PutResponse(
                    $"api/usuario/{usuario.IdUsuario}",
                    Convertir(usuario));

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Error en la API: {response.StatusCode} - {errorContent}");
                }

                var content = response.Content.ReadAsStringAsync().Result;
                return Convertir(JsonConvert.DeserializeObject<UsuarioAPI>(content));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UsuarioHelper.UpdateUsuario: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}