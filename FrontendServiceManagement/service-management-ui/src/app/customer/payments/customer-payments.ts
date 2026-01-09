import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaymentsService } from '../services/payments.service';

@Component({
  standalone: true,
  selector: 'customer-payments',
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-payments.html',
  styleUrls: ['./customer-payments.css']
})
export class CustomerPayments {

  requestId!: number;
  invoice: any;
  loading = false;

  constructor(private payments: PaymentsService) {}

  load() {
    if (!this.requestId) return;

    this.loading = true;
    this.invoice = null;

    this.payments.getInvoice(this.requestId)
      .subscribe({
        next: i => {
          this.invoice = i;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          alert('Invoice not found for this request');
        }
      });
  }

  pay() {
    this.payments.payInvoice(this.invoice.invoiceId)
      .subscribe(() => {
        this.invoice.paymentStatus = 'Paid';
      });
  }
}
