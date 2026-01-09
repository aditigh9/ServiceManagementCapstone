using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApi.Models
{
    public class Technician
{
    public int TechnicianId { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string Name { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;
}
}
