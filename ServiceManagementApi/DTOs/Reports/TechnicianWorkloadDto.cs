namespace ServiceManagementApi.Reports
{
    public class TechnicianWorkloadDto
    {
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; } = string.Empty;
        public int AssignedRequests { get; set; }
        public int CompletedRequests { get; set; }
    }
}
