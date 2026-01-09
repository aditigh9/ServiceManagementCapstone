using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagementApi.DTOs.Technician;
using ServiceManagementApi.Services.Interfaces;

namespace ServiceManagementApi.Controllers
{
    [ApiController]
    [Route("api/technicians")]
    public class TechnicianController : ControllerBase
    {
        private readonly ITechnicianService _service;

        public TechnicianController(ITechnicianService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,ServiceManager")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [Authorize(Roles = "ServiceManager")]
        [HttpGet("available")]
        public IActionResult GetAvailable()
        {
            return Ok(_service.GetAvailableTechnicians());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(TechnicianDto dto)
        {
            _service.Create(dto);
            return Ok("Technician created");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, TechnicianDto dto)
        {
            _service.Update(id, dto);
            return Ok("Technician updated");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/toggle")]
        public IActionResult ToggleStatus(int id)
        {
            _service.ToggleStatus(id);
            return Ok("Technician status changed");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok("Technician deleted");
        }

        [Authorize(Roles = "ServiceManager")]
        [HttpPost("assign")]
        public IActionResult AssignTechnician(AssignTechnicianDto dto)
        {
            try
            {
                _service.AssignTechnician(dto);
                return Ok(new { message = "Technician assigned successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

    }
}
