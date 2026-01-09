using ServiceManagementApi.DTOs.ServiceRequests;

namespace ServiceManagementApi.Services.Interfaces
{
    public interface IServiceRequestService
    {
        IEnumerable<ServiceRequestDto> GetAll();
        IEnumerable<ServiceRequestDto> GetForCustomer(int userId);
        IEnumerable<ServiceRequestDto> GetForTechnician(int technicianUserId);
        ServiceRequestDashboardDto GetDashboardSummary();


        void CreateServiceRequest(CreateServiceRequestDto dto, int userId);
        void AssignTechnician(int serviceRequestId, int technicianId);

        void UpdateStatus(int serviceRequestId, int technicianUserId, string status);
        void CloseRequest(int serviceRequestId);
        void CancelRequest(int serviceRequestId, int userId);
        void RescheduleRequest(int serviceRequestId, DateTime scheduledAt);
        void SetPriorityAndSchedule(int serviceRequestId, string priority, DateTime scheduledAt);
    }
}
