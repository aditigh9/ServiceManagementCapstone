import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class TechnicianRequestsService {

  private baseUrl = 'http://localhost:5022/api/servicerequests';

  constructor(private http: HttpClient) {}

  getAssigned() {
    return this.http.get<any>(`${this.baseUrl}/assigned`);
  }

  updateStatus(id: number, status: string) {
    return this.http.put(`${this.baseUrl}/${id}/status`, { status });
  }
}
