using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApi.Models
{
    public class ServiceRequest
    {
        [Key]
        public int ServiceRequestId { get; set; }

        //  Foreign Key → Service
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        //  Foreign Key → User
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;

        public string Status { get; set; } = "Requested";
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
         public string Priority { get; set; } = "Medium";
         public DateTime? ScheduledAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
