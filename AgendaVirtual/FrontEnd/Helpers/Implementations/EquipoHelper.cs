using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FrontEnd.Helpers.Implementations
{
    public class EquipoHelper : IEquipoHelper
    {
        private readonly IServiceRepository _serviceRepository;
        public string Token { get; set; }

        public EquipoHelper(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        private EquipoViewModel Convertir(EquipoAPI equipo) =>
            new EquipoViewModel
            {
                IdEquipo = equipo.IdEquipo,
                Nombre = equipo.Nombre,
                Descripcion = equipo.Descripcion,
                FechaCreacion = equipo.FechaCreacion
            };

        private UsuarioViewModel Convertir(UsuarioAPI usuario) =>
            new UsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre
            };

        private EquipoAPI Convertir(EquipoViewModel equipo) =>
            new EquipoAPI
            {
                IdEquipo = equipo.IdEquipo,
                Nombre = equipo.Nombre,
                Descripcion = equipo.Descripcion,
                FechaCreacion = equipo.FechaCreacion
            };

        public List<EquipoViewModel> getEquipos()
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.GetResponse("api/equipo");

            List<EquipoAPI> equipos = new();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                equipos = JsonConvert.DeserializeObject<List<EquipoAPI>>(content)!;
            }

            return equipos.Select(Convertir).ToList();
        }

        public List<EquipoViewModel> getEquiposPorUsuario(int usuarioId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.GetResponse($"api/Equipo/Usuario/{usuarioId}");

            List<EquipoAPI> equipos = new();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                equipos = JsonConvert.DeserializeObject<List<EquipoAPI>>(content)!;
            }

            return equipos.Select(Convertir).ToList();
        }

        public List<UsuarioViewModel> getUsuariosPorEquipo(int equipoId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.GetResponse($"api/Equipo/Equipo/{equipoId}");

            List<UsuarioAPI> usuarios = new();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                usuarios = JsonConvert.DeserializeObject<List<UsuarioAPI>>(content)!;
            }

            return usuarios.Select(Convertir).ToList();
        }

        public List<UsuarioViewModel> getUsuariosNotInEquipo(int equipoId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.GetResponse($"api/Equipo/UsuariosNotIn/{equipoId}");

            List<UsuarioAPI> usuarios = new();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                usuarios = JsonConvert.DeserializeObject<List<UsuarioAPI>>(content)!;
            }

            return usuarios.Select(Convertir).ToList();
        }

        public EquipoViewModel getEquipo(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.GetResponse($"api/equipo/{id}");

            EquipoAPI equipo = new();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoAPI>(content)!;
            }

            return Convertir(equipo);
        }

        public EquipoViewModel addEquipo(EquipoViewModel equipo)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PostResponse("api/equipo", equipo);

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoViewModel>(content)!;
            }

            return equipo;
        }

        public EquipoViewModel updateEquipo(EquipoViewModel equipo)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.PutResponse("api/equipo", equipo);

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoViewModel>(content)!;
            }

            return equipo;
        }

        public void deleteEquipo(int id)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            _serviceRepository.DeleteResponse($"api/equipo/{id}");
        }

        public EquipoViewModel GetEquipoPorUsuario(int usuarioId)
        {
            _serviceRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _serviceRepository.GetResponse($"api/Equipo/EquipoPorUsuario/{usuarioId}");

            EquipoAPI equipo = new();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoAPI>(content)!;
            }

            return Convertir(equipo);
        }
    }
}
