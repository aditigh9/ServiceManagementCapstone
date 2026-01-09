import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

import { MatCardModule } from '@angular/material/card';

import { ServiceRequestsService } from './services/service-requests.service';
import { ServiceRequestDto } from '../../models/service-request.dto';

@Component({
  standalone: true,
  selector: 'admin-service-requests',
  templateUrl: './service-requests.html',
  styleUrls: ['./service-requests.css'],
  imports: [CommonModule, MatCardModule]
})
export class AdminServiceRequests {

  serviceRequests$!: Observable<ServiceRequestDto[]>;

  constructor(private service: ServiceRequestsService) {
    this.load();
  }

  load() {
    this.serviceRequests$ = this.service.getAll();
  }
}

