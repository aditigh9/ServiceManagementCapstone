import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';
import { Service } from '../../models/service.models';

@Injectable({ providedIn: 'root' })
export class AdminServicesService {

  private baseUrl = 'http://localhost:5022/api/services';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Service[]>(this.baseUrl);
  }

  create(data: {
    name: string;
    description: string;
    serviceCharge: number;
    serviceCategoryId: number;
  }) {
    return this.http.post<Service>(this.baseUrl, data);
  }

  update(id: number, data: any) {
    return this.http.put<Service>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
