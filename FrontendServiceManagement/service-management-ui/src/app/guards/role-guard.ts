import { Injectable } from '@angular/core';
import {
  CanActivateChild,
  ActivatedRouteSnapshot,
  Router
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivateChild {

  constructor(private auth: AuthService, private router: Router) {}

  canActivateChild(route: ActivatedRouteSnapshot): boolean {
    const requiredRole = route.parent?.data?.['role'];
    const userRole = this.auth.getUserRole();

    if (!requiredRole || userRole !== requiredRole) {
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
