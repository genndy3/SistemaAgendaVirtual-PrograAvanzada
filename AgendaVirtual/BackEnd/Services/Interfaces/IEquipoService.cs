using BackEnd.DTO;

namespace BackEnd.Services.Interfaces
{
    public interface IEquipoService
    {
        EquipoDTO Get(int id);
        List<EquipoDTO> GetAll();
        EquipoDTO Add(EquipoDTO equipoDTO);
        EquipoDTO Update(EquipoDTO equipoDTO);
        EquipoDTO Delete(int id);
    }
}
