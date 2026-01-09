using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagementApi.Data;
using ServiceManagementApi.DTOs;
using ServiceManagementApi.Helpers;
using ServiceManagementApi.Models;

namespace ServiceManagementApi.Controllers;

[ApiController]
[Route("api/servicecategories")]
public class ServiceCategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServiceCategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // All users – get active categories
    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _context.ServiceCategories
            .Where(c => c.IsActive)
            .Select(c => new
            {
                c.ServiceCategoryId,
                c.Name,
                c.Description
            })
            .ToList();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Categories fetched",
            Data = categories
        });
    }

    // Create category
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Create(ServiceCategoryDto dto)
    {
        var category = new ServiceCategory
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.ServiceCategories.Add(category);
        _context.SaveChanges();

        return Ok(new ApiResponse<ServiceCategory>
        {
            Success = true,
            Message = "Service category created successfully",
            Data = category
        });
    }

    // Update category
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, ServiceCategoryDto dto)
    {
        var category = _context.ServiceCategories.Find(id);
        if (category == null)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Service category not found"
            });
        }

        category.Name = dto.Name;
        category.Description = dto.Description;

        _context.SaveChanges();

        return Ok(new ApiResponse<ServiceCategory>
        {
            Success = true,
            Message = "Service category updated successfully",
            Data = category
        });
    }

    // Soft delete category
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var category = _context.ServiceCategories.Find(id);
        if (category == null)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Service category not found"
            });
        }

        category.IsActive = false;
        _context.SaveChanges();

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Service category deleted successfully",
            Data = id
        });
    }
}
