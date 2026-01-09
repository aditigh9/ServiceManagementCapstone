import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class PaymentsService {

  private baseUrl = 'http://localhost:5022/api/billing';

  constructor(private http: HttpClient) {}

  getInvoice(requestId: number) {
    return this.http.get<any>(`${this.baseUrl}/${requestId}`);
  }

  payInvoice(invoiceId: number) {
    return this.http.put(`${this.baseUrl}/${invoiceId}/pay`, {});
  }
}
