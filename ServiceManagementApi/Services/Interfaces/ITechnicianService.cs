using ServiceManagementApi.DTOs.Technician;
using ServiceManagementApi.Models;

namespace ServiceManagementApi.Services.Interfaces
{
    public interface ITechnicianService
    {
        IEnumerable<Technician> GetAll();
        Technician GetById(int id);
        void Create(TechnicianDto dto);
        void Update(int id, TechnicianDto dto);
        void ToggleStatus(int id);

        void MarkBusy(int technicianId);
        void MarkAvailable(int technicianId);
        void Delete(int id);
        void AssignTechnician(AssignTechnicianDto dto);
        IEnumerable<Technician> GetAvailableTechnicians();

    }
}
