import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import { ChartData, ChartOptions } from 'chart.js';
import { ManagerReportsService } from '../services/manager-reports.service';

@Component({
  standalone: true,
  selector: 'manager-reports',
  imports: [CommonModule, NgChartsModule],
  templateUrl: './manager-reports.html',
  styleUrls: ['./manager-reports.css']
})
export class ManagerReports implements OnInit {

  technicianWorkload: { technicianName: string; assignedRequests: number }[] = [];
  monthlyRevenue: { month: number; year: number; totalRevenue: number }[] = [];
  avgResolutionTime = 0;

  statusChartData!: ChartData<'doughnut'>;
  categoryChartData!: ChartData<'bar'>;

  statusChartOptions: ChartOptions<'doughnut'> = {
  responsive: true,
  plugins: {
    legend: {
      display: false  
    }
  }
  };
  categoryChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: { display: false }
    }
  };

  constructor(private service: ManagerReportsService) {}

  ngOnInit(): void {
    this.service.loadAll().subscribe({
      next: (res: {
        status: { status: string; count: number }[];
        category: { categoryName: string; count: number }[];
        technician: { technicianName: string; assignedRequests: number }[];
        revenue: { month: number; year: number; totalRevenue: number }[];
        avgTime?: number;
      }) => {

       
        this.statusChartData = {
          labels: res.status.map(x => x.status),
          datasets: [{
            label: 'Requests by Status',   
            data: res.status.map(x => x.count),
            backgroundColor: ['#6366f1', '#22c55e', '#f59e0b', '#ef4444']
          }]
        };

        
        this.categoryChartData = {
          labels: res.category.map(x => x.categoryName),
          datasets: [{
            label: 'Requests',
            data: res.category.map(x => x.count),
            backgroundColor: '#3b82f6'
          }]
        };

        this.technicianWorkload = res.technician;
        this.monthlyRevenue = res.revenue;

       
        this.avgResolutionTime = res.avgTime ?? 0;
      },
      error: (err) => {
        console.error('Manager reports failed', err);
      }
    });
  }

  max<T>(arr: T[], key: keyof T): number {
  return Math.max(
    ...arr.map(x => Number(x[key]) || 0),
    1
  );
}

}
