import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerService } from '../services/customer.service';
import { ServiceRequestDto } from '../../models/service-request.dto';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'customer-dashboard',
  templateUrl: './customer-dashboard.html',
  styleUrls: ['./customer-dashboard.css']
})
export class CustomerDashboard implements OnInit {

  requests: ServiceRequestDto[] = [];

  constructor(private customer: CustomerService) {}

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests() {
    this.customer.getMyRequests().subscribe({
      next: r => this.requests = r,
      error: err => console.error(err)
    });
  }

  cancelRequest(id: number) {
    if (!confirm('Are you sure you want to cancel this request?')) return;

    this.customer.cancelRequest(id).subscribe({
      next: () => {
        // refresh list after cancel
        this.loadRequests();
      },
      error: err => console.error(err)
    });
  }
}
