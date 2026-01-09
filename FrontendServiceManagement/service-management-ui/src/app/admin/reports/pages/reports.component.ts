import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';

import { ReportsService } from '../services/reports.service';
import { ServiceStatusReport } from '../models/service-status-report.model';
import { ServiceCategoryReport } from '../models/service-category-report.model';
import { TechnicianWorkload } from '../models/technician-workload.model';
import { MonthlyRevenue } from '../models/monthly-revenue.model';

@Component({
  standalone: true,
  selector: 'app-admin-reports',
  imports: [CommonModule],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {

  statusReports: ServiceStatusReport[] = [];
  categoryReports: ServiceCategoryReport[] = [];
  technicianWorkload: TechnicianWorkload[] = [];
  monthlyRevenue: MonthlyRevenue[] = [];
  avgResolutionTime = 0;

  loading = true;
  error = '';

  constructor(private reportsService: ReportsService) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.loading = true;

    forkJoin({
      status: this.reportsService.getServiceCountByStatus(),
      category: this.reportsService.getServiceCountByCategory(),
      technician: this.reportsService.getTechnicianWorkload(),
      revenue: this.reportsService.getMonthlyRevenue(),
      avgTime: this.reportsService.getAverageResolutionTime()
    }).subscribe({
      next: (res) => {
        this.statusReports = res.status;
        this.categoryReports = res.category;
        this.technicianWorkload = res.technician;
        this.monthlyRevenue = res.revenue;
        this.avgResolutionTime = res.avgTime;
        this.loading = false;
      },
      error: (err) => {
        console.error('Reports API error:', err);
        this.error = 'Failed to load reports';
        this.loading = false; 
      }
    });
  }
}
