using ServiceManagementApi.Models;
using ServiceManagementApi.DTOs.Billing;
using System.Collections.Generic;

namespace ServiceManagementApi.Services.Interfaces
{
    public interface IBillingService
    {
        Invoice GetInvoiceByRequestId(int requestId);
        Invoice GetInvoiceById(int invoiceId);
        List<InvoiceDto> GetAllInvoices();
        void CustomerPayInvoice(int invoiceId);
    }
}
