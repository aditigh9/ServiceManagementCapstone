namespace ServiceManagementApi.DTOs.Service;

public class ServiceDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal ServiceCharge { get; set; }
    public int ServiceCategoryId { get; set; }
}
