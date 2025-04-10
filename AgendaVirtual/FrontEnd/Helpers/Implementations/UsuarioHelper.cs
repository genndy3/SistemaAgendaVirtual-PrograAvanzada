using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class UsuarioHelper : IUsuarioHelper
    {
        IServiceRepository _serviceRepository;
        public string Token { get; set; }
        public UsuarioHelper(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        UsuarioViewModel convertir(UsuarioAPI usuarioAPI)
        {
            return new UsuarioViewModel
            {
                IdUsuario = usuarioAPI.IdUsuario,
                Nombre = usuarioAPI.Nombre
            };
        }
        public UsuarioViewModel addUsuario(UsuarioViewModel usuario)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PostResponse("api/usuario", usuario);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
            }
            return usuario;
        }

        public void deleteUsuario(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.DeleteResponse($"api/usuario/{id}");
        }

        public List<UsuarioViewModel> getUsuarios()
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/usuario");
            List<UsuarioAPI> usuarios = new List<UsuarioAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                usuarios = JsonConvert.DeserializeObject<List<UsuarioAPI>>(content);
            }
            List<UsuarioViewModel> resultado = new List<UsuarioViewModel>();
            foreach (var usuario in usuarios)
            {
                resultado.Add(convertir(usuario));
            }
            return resultado;
        }

        public UsuarioViewModel getUsuario(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/usuario/" + id.ToString());
            UsuarioAPI usuario = new UsuarioAPI();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                usuario = JsonConvert.DeserializeObject<UsuarioAPI>(content);
            }
            UsuarioViewModel resultado = convertir(usuario);
            return resultado;
        }

        public UsuarioViewModel updateUsuario(UsuarioViewModel usuario)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PutResponse("api/usuario", usuario);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(content);
            }
            return usuario;
        }
    }
}