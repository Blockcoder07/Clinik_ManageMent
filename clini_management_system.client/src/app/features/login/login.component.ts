import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../core';

type AuthMode = 'login' | 'register';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  // #region Public Properties
  mode: AuthMode = 'login';
  clinicName = '';
  username = '';
  password = '';
  errorMessage = '';
  busy = false;
  // #endregion

  // #region Constructor
  constructor(
    private readonly auth: AuthService,
    private readonly router: Router
  ) {}
  // #endregion

  // #region Public Methods
  switchMode(mode: AuthMode): void {
    this.mode = mode;
    this.errorMessage = '';
  }

  login(): void {
    this.runAuth(
      this.auth.login({ username: this.username, password: this.password }),
      'Login failed.'
    );
  }

  register(): void {
    this.runAuth(
      this.auth.register({
        clinicName: this.clinicName,
        username: this.username,
        password: this.password,
        role: 'Admin'
      }),
      'Registration failed.'
    );
  }
  // #endregion

  // #region Private Methods
  private runAuth(observable: ReturnType<AuthService['login']>, fallbackMessage: string): void {
    this.busy = true;
    this.errorMessage = '';

    observable.subscribe({
      next: () => {
        this.busy = false;
        this.router.navigate(['/appointments']);
      },
      error: (err: HttpErrorResponse) => {
        this.busy = false;
        this.errorMessage = err.error?.message ?? fallbackMessage;
      }
    });
  }
  // #endregion
}
