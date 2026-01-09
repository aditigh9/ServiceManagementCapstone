using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApi.Data;
using ServiceManagementApi.DTOs.Service;
using ServiceManagementApi.Models;

namespace ServiceManagementApi.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // View services (all authenticated users)
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var services = await _context.Services
            .Where(s => s.IsActive)
            .Select(s => new
            {
                s.ServiceId,
                s.Name,
                s.Description,
                s.ServiceCharge,
                s.ServiceCategoryId,
                CategoryName = s.ServiceCategory!.Name
            })
            .ToListAsync();

        return Ok(services);
    }

    // Admin only - Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ServiceDto dto)
    {
        if (!await _context.ServiceCategories.AnyAsync(c => c.ServiceCategoryId == dto.ServiceCategoryId))
            return BadRequest("Invalid Service Category");

        var service = new Service
        {
            Name = dto.Name,
            Description = dto.Description,
            ServiceCharge = dto.ServiceCharge,
            ServiceCategoryId = dto.ServiceCategoryId
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return Ok(service);
    }

    //  Admin only - Update
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, ServiceDto dto)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return NotFound();

        service.Name = dto.Name;
        service.Description = dto.Description;
        service.ServiceCharge = dto.ServiceCharge;
        service.ServiceCategoryId = dto.ServiceCategoryId;

        await _context.SaveChangesAsync();
        return Ok(service);
    }

    // Admin only - Soft delete
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return NotFound();

        service.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok("Service deleted");
    }
}
