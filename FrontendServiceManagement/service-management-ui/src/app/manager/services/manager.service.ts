import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ServiceRequestDto } from '../../models/service-request.dto';
import { Technician } from '../../models/technician.dto';

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

@Injectable({ providedIn: 'root' })
export class ManagerService {

  private requestUrl = 'http://localhost:5022/api/servicerequests';
  private techUrl = 'http://localhost:5022/api/technicians';

  constructor(private http: HttpClient) {}

  // ---------------- DASHBOARD ----------------
  getDashboardSummary(): Observable<any> {
    return this.http
      .get<ApiResponse<any>>(`${this.requestUrl}/dashboard-summary`)
      .pipe(map(res => res.data));
  }

  // ---------------- REQUESTS ----------------
  getAllRequests(): Observable<ServiceRequestDto[]> {
    return this.http
      .get<ApiResponse<ServiceRequestDto[]>>(this.requestUrl)
      .pipe(map(res => res.data));
  }

  setPriority(id: number, priority: string, scheduledAt: string) {
    return this.http.put(
      `${this.requestUrl}/${id}/priority`,
      { priority, scheduledAt }
    );
  }

  reschedule(id: number, scheduledAt: string) {
    return this.http.put(
      `${this.requestUrl}/${id}/reschedule`,
      { scheduledAt }
    );
  }

  closeRequest(id: number) {
    return this.http.put(`${this.requestUrl}/${id}/close`, {});
  }

  // ---------------- TECHNICIANS ----------------
  getTechnicians(): Observable<Technician[]> {
    return this.http.get<Technician[]>(this.techUrl);
  }

  assignTechnician(serviceRequestId: number, technicianId: number) {
    return this.http.post(`${this.techUrl}/assign`, {
      serviceRequestId,
      technicianId
    });
  }
}
