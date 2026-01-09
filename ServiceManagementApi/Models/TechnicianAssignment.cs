using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApi.Models
{
    public class TechnicianAssignment
    {
        [Key]
        public int TechnicianAssignmentId { get; set; }

        // FK → ServiceRequest
        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; } = null!;

        // FK → Technician
        public int TechnicianId { get; set; }
        public Technician Technician { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
