namespace ServiceManagementApi.DTOs.ServiceRequests
{
    public class ServiceRequestDashboardDto
    {
        public int Total { get; set; }
        public int Requested { get; set; }
        public int Assigned { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int Closed { get; set; }
        public int Cancelled { get; set; }
    }
}
