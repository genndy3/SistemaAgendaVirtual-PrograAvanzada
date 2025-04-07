using FrontEnd.APIModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class UsuarioHelper : IUsuarioHelper
    {
        IServiceRepository _repository;

        public UsuarioHelper(IServiceRepository repository)
        {
            _repository = repository;
        }

        UsuarioViewModel Convertir(UsuarioAPI usuario)
        {
            return new UsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Contrasena = usuario.Contrasena,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }
        UsuarioAPI Convertir(UsuarioViewModel usuario)
        {
            return new UsuarioAPI
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Contrasena = usuario.Contrasena,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }
        public UsuarioViewModel AddUsuario(UsuarioViewModel usuario)
        {
            HttpResponseMessage responseMessage = _repository.PostResponse("api/usuario", Convertir(usuario));
            if (responseMessage != null)
            {
                var content = responseMessage.Content;
            }


            return usuario;
        }

        public List<UsuarioViewModel> GetAll()
        {
           List<UsuarioAPI> usuarios = new List<UsuarioAPI>();
           HttpResponseMessage responseMessage = _repository.GetResponse("api/usuario");


            if (responseMessage != null)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                usuarios = JsonConvert.DeserializeObject<List<UsuarioAPI>>(content);
            }
            List<UsuarioViewModel> list = new List<UsuarioViewModel>();
            foreach (var item in usuarios)
            {
                list.Add(Convertir(item));
            }

            return list;

        }

        public UsuarioViewModel UpdateUsuario(UsuarioViewModel usuario)
        {
            HttpResponseMessage responseMessage = _repository.PutResponse($"api/usuario/{usuario.IdUsuario}", Convertir(usuario));
            if (responseMessage != null)
            {
                var content = responseMessage.Content;
            }
            return usuario;
        }

        public UsuarioViewModel GetById(int id)
        {
            UsuarioAPI usuario = new UsuarioAPI();
            HttpResponseMessage responseMessage = _repository.GetResponse("api/Usuario/" + id.ToString());

            if (responseMessage != null)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                usuario = JsonConvert.DeserializeObject<UsuarioAPI>(content);
            }
            return Convertir(usuario);
        }

        void IUsuarioHelper.DeleteUsuario(int id)
        {
            throw new NotImplementedException();
        }
    }
    }
