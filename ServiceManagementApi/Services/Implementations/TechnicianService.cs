using ServiceManagementApi.Data;
using ServiceManagementApi.DTOs.Technician;
using ServiceManagementApi.Models;
using ServiceManagementApi.Services.Interfaces;

namespace ServiceManagementApi.Services.Implementations
{
    public class TechnicianService : ITechnicianService
    {
        private readonly ApplicationDbContext _context;

        public TechnicianService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Technician> GetAll()
        {
            return _context.Technicians.ToList();
        }

        public IEnumerable<Technician> GetAvailableTechnicians()
        {
            return _context.Technicians
                .Where(t => t.IsActive && t.IsAvailable)
                .ToList();
        }

        public Technician GetById(int id)
        {
            return _context.Technicians.FirstOrDefault(t => t.TechnicianId == id)
                   ?? throw new Exception("Technician not found");
        }

        public void Create(TechnicianDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId)
                       ?? throw new Exception("User not found");

            if (user.Role != "Technician")
                throw new Exception("Only users with Technician role can be created as technicians");

            if (_context.Technicians.Any(t => t.UserId == dto.UserId))
                throw new Exception("Technician already exists for this user");

            var technician = new Technician
            {
                Name = dto.Name,
                UserId = dto.UserId,
                IsActive = true,
                IsAvailable = true
            };

            _context.Technicians.Add(technician);
            _context.SaveChanges();
        }

        public void Update(int id, TechnicianDto dto)
        {
            var technician = GetById(id);

            var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId)
                       ?? throw new Exception("User not found");

            if (user.Role != "Technician")
                throw new Exception("Only users with Technician role can be assigned");

            technician.Name = dto.Name;
            technician.UserId = dto.UserId;

            _context.SaveChanges();
        }

        public void ToggleStatus(int id)
        {
            var technician = GetById(id);
            technician.IsActive = !technician.IsActive;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var technician = GetById(id);

            bool hasAssignments = _context.TechnicianAssignments
                .Any(a => a.TechnicianId == id);

            if (hasAssignments)
                throw new Exception("Cannot delete technician with assignments");

            technician.IsActive = false;
            technician.IsAvailable = false;
            _context.SaveChanges();
        }

        public void MarkBusy(int technicianId)
        {
            var technician = GetById(technicianId);
            technician.IsAvailable = false;
            _context.SaveChanges();
        }

        public void MarkAvailable(int technicianId)
        {
            var technician = GetById(technicianId);
            technician.IsAvailable = true;
            _context.SaveChanges();
        }

        public void AssignTechnician(AssignTechnicianDto dto)
        {
            var request = _context.ServiceRequests
                .FirstOrDefault(sr => sr.ServiceRequestId == dto.ServiceRequestId)
                ?? throw new Exception("Service request not found");

            if (request.Status == "Cancelled" || request.Status == "Closed")
                throw new Exception("Technician cannot be assigned to cancelled or closed requests");

            var technician = _context.Technicians
                .FirstOrDefault(t =>
                    t.TechnicianId == dto.TechnicianId &&
                    t.IsActive &&
                    t.IsAvailable)
                ?? throw new Exception("Technician not available");

            if (_context.TechnicianAssignments.Any(a =>
                a.ServiceRequestId == dto.ServiceRequestId))
                throw new Exception("Technician already assigned");

            _context.TechnicianAssignments.Add(new TechnicianAssignment
            {
                ServiceRequestId = dto.ServiceRequestId,
                TechnicianId = dto.TechnicianId,
                AssignedAt = DateTime.UtcNow
            });

            request.Status = "Assigned";
            technician.IsAvailable = false;

            _context.SaveChanges();
        }
    }
}
