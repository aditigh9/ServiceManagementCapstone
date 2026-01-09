using ServiceManagementApi.Data;
using ServiceManagementApi.DTOs.Billing;
using ServiceManagementApi.Models;
using ServiceManagementApi.Services.Interfaces;

namespace ServiceManagementApi.Services.Implementations
{
    public class BillingService : IBillingService
    {
        private readonly ApplicationDbContext _context;

        public BillingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Invoice GetInvoiceByRequestId(int requestId)
        {
            return _context.Invoices
                .FirstOrDefault(i => i.RequestId == requestId)
                ?? throw new Exception("Invoice not found");
        }

        public Invoice GetInvoiceById(int invoiceId)
        {
            return _context.Invoices
                .FirstOrDefault(i => i.InvoiceId == invoiceId)
                ?? throw new Exception("Invoice not found");
        }

        public List<InvoiceDto> GetAllInvoices()
        {
            return _context.Invoices
                .OrderByDescending(i => i.GeneratedDate)
                .Select(i => new InvoiceDto
                {
                    InvoiceId = i.InvoiceId,
                    RequestId = i.RequestId,
                    Amount = i.Amount,
                    PaymentStatus = i.PaymentStatus,
                    GeneratedDate = i.GeneratedDate
                })
                .ToList();
        }

        public void CustomerPayInvoice(int invoiceId)
        {
            var invoice = GetInvoiceById(invoiceId);

            if (invoice.PaymentStatus != "Pending")
                throw new Exception("Invoice cannot be paid");

            invoice.PaymentStatus = "Paid";
            _context.SaveChanges();
        }
    }
}
