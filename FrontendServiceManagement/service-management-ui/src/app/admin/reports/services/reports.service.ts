import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ServiceStatusReport } from '../models/service-status-report.model';
import { ServiceCategoryReport } from '../models/service-category-report.model';
import { TechnicianWorkload } from '../models/technician-workload.model';
import { MonthlyRevenue } from '../models/monthly-revenue.model';

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

@Injectable({
  providedIn: 'root'
})
export class ReportsService {

  private baseUrl = 'http://localhost:5022/api/reports';

  constructor(private http: HttpClient) {}

  getServiceCountByStatus(): Observable<ServiceStatusReport[]> {
    return this.http
      .get<ApiResponse<ServiceStatusReport[]>>(`${this.baseUrl}/service-count-by-status`)
      .pipe(map(res => res.data));
  }

  getServiceCountByCategory(): Observable<ServiceCategoryReport[]> {
    return this.http
      .get<ApiResponse<ServiceCategoryReport[]>>(`${this.baseUrl}/service-count-by-category`)
      .pipe(map(res => res.data));
  }

  getTechnicianWorkload(): Observable<TechnicianWorkload[]> {
    return this.http
      .get<ApiResponse<TechnicianWorkload[]>>(`${this.baseUrl}/technician-workload`)
      .pipe(map(res => res.data));
  }

  getAverageResolutionTime(): Observable<number> {
    return this.http
      .get<ApiResponse<{ averageResolutionTimeInHours: number }>>(
        `${this.baseUrl}/average-resolution-time`
      )
      .pipe(map(res => res.data.averageResolutionTimeInHours));
  }

  getMonthlyRevenue(): Observable<MonthlyRevenue[]> {
    return this.http
      .get<ApiResponse<MonthlyRevenue[]>>(`${this.baseUrl}/monthly-revenue`)
      .pipe(map(res => res.data));
  }
}
