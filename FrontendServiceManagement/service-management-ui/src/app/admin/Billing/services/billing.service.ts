import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Invoice } from '../models/invoice.model';

@Injectable({
  providedIn: 'root'
})
export class BillingService {

  private baseUrl = 'http://localhost:5022/api/billing';

  constructor(private http: HttpClient) {}

  // ADMIN: get all invoices
  getAllInvoices(): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(this.baseUrl);
  }

  // ADMIN: approve payment
  approvePayment(invoiceId: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/${invoiceId}/approve`, {});
  }
}
