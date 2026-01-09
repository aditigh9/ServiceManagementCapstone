namespace ServiceManagementApi.DTOs.Billing
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public int RequestId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; }
    }
}
