import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'admin-layout',
  templateUrl: './admin.layout.html',
  styleUrls: ['./admin.layout.css'],
  imports: [RouterModule]
})
export class AdminLayout {

  constructor(private auth: AuthService) {}

  logout() {
    this.auth.logout();
    location.href = '/login';
  }
}
