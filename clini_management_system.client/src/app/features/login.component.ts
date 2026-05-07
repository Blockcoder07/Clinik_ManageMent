import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../core/core';

@Component({
  selector: 'app-login',
  template: `
    <div class="container" style="max-width: 420px; margin-top: 60px;">
      <div class="card">
        <h2>Clinic Login</h2>
        <form (ngSubmit)="login()" #f="ngForm" *ngIf="mode === 'login'">
          <div style="margin-bottom: 12px;">
            <input [(ngModel)]="username" name="username" placeholder="Username" required style="width: 100%;" />
          </div>
          <div style="margin-bottom: 12px;">
            <input [(ngModel)]="password" name="password" type="password" placeholder="Password" required style="width: 100%;" />
          </div>
          <button type="submit" [disabled]="busy" style="width: 100%;">Login</button>
          <p class="muted" style="text-align: center; margin-top: 16px;">
            New clinic? <a href="javascript:void(0)" (click)="mode = 'register'">Register here</a>
          </p>
        </form>

        <form (ngSubmit)="register()" *ngIf="mode === 'register'">
          <div style="margin-bottom: 12px;">
            <input [(ngModel)]="clinicName" name="clinicName" placeholder="Clinic Name" required style="width: 100%;" />
          </div>
          <div style="margin-bottom: 12px;">
            <input [(ngModel)]="username" name="username" placeholder="Admin Username" required style="width: 100%;" />
          </div>
          <div style="margin-bottom: 12px;">
            <input [(ngModel)]="password" name="password" type="password" placeholder="Password (min 8)" required style="width: 100%;" />
          </div>
          <button type="submit" [disabled]="busy" style="width: 100%;">Register</button>
          <p class="muted" style="text-align: center; margin-top: 16px;">
            Already have an account? <a href="javascript:void(0)" (click)="mode = 'login'">Login</a>
          </p>
        </form>

        <p class="error" *ngIf="errorMessage">{{ errorMessage }}</p>
      </div>
    </div>
  `
})
export class LoginComponent {
  mode: 'login' | 'register' = 'login';
  clinicName = '';
  username = '';
  password = '';
  errorMessage = '';
  busy = false;

  constructor(private readonly auth: AuthService, private readonly router: Router) {}

  login(): void {
    this.busy = true;
    this.errorMessage = '';
    this.auth.login({ username: this.username, password: this.password }).subscribe({
      next: () => { this.busy = false; this.router.navigate(['/appointments']); },
      error: (err: HttpErrorResponse) => { this.busy = false; this.errorMessage = err.error?.message ?? 'Login failed.'; }
    });
  }

  register(): void {
    this.busy = true;
    this.errorMessage = '';
    this.auth.register({
      clinicName: this.clinicName,
      username: this.username,
      password: this.password,
      role: 'Admin'
    }).subscribe({
      next: () => { this.busy = false; this.router.navigate(['/appointments']); },
      error: (err: HttpErrorResponse) => { this.busy = false; this.errorMessage = err.error?.message ?? 'Registration failed.'; }
    });
  }
}
