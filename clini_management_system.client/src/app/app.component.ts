import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './core/core';

@Component({
  selector: 'app-root',
  template: `
    <nav class="topbar" *ngIf="auth.isAuthenticated()">
      <a routerLink="/appointments" routerLinkActive="active">Appointments</a>
      <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
      <span class="spacer"></span>
      <a href="javascript:void(0)" (click)="logout()">Logout</a>
    </nav>
    <router-outlet></router-outlet>
  `
})
export class AppComponent {
  constructor(public readonly auth: AuthService, private readonly router: Router) {}

  logout(): void { this.auth.logout(); }
}
