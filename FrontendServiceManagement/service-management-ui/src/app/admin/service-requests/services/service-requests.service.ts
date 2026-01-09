import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ServiceRequestDto } from '../../../models/service-request.dto';

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

@Injectable({
  providedIn: 'root'
})
export class ServiceRequestsService {

  private baseUrl = 'http://localhost:5022/api/servicerequests';

  constructor(private http: HttpClient) {}

  // ADMIN / SERVICE MANAGER
  getAll(): Observable<ServiceRequestDto[]> {
    return this.http
      .get<ApiResponse<ServiceRequestDto[]>>(this.baseUrl)
      .pipe(map(res => res.data));
  }
}
