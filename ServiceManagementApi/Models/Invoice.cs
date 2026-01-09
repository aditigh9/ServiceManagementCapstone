namespace ServiceManagementApi.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int RequestId { get; set; }
        public decimal Amount { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    }
}
