import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-manager-layout',
  imports: [RouterModule],
  templateUrl: './manager.layout.html',
  styleUrls: ['./manager.layout.css']
})
export class ManagerLayout {

  constructor(private auth: AuthService) {}

  logout() {
    this.auth.logout();
    location.href = '/login';
  }
}
