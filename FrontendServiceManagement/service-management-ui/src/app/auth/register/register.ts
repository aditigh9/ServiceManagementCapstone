import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-register',
  templateUrl: './register.html',
  styleUrls: ['./register.css'],
  imports: [
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatSnackBarModule
  ]
})
export class RegisterPage {

  form: FormGroup;
  loading = false;

  roles = ['Customer', 'Admin', 'Technician', 'ServiceManager'];

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private snack: MatSnackBar
  ) {
    
    this.form = this.fb.nonNullable.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      role: ['Customer', Validators.required]
    });
  }

  submit(): void {
    if (this.form.invalid || this.loading) return;

    this.loading = true;

    this.auth.register(this.form.getRawValue()).subscribe({
      next: (res: { message: string }) => {
        setTimeout(() => {
          this.loading = false;
          this.snack.open(res.message, 'Close', { duration: 3000 });
          this.router.navigate(['/login']);
        });
      },
      error: (err: any) => {
        setTimeout(() => {
          this.loading = false;
          this.snack.open(
            err?.error?.message || 'Registration failed',
            'Close',
            { duration: 4000 }
          );
        });
      }
    });
  }
}
