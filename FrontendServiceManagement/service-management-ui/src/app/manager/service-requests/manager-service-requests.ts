import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ManagerService } from '../services/manager.service';
import { ServiceRequestDto } from '../../models/service-request.dto';
import { Technician } from '../../models/technician.dto';

@Component({
  standalone: true,
  selector: 'manager-service-requests',
  imports: [CommonModule, FormsModule],
  templateUrl: './manager-service-requests.html',
  styleUrls: ['./manager-service-requests.css']
})
export class ManagerServiceRequests implements OnInit {

  requests: ServiceRequestDto[] = [];
  technicians: Technician[] = [];

  loading = true;
  error = '';

  showModal = false;
  popupMessage = '';

  constructor(private service: ManagerService) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.loading = true;

    this.service.getAllRequests().subscribe({
      next: data => {
        this.requests = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load service requests';
        this.loading = false;
      }
    });

    this.service.getTechnicians().subscribe({
      next: data => this.technicians = data
    });
  }

  assign(requestId: number, technicianId: number) {
    this.service.assignTechnician(requestId, technicianId).subscribe({
      next: (res: any) => {
        this.openModal(res?.message || 'Technician assigned successfully');
        this.load();
      },
      error: err => {
        this.openModal(err?.error?.message || 'Technician not available');
      }
    });
  }

  setPriority(r: ServiceRequestDto) {
    this.service.setPriority(r.serviceRequestId, r.priority, r.scheduledAt!)
      .subscribe({
        next: () => this.openModal('Priority saved'),
        error: () => this.openModal('Failed to save priority')
      });
  }

  reschedule(r: ServiceRequestDto) {
    this.service.reschedule(r.serviceRequestId, r.scheduledAt!)
      .subscribe({
        next: () => this.openModal('Rescheduled'),
        error: () => this.openModal('Failed to reschedule')
      });
  }

  close(r: ServiceRequestDto) {
    this.service.closeRequest(r.serviceRequestId)
      .subscribe({
        next: () => {
          r.status = 'Closed';
          this.openModal('Request closed');
        },
        error: () => this.openModal('Failed to close request')
      });
  }

  openModal(message: string) {
    this.popupMessage = message;
    this.showModal = true;

    setTimeout(() => {
      this.closeModal();
    }, 2000);
  }

  closeModal() {
    this.showModal = false;
    this.popupMessage = '';
  }
}
