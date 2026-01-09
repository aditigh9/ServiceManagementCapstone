import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CustomerService {

  private baseUrl = 'http://localhost:5022/api/servicerequests';

  constructor(private http: HttpClient) {}

  getMyRequests() {
    return this.http.get<any>(`${this.baseUrl}/my`)
      .pipe(map(res => res.data));
  }

  createRequest(serviceId: number) {
    return this.http.post(`${this.baseUrl}`, { serviceId });
  }

  cancelRequest(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/cancel`, {});
  }
 

}
