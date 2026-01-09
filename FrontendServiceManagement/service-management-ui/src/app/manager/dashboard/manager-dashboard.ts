import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chart } from 'chart.js/auto';

@Component({
  standalone: true,
  selector: 'manager-dashboard',
  templateUrl: './manager-dashboard.html',
  styleUrls: ['./manager-dashboard.css']
})
export class ManagerDashboard implements OnInit {

  requested = 0;
  assigned = 0;
  completed = 0;
  closed = 0;
  cancelled = 0;

  private apiUrl = 'http://localhost:5022/api/servicerequests/dashboard-summary';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadSummary();
  }

  loadSummary() {
    this.http.get<any>(this.apiUrl).subscribe({
      next: res => {
        const d = res.data;
        this.requested = d.requested;
        this.assigned = d.assigned;
        this.completed = d.completed;
        this.closed = d.closed;
        this.cancelled = d.cancelled;
        this.renderChart();
      },
      error: err => {
        console.error('Dashboard summary failed', err);
      }
    });
  }

  renderChart() {
    new Chart('requestsChart', {
      type: 'bar',
      data: {
        labels: [
          'Requested',
          'Assigned',
          'Completed',
          'Closed',
          'Cancelled'
        ],
        datasets: [
          {
            label: 'Service Requests',
            data: [
              this.requested,
              this.assigned,
              this.completed,
              this.closed,
              this.cancelled
            ]
          }
        ]
      },
      options: {
        responsive: true
      }
    });
  }

}
