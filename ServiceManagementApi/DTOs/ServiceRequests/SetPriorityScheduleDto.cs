namespace ServiceManagementApi.DTOs.ServiceRequests
{
    public class SetPriorityScheduleDto
    {
        public string Priority { get; set; } = "Medium"; // Low | Medium | High
        public DateTime ScheduledAt { get; set; }
    }
}
