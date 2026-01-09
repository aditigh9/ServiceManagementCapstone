import { Routes } from '@angular/router';
import { ReportsComponent } from './pages/reports.component';
import { AuthGuard } from '../../guards/auth-guard';

export const REPORTS_ROUTES: Routes = [
  {
    path: '',
    component: ReportsComponent,
    canActivate: [AuthGuard]
  }
];
