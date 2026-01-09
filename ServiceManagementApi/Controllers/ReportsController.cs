using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApi.Data;
using ServiceManagementApi.Helpers;
using ServiceManagementApi.Reports;

namespace ServiceManagementApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize(Roles = "Admin,ServiceManager")]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("service-count-by-status")]
        public async Task<IActionResult> ServiceCountByStatus()
        {
            var result = await _context.ServiceRequests
                .GroupBy(s => s.Status)
                .Select(g => new ServiceStatusReportDto
                {
                    Status = g.Key!,
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service count by status fetched",
                Data = result
            });
        }

        [HttpGet("service-count-by-category")]
        public async Task<IActionResult> ServiceCountByCategory()
        {
            var result = await _context.ServiceRequests
                .Include(sr => sr.Service)
                .ThenInclude(s => s.ServiceCategory)
                .Where(sr => sr.Service != null && sr.Service.ServiceCategory != null)
                .GroupBy(sr => sr.Service!.ServiceCategory!.Name)
                .Select(g => new ServiceCategoryReportDto
                {
                    CategoryName = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service count by category fetched",
                Data = result
            });
        }

        [HttpGet("technician-workload")]
        public async Task<IActionResult> TechnicianWorkload()
        {
            var result = await _context.Technicians
                .Select(t => new TechnicianWorkloadDto
                {
                    TechnicianId = t.TechnicianId,
                    TechnicianName = t.Name,
                    AssignedRequests = _context.TechnicianAssignments
                        .Count(a => a.TechnicianId == t.TechnicianId),
                    CompletedRequests = _context.TechnicianAssignments
                        .Count(a =>
                            a.TechnicianId == t.TechnicianId &&
                            a.ServiceRequest.Status == "Completed")
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Technician workload fetched",
                Data = result
            });
        }

        [HttpGet("average-resolution-time")]
        public async Task<IActionResult> AverageResolutionTime()
        {
            var query = _context.ServiceRequests
                .Where(sr => sr.ClosedAt != null)
                .Select(sr =>
                    EF.Functions.DateDiffHour(
                        sr.RequestedAt,
                        sr.ClosedAt!.Value));

            double avgHours = 0;

            if (await query.AnyAsync())
            {
                avgHours = Math.Round(await query.AverageAsync(), 2);
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Average resolution time calculated",
                Data = new
                {
                    AverageResolutionTimeInHours = avgHours
                }
            });
        }


        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> MonthlyRevenue()
        {
            var result = await _context.Invoices
                .GroupBy(i => new { i.GeneratedDate.Year, i.GeneratedDate.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(x => x.Amount)
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Monthly revenue fetched",
                Data = result
            });
        }
    }
}
