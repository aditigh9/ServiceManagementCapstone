import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Technician {
  technicianId: number;
  userId: number;
  name: string;
  isActive: boolean;
  isAvailable: boolean;
}

@Injectable({ providedIn: 'root' })
export class TechnicianService {
  private baseUrl = 'http://localhost:5022/api/technicians';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Technician[]> {
    return this.http.get<Technician[]>(this.baseUrl);
  }

  create(dto: { name: string; userId: number }): Observable<string> {
    return this.http.post(this.baseUrl, dto, { responseType: 'text' });
  }

  update(id: number, dto: { name: string }): Observable<string> {
    return this.http.put(`${this.baseUrl}/${id}`, dto, {
      responseType: 'text'
    });
  }

  toggleStatus(id: number): Observable<string> {
    return this.http.put(`${this.baseUrl}/${id}/toggle`, {}, {
      responseType: 'text'
    });
  }

  delete(id: number): Observable<string> {
    return this.http.delete(`${this.baseUrl}/${id}`, {
      responseType: 'text'
    });
  }
}

