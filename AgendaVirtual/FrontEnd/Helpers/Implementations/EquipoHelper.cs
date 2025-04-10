using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class EquipoHelper : IEquipoHelper
    {
        IServiceRepository _serviceRepository;
        public string Token { get; set; }
        public EquipoHelper(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        EquipoViewModel convertir(EquipoAPI equipoAPI)
        {
            return new EquipoViewModel
            {
                IdEquipo = equipoAPI.IdEquipo,
                Nombre = equipoAPI.Nombre,
                Descripcion = equipoAPI.Descripcion,
                FechaCreacion = equipoAPI.FechaCreacion
            };
        }

        UsuarioViewModel convertir(UsuarioAPI usuarioAPI)
        {
            return new UsuarioViewModel
            {
                IdUsuario = usuarioAPI.IdUsuario,
                Nombre = usuarioAPI.Nombre
            };
        }
        public List<EquipoViewModel> getEquipos()
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/equipo");
            List<EquipoAPI> equipos = new List<EquipoAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipos = JsonConvert.DeserializeObject<List<EquipoAPI>>(content);
            }
            List<EquipoViewModel> resultado = new List<EquipoViewModel>();
            foreach (var equipo in equipos)
            {
                resultado.Add(convertir(equipo));
            }
            return resultado;
        }

        public List<EquipoViewModel> getEquiposPorUsuario(int usuarioId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/Equipo/Usuario/" + usuarioId.ToString());
            List<EquipoAPI> equipos = new List<EquipoAPI>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipos = JsonConvert.DeserializeObject<List<EquipoAPI>>(content);
            }
            List<EquipoViewModel> resultado = new List<EquipoViewModel>();
            foreach (var equipo in equipos)
            {
                resultado.Add(convertir(equipo));
            }
            return resultado;
        }

        public List<UsuarioViewModel> getUsuariosPorEquipo(int equipoId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/Equipo/Equipo/" + equipoId.ToString());
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

        public List<UsuarioViewModel> getUsuariosNotInEquipo(int equipoId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/Equipo/UsuariosNotIn/" + equipoId.ToString());
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

        public EquipoViewModel getEquipo(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/equipo/" + id.ToString());
            EquipoAPI equipo = new EquipoAPI();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoAPI>(content);
            }
            EquipoViewModel resultado = convertir(equipo);
            return resultado;
        }
        public EquipoViewModel addEquipo(EquipoViewModel equipo)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PostResponse("api/equipo", equipo);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoViewModel>(content);
            }
            return equipo;
        }
        public EquipoViewModel updateEquipo(EquipoViewModel equipo)
        {

            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PutResponse("api/equipo", equipo);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoViewModel>(content);
            }
            return equipo;
        }
        public void deleteEquipo(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.DeleteResponse($"api/equipo/{id}");

        }

        public EquipoViewModel GetEquipoPorUsuario(int usuarioId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _serviceRepository.GetResponse("api/Equipo/EquipoPorUsuario/" + usuarioId.ToString());
            EquipoAPI equipo = new EquipoAPI();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoAPI>(content);
            }
            EquipoViewModel resultado = convertir(equipo);
            return resultado;
        }

    }
}