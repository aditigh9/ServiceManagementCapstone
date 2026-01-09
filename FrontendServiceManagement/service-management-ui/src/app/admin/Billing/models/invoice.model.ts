export interface Invoice {
  invoiceId: number;
  requestId: number;
  amount: number;
  paymentStatus: string;
  generatedDate: string;
}
