namespace ServiceManagementApi.DTOs.ServiceRequests
{
    public class ServiceRequestDto
    {
        public int ServiceRequestId { get; set; }
        public required string ServiceName { get; set; }
        public required string CustomerName { get; set; }
        public required string Status { get; set; }
        public required string Priority { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }
}