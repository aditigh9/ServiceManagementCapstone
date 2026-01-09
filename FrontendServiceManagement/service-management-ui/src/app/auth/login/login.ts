import { Component } from '@angular/core';
import {
  FormBuilder,
  Validators,
  ReactiveFormsModule,
  FormGroup
} from '@angular/forms';
import { Router,RouterModule } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule
  ]
})
export class LoginPage {

  loading = false;
  form!: FormGroup; 

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private snack: MatSnackBar
  ) {
    
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  submit() {
  if (this.form.invalid) return;

  this.loading = true;

  this.auth.login({
    email: this.form.value.email!,
    password: this.form.value.password!
  }).subscribe({
    next: () => {
      this.loading = false;

      const role = this.auth.getUserRole();

      switch (role) {
        case 'Admin':
          this.router.navigate(['/admin/dashboard']);
          break;

        case 'Customer':
          this.router.navigate(['/customer/dashboard']);
          break;

        case 'Technician':
          this.router.navigate(['/technician/technician-tasks']);
          break;

        case 'ServiceManager':
          this.router.navigate(['/manager/dashboard']);
          break;

        default:
          this.router.navigate(['/login']);
      }
    },
    error: (err) => {
      this.loading = false;
      this.snack.open(err.error?.message || 'Login failed', 'Close', {
        duration: 3000
      });
    }
  });
}
}