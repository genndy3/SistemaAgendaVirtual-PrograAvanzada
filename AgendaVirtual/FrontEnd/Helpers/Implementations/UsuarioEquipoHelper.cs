using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;

namespace FrontEnd.Helpers.Implementations
{
    public class UsuarioEquipoHelper : IUsuarioEquipoHelper
    {
        IServiceRepository _serviceRepository;
        public string Token { get; set; }
        public UsuarioEquipoHelper(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public UsuarioEquipoViewModel addUsuarioEquipo(UsuarioEquipoViewModel usuarioEquipo)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PostResponse("api/usuarioEquipo", usuarioEquipo);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
            }
            return usuarioEquipo;
        }

        public void deleteUsuarioEquipo(int idUsuario, int idEquipo)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.DeleteResponse($"api/usuarioEquipo/{idUsuario}/{idEquipo}");
        }
    }
}