import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TechnicianService, Technician } from '../service-requests/services/technician.service';

@Component({
  standalone: true,
  selector: 'admin-technicians',
  templateUrl: './technicians.html',
  styleUrls: ['./technicians.css'],
  imports: [CommonModule, FormsModule]
})
export class AdminTechnicians implements OnInit {

  technicians: Technician[] = [];
  loading = true;
  error = '';

  constructor(private service: TechnicianService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: data => {
        this.technicians = data;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load technicians';
        this.loading = false;
      }
    });
  }

  toggle(t: Technician) {
    this.service.toggleStatus(t.technicianId).subscribe({
      next: () => t.isActive = !t.isActive,
      error: err => alert(err.error || 'Toggle failed')
    });
  }
}
