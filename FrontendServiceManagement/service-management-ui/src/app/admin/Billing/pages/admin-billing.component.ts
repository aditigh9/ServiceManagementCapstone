import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BillingService } from '../services/billing.service';
import { Invoice } from '../models/invoice.model';

@Component({
  standalone: true,
  selector: 'app-admin-billing',
  imports: [CommonModule],
  templateUrl: './admin-billing.component.html',
  styleUrls: ['./admin-billing.component.css']
})
export class AdminBillingComponent implements OnInit {

  invoices: Invoice[] = [];
  loading = true;

  constructor(private billingService: BillingService) {}

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.loading = true;
    this.billingService.getAllInvoices().subscribe({
      next: data => {
        this.invoices = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
