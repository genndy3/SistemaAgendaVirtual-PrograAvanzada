using BackEnd.DTO;
using DAL.Implementations;

namespace BackEnd.Services.Interfaces
{
    public interface IRecordatorioService
    {
        RecordatorioDTO Get(int id);
        List<RecordatorioDTO> GetAll();
        RecordatorioDTO Add(RecordatorioDTO recordatorioDTO);
        RecordatorioDTO Update(RecordatorioDTO recordatorioDTO);
        RecordatorioDTO Delete(int id);
    }
}
