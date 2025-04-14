using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IRecordatorioHelper
    {
        string Token { get; set; }
        List<RecordatorioViewModel> GetRecordatorios();
        RecordatorioViewModel GetRecordatorio(int id);
        RecordatorioViewModel AddRecordatorio(RecordatorioViewModel recordatorio);
        RecordatorioViewModel UpdateRecordatorio(RecordatorioViewModel recordatorio);
        void DeleteRecordatorio(int id);

        List<RecordatorioViewModel> GetRecordatoriosByTarea(int idTarea);
        List<RecordatorioViewModel> GetRecordatoriosNotInTarea(int idTarea);
    }
}
