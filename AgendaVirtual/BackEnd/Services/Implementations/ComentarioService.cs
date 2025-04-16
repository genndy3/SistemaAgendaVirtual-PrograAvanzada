using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Interfaces;
using Entities.Entities;

namespace BackEnd.Services.Implementations
{
    public class ComentarioService : IComentarioService
    {
        IUnidadDeTrabajo _unidadDeTrabajo;
        public ComentarioService(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        ComentarioDTO Convertir (Comentario comentario)
        {
            return new ComentarioDTO
            {
                IdComentario = comentario.IdComentario,
                IdTarea = comentario.IdTarea,
                IdUsuario = comentario.IdUsuario,
                Texto = comentario.Texto,
                FechaHora = comentario.FechaHora
            };
        }

        Comentario Convertir(ComentarioDTO comentarioDTO)
        {
            return new Comentario
            {
                IdComentario = comentarioDTO.IdComentario,
                IdTarea = comentarioDTO.IdTarea,
                IdUsuario = comentarioDTO.IdUsuario,
                Texto = comentarioDTO.Texto,
                FechaHora = comentarioDTO.FechaHora
            };
        }
        public ComentarioDTO Add(ComentarioDTO comentarioDTO)
        {
            _unidadDeTrabajo.comentarioDAL.Add(Convertir(comentarioDTO));
            _unidadDeTrabajo.Complete();
            return comentarioDTO;
        }

        public ComentarioDTO Delete(int id)
        {
           Comentario comentario = new Comentario { IdComentario = id };
            _unidadDeTrabajo.comentarioDAL.Remove(comentario);
            _unidadDeTrabajo.Complete();
            return Convertir(comentario);
        }

        public ComentarioDTO Get(int id)
        {
           var comentario = _unidadDeTrabajo.comentarioDAL.Get(id);
            return Convertir(comentario);
        }

        public List<ComentarioDTO> GetAll()
        {
            var comentarios = _unidadDeTrabajo.comentarioDAL.GetAll();
            List<ComentarioDTO> comentarioDTO = new List<ComentarioDTO>();
            foreach (var comentario in comentarios)
            {
                comentarioDTO.Add(Convertir(comentario));
            }
            return comentarioDTO;
        }

        public ComentarioDTO Update(ComentarioDTO comentarioDTO)
        {
            _unidadDeTrabajo.comentarioDAL.Update(Convertir(comentarioDTO));
            _unidadDeTrabajo.Complete();
            return comentarioDTO;
        }

        public List<ComentarioDTO> GetAllByTarea(int idTarea)
        {
            var comentarios = _unidadDeTrabajo.comentarioDAL.GetComentariosByTarea(idTarea);
            List<ComentarioDTO> comentarioDTO = new List<ComentarioDTO>();
            foreach (var comentario in comentarios)
            {
                comentarioDTO.Add(Convertir(comentario));
            }
            return comentarioDTO;
        }

    }
}
