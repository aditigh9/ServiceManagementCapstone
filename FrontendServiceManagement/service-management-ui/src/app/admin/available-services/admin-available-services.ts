import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminServicesService } from '../services/admin-services.service';
import { ServiceCategoryService } from '../service-categories/service-category.service';
import { Service } from '../../models/service.models';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-available-services',
  templateUrl: './admin-available-services.html',
  styleUrls: ['./admin-available-services.css']
})
export class AdminAvailableServices implements OnInit {

  services: Service[] = [];
  filteredServices: Service[] = [];
  categories: any[] = [];

  search = '';
  editingId: number | null = null;

  // popup
  showModal = false;
  popupMessage = '';

  form = {
    name: '',
    description: '',
    serviceCharge: 0,
    serviceCategoryId: 0
  };

  constructor(
    private servicesApi: AdminServicesService,
    private categoryApi: ServiceCategoryService
  ) {}

  ngOnInit() {
    this.loadAll();
  }

  loadAll() {
    this.servicesApi.getAll().subscribe(s => {
      this.services = s;
      this.filteredServices = s;
    });

    this.categoryApi.getAll().subscribe(c => this.categories = c);
  }

  filter() {
    const q = this.search.toLowerCase();
    this.filteredServices = this.services.filter(s =>
      s.name.toLowerCase().includes(q) ||
      s.categoryName?.toLowerCase().includes(q)
    );
  }

  startEdit(s: Service) {
    this.editingId = s.serviceId;
  }

  cancelEdit() {
    this.editingId = null;
    this.loadAll(); // reset edited values
  }

  update(s: Service) {
    this.servicesApi.update(s.serviceId, s).subscribe(() => {
      this.editingId = null;
      this.loadAll();
      this.showPopup('Service updated successfully');
    });
  }

  create() {
    this.servicesApi.create(this.form).subscribe(() => {
      this.loadAll();
      this.showPopup('Service created successfully');

      this.form = {
        name: '',
        description: '',
        serviceCharge: 0,
        serviceCategoryId: 0
      };
    });
  }

  delete(id: number) {
    if (!confirm('Delete this service?')) return;

    this.servicesApi.delete(id).subscribe(() => {
      this.loadAll();
      this.showPopup('Service deleted successfully');
    });
  }

  showPopup(msg: string) {
    this.popupMessage = msg;
    this.showModal = true;

    setTimeout(() => {
      this.showModal = false;
      this.popupMessage = '';
    }, 2000);
  }
}
