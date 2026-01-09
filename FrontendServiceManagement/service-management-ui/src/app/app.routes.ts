import { Routes } from '@angular/router';
import { LoginPage } from './auth/login/login';
import { RegisterPage } from './auth/register/register';
import { AuthGuard } from './guards/auth-guard';
import { RoleGuard } from './guards/role-guard';

import { AdminLayout } from './layouts/admin-layout/admin.layout';
import { ManagerLayout } from './layouts/manager-layout/manager.layout';
import { CustomerLayout } from './layouts/customer-layout/customer.layout';
import { TechnicianLayout } from './layouts/technician-layout/technician.layout'; 

export const routes: Routes = [

  // ---------- AUTH ----------
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginPage },
  { path: 'register', component: RegisterPage },

  // ---------- ADMIN ----------
  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [AuthGuard],
    canActivateChild: [RoleGuard],
    data: { role: 'Admin' },
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./admin/dashboard/admin-dashboard')
            .then(m => m.AdminDashboard)
      },
      {
        path: 'service-requests',
        loadComponent: () =>
          import('./admin/service-requests/service-requests')
            .then(m => m.AdminServiceRequests)
      },
      {
        path: 'technicians',
        loadComponent: () =>
          import('./admin/technicians/technicians')
            .then(m => m.AdminTechnicians)
      },
      {
        path: 'service-categories',
        loadComponent: () =>
          import('./admin/service-categories/service-categories')
            .then(m => m.AdminServiceCategories)
      },
      {
        path: 'reports',
        loadComponent: () =>
          import('./admin/reports/pages/reports.component')
            .then(m => m.ReportsComponent)
      },
      {
        path: 'billing',
        loadComponent: () =>
          import('./admin/Billing/pages/admin-billing.component')
            .then(m => m.AdminBillingComponent)
      },
      {
        path: 'available-services',
        loadComponent: () =>
          import('./admin/available-services/admin-available-services')
            .then(m => m.AdminAvailableServices)
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  // ---------- SERVICE MANAGER ----------
  {
    path: 'manager',
    component: ManagerLayout,
    canActivate: [AuthGuard],
    canActivateChild: [RoleGuard],
    data: { role: 'ServiceManager' },
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./manager/dashboard/manager-dashboard')
            .then(m => m.ManagerDashboard)
      },
      {
        path: 'service-requests',
        loadComponent: () =>
          import('./manager/service-requests/manager-service-requests')
            .then(m => m.ManagerServiceRequests)
      },
      {
        path: 'reports',
        loadComponent: () =>
          import('./manager/reports/manager-reports')
            .then(m => m.ManagerReports)
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  // ---------- CUSTOMER ----------
  {
    path: 'customer',
    component: CustomerLayout,
    canActivate: [AuthGuard],
    canActivateChild: [RoleGuard],
    data: { role: 'Customer' },
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./customer/dashboard/customer-dashboard')
            .then(m => m.CustomerDashboard)
      },
      {
        path: 'request-service',
        loadComponent: () =>
          import('./customer/request-service/request-service')
            .then(m => m.RequestService)
      },
      {
        path: 'payments',
        loadComponent: () =>
          import('./customer/payments/customer-payments')
            .then(m => m.CustomerPayments)
      },
      
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  // ---------- TECHNICIAN----------
  {
    path: 'technician',
    component: TechnicianLayout,
    canActivate: [AuthGuard],
    canActivateChild: [RoleGuard],
    data: { role: 'Technician' },
    children: [
      {
        path: 'technician-tasks',
        loadComponent: () =>
          import('./technician/technician-tasks/technician-tasks.component')
            .then(m => m.TechnicianTasksComponent)
      },
      { path: '', redirectTo: 'technician-tasks', pathMatch: 'full' }
    ]
  }
];
