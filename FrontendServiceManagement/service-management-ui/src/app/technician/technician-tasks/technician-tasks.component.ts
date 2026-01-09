import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TechnicianRequestsService } from '../technician-services/technician-requests.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'technician-tasks',
  templateUrl: './technician-tasks.component.html',
  styleUrls: ['./technician-tasks.component.css']
})
export class TechnicianTasksComponent implements OnInit {

  tasks: any[] = [];

  showModal = false;
  popupMessage = '';

  statuses = [
    { label: 'In Progress', value: 'InProgress' },
    { label: 'Completed', value: 'Completed' }
  ];

  constructor(private service: TechnicianRequestsService) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks() {
    this.service.getAssigned().subscribe({
      next: res => {
        this.tasks = res.data;
      },
      error: err => {
        this.openModal(err?.error?.message || 'Failed to load assigned tasks');
      }
    });
  }

  onStatusChange(task: any, newStatus: string) {
    if (task.status === newStatus) return;

    this.service.updateStatus(task.serviceRequestId, newStatus)
      .subscribe({
        next: () => {
          task.status = newStatus;

          if (newStatus === 'Completed') {
            this.tasks = this.tasks.filter(
              t => t.serviceRequestId !== task.serviceRequestId
            );
          }

          this.openModal('Status updated successfully');
        },
        error: err => {
          this.openModal(err?.error?.message || 'Failed to update status');
        }
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
