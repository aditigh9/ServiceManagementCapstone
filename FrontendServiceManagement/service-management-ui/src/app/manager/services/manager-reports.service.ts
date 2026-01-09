import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ManagerReportsService {

  private baseUrl = 'http://localhost:5022/api/reports';

  constructor(private http: HttpClient) {}

  loadAll(): Observable<{
    status: { status: string; count: number }[];
    category: { categoryName: string; count: number }[];
    technician: { technicianName: string; assignedRequests: number }[];
    revenue: { month: number; year: number; totalRevenue: number }[];
    avgTime: number;
  }> {
    return forkJoin({
      status: this.http.get<any>(`${this.baseUrl}/service-count-by-status`)
        .pipe(map(res => res.data)),

      category: this.http.get<any>(`${this.baseUrl}/service-count-by-category`)
        .pipe(map(res => res.data)),

      technician: this.http.get<any>(`${this.baseUrl}/technician-workload`)
        .pipe(map(res => res.data)),

      revenue: this.http.get<any>(`${this.baseUrl}/monthly-revenue`)
        .pipe(map(res => res.data)),

      avgTime: this.http.get<any>(`${this.baseUrl}/average-resolution-time`)
        .pipe(map(res => res.data.averageResolutionTimeInHours ?? 0))
    });
  }
}
