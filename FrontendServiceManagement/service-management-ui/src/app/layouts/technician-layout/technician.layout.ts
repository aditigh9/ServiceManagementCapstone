import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'technician-layout',
  templateUrl: './technician.layout.html',
  styleUrls: ['./technician.layout.css'],
  imports: [RouterModule]
})
export class TechnicianLayout {

  constructor(private auth: AuthService) {}

  logout() {
    this.auth.logout();
    location.href = '/login';
  }
}
