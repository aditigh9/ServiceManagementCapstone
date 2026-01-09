using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagementApi.DTOs.ServiceRequests;
using ServiceManagementApi.Helpers;
using ServiceManagementApi.Services.Interfaces;
using System.Security.Claims;
using ServiceManagementApi.DTOs.Technician;


namespace ServiceManagementApi.Controllers
{
    [ApiController]
    [Route("api/servicerequests")]
    [Authorize]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestService _service;

        public ServiceRequestsController(IServiceRequestService service)
        {
            _service = service;
        }

        // GET: ALL (Admin / ServiceManager)
        [HttpGet]
        [Authorize(Roles = "Admin,ServiceManager")]
        public IActionResult GetAll()
        {
            var data = _service.GetAll();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service requests fetched",
                Data = data
            });
        }

        // GET: CUSTOMER
        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public IActionResult GetMyRequests()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var data = _service.GetForCustomer(userId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Customer service requests fetched",
                Data = data
            });
        }

        // GET: TECHNICIAN
        [HttpGet("assigned")]
        [Authorize(Roles = "Technician")]
        public IActionResult GetAssigned()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var data = _service.GetForTechnician(userId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Assigned service requests fetched",
                Data = data
            });
        }

        // CREATE
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult Create(CreateServiceRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _service.CreateServiceRequest(dto, userId);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Service request created"
            });
        }

        [Authorize(Roles = "ServiceManager")]
        [HttpPost("assign")]
        public IActionResult Assign(AssignTechnicianDto dto)
        {
            _service.AssignTechnician(dto.ServiceRequestId, dto.TechnicianId);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Technician assigned successfully"
            });

        }


        // UPDATE STATUS (Technician)
        [Authorize(Roles = "Technician")]
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, UpdateServiceRequestStatusDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _service.UpdateStatus(id, userId, dto.Status);

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Status updated",
                Data = id
            });
        }

        // SET PRIORITY
        [Authorize(Roles = "ServiceManager")]
        [HttpPut("{id}/priority")]
        public IActionResult SetPriority(int id, SetPriorityScheduleDto dto)
        {
            _service.SetPriorityAndSchedule(id, dto.Priority, dto.ScheduledAt);

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Priority set",
                Data = id
            });
        }

        // CLOSE REQUEST
        [Authorize(Roles = "ServiceManager")]
        [HttpPut("{id}/close")]
        public IActionResult Close(int id)
        {
            _service.CloseRequest(id);

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Request closed",
                Data = id
            });
        }

        // CANCEL REQUEST
        [Authorize(Roles = "Customer")]
        [HttpPut("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _service.CancelRequest(id, userId);

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Request cancelled",
                Data = id
            });
        }

        // RESCHEDULE
        [Authorize(Roles = "ServiceManager")]
        [HttpPut("{id}/reschedule")]
        public IActionResult Reschedule(int id, RescheduleServiceRequestDto dto)
        {
            _service.RescheduleRequest(id, dto.ScheduledAt);

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Request rescheduled",
                Data = id
            });
        }

        // DASHBOARD SUMMARY 
        [HttpGet("dashboard-summary")]
        [Authorize(Roles = "Admin,ServiceManager")]
        public IActionResult GetDashboardSummary()
        {
            var data = _service.GetDashboardSummary();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dashboard summary fetched",
                Data = data
            });
        }


    }
}
