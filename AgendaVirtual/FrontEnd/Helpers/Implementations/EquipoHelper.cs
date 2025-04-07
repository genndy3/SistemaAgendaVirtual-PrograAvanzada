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

        public EquipoHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        EquipoViewModel Convertir (EquipoAPI equipo)
        {
            return new EquipoViewModel
            {
                IdEquipo = equipo.IdEquipo,
                Nombre = equipo.Nombre,
                Descripcion = equipo.Descripcion,
                FechaCreacion = equipo.FechaCreacion
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

        public EquipoViewModel Add(EquipoViewModel equipo)
        {
            HttpResponseMessage response = _repository.PostResponse("api/equipo", equipo);
            if (response.IsSuccessStatusCode)
            {

                var content = response.Content.ReadAsStringAsync().Result;
            }
            return equipo;
        }

        public void DeleteEquipo(int id)
        {
            throw new NotImplementedException();
        }

        public EquipoViewModel EditEquipo(EquipoViewModel EquipoViewModel)
        {
            throw new NotImplementedException();
        }

        public EquipoViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<EquipoViewModel> GetEquipos()
        {
            List<EquipoAPI> equipos = new List<EquipoAPI>();
            HttpResponseMessage responseMessage = _repository.GetResponse("api/equipo");

            if (responseMessage != null)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                equipos = JsonConvert.DeserializeObject<List<EquipoAPI>>(content);
            }

            List<EquipoViewModel> list = new List<EquipoViewModel>();

            foreach (var item in equipos)
            {
                list.Add(Convertir(item));
            }
            return list;
        }
    }
}
