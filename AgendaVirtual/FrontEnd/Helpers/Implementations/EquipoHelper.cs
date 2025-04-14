using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;

namespace FrontEnd.Helpers.Implementations
{
    public class EquipoHelper : IEquipoHelper
    {
        IServiceRepository _repository;
        public string Token { get; set; }

        public EquipoHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        EquipoViewModel Convertir(EquipoAPI equipoAPI)
        {
            return new EquipoViewModel
            {
                IdEquipo = equipoAPI.IdEquipo,
                Nombre = equipoAPI.Nombre,
                Descripcion = equipoAPI.Descripcion,
                FechaCreacion = equipoAPI.FechaCreacion
            };
        }

        EquipoAPI Convertir(EquipoViewModel equipo)
        {
            return new EquipoAPI
            {
                IdEquipo = equipo.IdEquipo,
                Nombre = equipo.Nombre,
                Descripcion = equipo.Descripcion,
                FechaCreacion = equipo.FechaCreacion
            };
        }

        UsuarioViewModel Convertir(UsuarioAPI usuarioAPI)
        {
            return new UsuarioViewModel
            {
                IdUsuario = usuarioAPI.IdUsuario,
                Nombre = usuarioAPI.Nombre
            };
        }


        public List<EquipoViewModel> GetEquipos()
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/equipo");
            List<EquipoAPI> equipos = new List<EquipoAPI>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipos = JsonConvert.DeserializeObject<List<EquipoAPI>>(content);
            }

            List<EquipoViewModel> resultado = new List<EquipoViewModel>();
            foreach (var equipo in equipos)
            {
                resultado.Add(Convertir(equipo));
            }
            return resultado;
        }

        public EquipoViewModel Add(EquipoViewModel equipo)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PostResponse("api/equipo", Convertir(equipo));

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                equipo = Convertir(JsonConvert.DeserializeObject<EquipoAPI>(content));
            }
            return equipo;
        }

        public bool DeleteEquipo(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.DeleteResponse($"api/equipo/{id}");
            return response.IsSuccessStatusCode;
        }

        public EquipoViewModel EditEquipo(EquipoViewModel equipo)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PutResponse("api/equipo", Convertir(equipo));

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                equipo = Convertir(JsonConvert.DeserializeObject<EquipoAPI>(content));
            }
            return equipo;
        }

        public EquipoViewModel GetById(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/equipo/{id}");
            EquipoAPI equipo = new EquipoAPI();

            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoAPI>(content);
            }
            return Convertir(equipo);
        }

        public List<EquipoViewModel> GetEquiposPorUsuario(int usuarioId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/Equipo/Usuario/{usuarioId}");
            List<EquipoAPI> equipos = new List<EquipoAPI>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipos = JsonConvert.DeserializeObject<List<EquipoAPI>>(content);
            }

            List<EquipoViewModel> resultado = new List<EquipoViewModel>();
            foreach (var equipo in equipos)
            {
                resultado.Add(Convertir(equipo));
            }
            return resultado;
        }

        public List<UsuarioViewModel> GetUsuariosPorEquipo(int equipoId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/Equipo/Equipo/{equipoId}");
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

        public List<UsuarioViewModel> GetUsuariosNotInEquipo(int equipoId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/Equipo/UsuariosNotIn/{equipoId}");
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

        public EquipoViewModel GetEquipoPorUsuario(int usuarioId)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse($"api/Equipo/EquipoPorUsuario/{usuarioId}");
            EquipoAPI equipo = new EquipoAPI();

            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoAPI>(content);
            }
            return Convertir(equipo);
        }

        public EquipoViewModel addEquipo(EquipoViewModel equipo)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PostResponse("api/equipo", equipo);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoViewModel>(content);
            }
            return equipo;
        }
        public EquipoViewModel updateEquipo(EquipoViewModel equipo)
        {

            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = _repository.PutResponse("api/equipo", equipo);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoViewModel>(content);
            }
            return equipo;
        }

        public EquipoViewModel getEquipo(int id)
        {
            _repository.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage responseMessage = _repository.GetResponse("api/equipo/" + id.ToString());
            EquipoAPI equipo = new EquipoAPI();
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipo = JsonConvert.DeserializeObject<EquipoAPI>(content);
            }
            EquipoViewModel resultado = Convertir(equipo);
            return resultado;
        }

    }
}