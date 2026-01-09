import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ServicesService } from '../services/services.service';
import { CustomerService } from '../services/customer.service';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'request-service',
  templateUrl: './request-service.html',
  styleUrls: ['./request-service.css']
})
export class RequestService implements OnInit {

  services: any[] = [];
  filteredServices: any[] = [];
  search = '';

  constructor(
    private servicesApi: ServicesService,
    private customer: CustomerService
  ) {}

  ngOnInit(): void {
    this.servicesApi.getAll().subscribe(s => {
      this.services = s;
      this.filteredServices = s;
    });
  }

  filter() {
    const q = this.search.toLowerCase();
    this.filteredServices = this.services.filter(s =>
      s.name.toLowerCase().includes(q) ||
      s.description?.toLowerCase().includes(q)
    );
  }

  request(id: number) {
    this.customer.createRequest(id).subscribe(() =>
      alert('Service requested')
    );
  }
}
