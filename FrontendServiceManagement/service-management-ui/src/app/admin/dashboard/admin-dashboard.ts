import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  standalone: true,
  selector: 'app-admin-dashboard',
  imports: [CommonModule],
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.css']
})
export class AdminDashboard implements OnInit {

  stats = {
    total: 0,
    requested: 0,
    assigned: 0,
    inProgress: 0,
    completed: 0,
    closed: 0,
    cancelled: 0
  };

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http
      .get<any>('http://localhost:5022/api/servicerequests/dashboard-summary')
      .subscribe(res => {
        if (res?.data) {
          this.stats = res.data;
        }
      });
  }
}
