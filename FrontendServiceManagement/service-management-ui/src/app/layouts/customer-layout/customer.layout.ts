import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-customer-layout',
  imports: [RouterModule],
  templateUrl: './customer.layout.html',
  styleUrls: ['./customer.layout.css']
})
export class CustomerLayout {

  constructor(private auth: AuthService) {}

  logout() {
    this.auth.logout();
    location.href = '/login';
  }
}
