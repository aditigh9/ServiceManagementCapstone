using ServiceManagementApi.Data;
using ServiceManagementApi.DTOs.ServiceRequests;
using ServiceManagementApi.Models;
using ServiceManagementApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ServiceManagementApi.Services.Implementations
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITechnicianService _technicianService;

        public ServiceRequestService(
            ApplicationDbContext context,
            ITechnicianService technicianService)
        {
            _context = context;
            _technicianService = technicianService;
        }

        // ---------------- CUSTOMER: CREATE ----------------
        public void CreateServiceRequest(CreateServiceRequestDto dto, int userId)
        {
            var service = _context.Services.Find(dto.ServiceId)
                ?? throw new Exception("Service not found");

            var request = new ServiceRequest
            {
                ServiceId = service.ServiceId,
                UserId = userId,
                Status = "Requested",
                RequestedAt = DateTime.UtcNow
            };

            _context.ServiceRequests.Add(request);
            _context.SaveChanges();
        }

        // ---------------- TECHNICIAN: UPDATE STATUS ----------------
        public void UpdateStatus(int serviceRequestId, int technicianUserId, string status)
        {
            var technician = _context.Technicians
                .FirstOrDefault(t => t.UserId == technicianUserId)
                ?? throw new Exception("Technician not found");

            var assignment = _context.TechnicianAssignments
                .FirstOrDefault(a =>
                    a.ServiceRequestId == serviceRequestId &&
                    a.TechnicianId == technician.TechnicianId)
                ?? throw new Exception("You are not assigned to this request");

            var request = _context.ServiceRequests
                .Include(sr => sr.Service)
                .FirstOrDefault(sr => sr.ServiceRequestId == serviceRequestId)
                ?? throw new Exception("Service request not found");

            if (request.ScheduledAt == null)
                throw new Exception("This task is not scheduled yet");

            request.Status = status;

            if (status == "InProgress")
            {
                _technicianService.MarkBusy(technician.TechnicianId);
            }

            if (status == "Completed")
            {
                _technicianService.MarkAvailable(technician.TechnicianId);

                _context.TechnicianAssignments.Remove(assignment);

                if (!_context.Invoices.Any(i => i.RequestId == serviceRequestId))
                {
                    _context.Invoices.Add(new Invoice
                    {
                        RequestId = serviceRequestId,
                        Amount = request.Service.ServiceCharge,
                        PaymentStatus = "Pending",
                        GeneratedDate = DateTime.UtcNow
                    });
                }
            }

            _context.SaveChanges();
        }

        // ---------------- SERVICE MANAGER: CLOSE ----------------
        public void CloseRequest(int serviceRequestId)
        {
            var request = _context.ServiceRequests
                .FirstOrDefault(sr => sr.ServiceRequestId == serviceRequestId)
                ?? throw new Exception("Service request not found");

            if (request.Status != "Completed")
                throw new Exception("Only completed requests can be closed");

            request.Status = "Closed";
            request.ClosedAt = DateTime.UtcNow;

            var assignment = _context.TechnicianAssignments
                .FirstOrDefault(a => a.ServiceRequestId == serviceRequestId);

            if (assignment != null)
            {
                _technicianService.MarkAvailable(assignment.TechnicianId);
                _context.TechnicianAssignments.Remove(assignment);
            }

            _context.SaveChanges();
        }

        // ---------------- CUSTOMER: CANCEL ----------------
        public void CancelRequest(int serviceRequestId, int userId)
        {
            var request = _context.ServiceRequests
                .FirstOrDefault(sr =>
                    sr.ServiceRequestId == serviceRequestId &&
                    sr.UserId == userId)
                ?? throw new Exception("Service request not found");

            if (request.Status == "Completed" || request.Status == "Closed")
                throw new Exception("Completed or closed requests cannot be cancelled");

            var assignment = _context.TechnicianAssignments
                .FirstOrDefault(a => a.ServiceRequestId == serviceRequestId);

            if (assignment != null)
            {
                _technicianService.MarkAvailable(assignment.TechnicianId);
                _context.TechnicianAssignments.Remove(assignment);
            }

            request.Status = "Cancelled";
            _context.SaveChanges();
        }

        // ---------------- SERVICE MANAGER: RESCHEDULE ----------------
        public void RescheduleRequest(int serviceRequestId, DateTime scheduledAt)
        {
            var request = _context.ServiceRequests
                .FirstOrDefault(sr => sr.ServiceRequestId == serviceRequestId)
                ?? throw new Exception("Service request not found");

            request.ScheduledAt = scheduledAt;
            _context.SaveChanges();
        }

        // ---------------- SERVICE MANAGER: PRIORITY ----------------
        public void SetPriorityAndSchedule(int serviceRequestId, string priority, DateTime scheduledAt)
        {
            var request = _context.ServiceRequests
                .FirstOrDefault(sr => sr.ServiceRequestId == serviceRequestId)
                ?? throw new Exception("Service request not found");

            request.Priority = priority;
            request.ScheduledAt = scheduledAt;
            request.Status = "Assigned";

            _context.SaveChanges();
        }

        // ================= GET METHODS =================

        public IEnumerable<ServiceRequestDto> GetAll()
        {
            return _context.ServiceRequests
                .Include(sr => sr.Service)
                .Include(sr => sr.User)
                .OrderByDescending(sr => sr.RequestedAt)
                .Select(sr => new ServiceRequestDto
                {
                    ServiceRequestId = sr.ServiceRequestId,
                    ServiceName = sr.Service.Name,
                    CustomerName = sr.User.FullName,
                    Status = sr.Status,
                    Priority = sr.Priority,
                    RequestedAt = sr.RequestedAt,
                    ScheduledAt = sr.ScheduledAt
                })
                .ToList();
        }

        public IEnumerable<ServiceRequestDto> GetForCustomer(int userId)
        {
            return _context.ServiceRequests
                .Include(sr => sr.Service)
                .Include(sr => sr.User)
                .Where(sr => sr.UserId == userId)
                .OrderByDescending(sr => sr.RequestedAt)
                .Select(sr => new ServiceRequestDto
                {
                    ServiceRequestId = sr.ServiceRequestId,
                    ServiceName = sr.Service.Name,
                    CustomerName = sr.User.FullName,
                    Status = sr.Status,
                    Priority = sr.Priority,
                    RequestedAt = sr.RequestedAt,
                    ScheduledAt = sr.ScheduledAt
                })
                .ToList();
        }

        public IEnumerable<ServiceRequestDto> GetForTechnician(int technicianUserId)
        {
            var technicianId = _context.Technicians
                .Where(t => t.UserId == technicianUserId)
                .Select(t => t.TechnicianId)
                .FirstOrDefault();

            return _context.TechnicianAssignments
                .Where(a => a.TechnicianId == technicianId)
                .Include(a => a.ServiceRequest)
                    .ThenInclude(sr => sr.Service)
                .Include(a => a.ServiceRequest)
                    .ThenInclude(sr => sr.User)
                .Where(a =>
                    a.ServiceRequest.Status != "Cancelled" &&
                    a.ServiceRequest.Status != "Closed")
                .Select(a => new ServiceRequestDto
                {
                    ServiceRequestId = a.ServiceRequest.ServiceRequestId,
                    ServiceName = a.ServiceRequest.Service.Name,
                    CustomerName = a.ServiceRequest.User.FullName,
                    Status = a.ServiceRequest.Status,
                    Priority = a.ServiceRequest.Priority,
                    RequestedAt = a.ServiceRequest.RequestedAt,
                    ScheduledAt = a.ServiceRequest.ScheduledAt
                })
                .OrderByDescending(dto => dto.RequestedAt)
                .ToList();
        }

        public ServiceRequestDashboardDto GetDashboardSummary()
        {
            return new ServiceRequestDashboardDto
            {
                Total = _context.ServiceRequests.Count(),
                Requested = _context.ServiceRequests.Count(r => r.Status == "Requested"),
                Assigned = _context.ServiceRequests.Count(r => r.Status == "Assigned"),
                InProgress = _context.ServiceRequests.Count(r => r.Status == "InProgress"),
                Completed = _context.ServiceRequests.Count(r => r.Status == "Completed"),
                Closed = _context.ServiceRequests.Count(r => r.Status == "Closed"),
                Cancelled = _context.ServiceRequests.Count(r => r.Status == "Cancelled")
            };
        }

        // ---------------- SERVICE MANAGER: ASSIGN TECHNICIAN ----------------
        public void AssignTechnician(int serviceRequestId, int technicianId)
        {
            var request = _context.ServiceRequests
                .FirstOrDefault(r => r.ServiceRequestId == serviceRequestId)
                ?? throw new Exception("Request not found");

            if (request.Status == "Cancelled")
                throw new Exception("This request has been cancelled by the customer");

            if (_context.TechnicianAssignments.Any(a =>
                a.ServiceRequestId == serviceRequestId))
                throw new Exception("Technician already assigned");

            _context.TechnicianAssignments.Add(new TechnicianAssignment
            {
                ServiceRequestId = serviceRequestId,
                TechnicianId = technicianId,
                AssignedAt = DateTime.UtcNow
            });

            request.Status = "Assigned";
            _technicianService.MarkBusy(technicianId);

            _context.SaveChanges();
        }
    }
}
