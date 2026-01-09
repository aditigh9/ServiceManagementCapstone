import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

export interface ServiceCategory {
  serviceCategoryId: number;
  name: string;
  description?: string;
}

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

@Injectable({
  providedIn: 'root'
})
export class ServiceCategoryService {

  private baseUrl = 'http://localhost:5022/api/servicecategories';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ServiceCategory[]> {
    return this.http
      .get<ApiResponse<ServiceCategory[]>>(this.baseUrl)
      .pipe(map(res => res.data));
  }

  create(data: { name: string; description?: string }): Observable<ServiceCategory> {
    return this.http
      .post<ApiResponse<ServiceCategory>>(this.baseUrl, data)
      .pipe(map(res => res.data));
  }

  update(id: number, data: { name: string; description?: string }): Observable<ServiceCategory> {
    return this.http
      .put<ApiResponse<ServiceCategory>>(`${this.baseUrl}/${id}`, data)
      .pipe(map(res => res.data));
  }

  delete(id: number): Observable<number> {
    return this.http
      .delete<ApiResponse<number>>(`${this.baseUrl}/${id}`)
      .pipe(map(res => res.data));
  }
}
