using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApi.Models;

public class Service
{
    [Key]
    public int ServiceId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    
    [Column(TypeName = "decimal(10,2)")]
    public decimal ServiceCharge { get; set; }

    public bool IsActive { get; set; } = true;

    public int ServiceCategoryId { get; set; }
    public ServiceCategory? ServiceCategory { get; set; }
}
