using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagementApi.Services.Interfaces;

namespace ServiceManagementApi.Controllers
{
    [ApiController]
    [Route("api/billing")]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpGet("{requestId}")]
        [Authorize(Roles = "Customer")]
        public IActionResult GetInvoiceByRequestId(int requestId)
        {
            try
            {
                var invoice = _billingService.GetInvoiceByRequestId(requestId);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllInvoices()
        {
            return Ok(_billingService.GetAllInvoices());
        }

        [HttpPut("{invoiceId}/pay")]
        [Authorize(Roles = "Customer")]
        public IActionResult PayInvoice(int invoiceId)
        {
            try
            {
                _billingService.CustomerPayInvoice(invoiceId);
                return Ok(new { message = "Payment successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
