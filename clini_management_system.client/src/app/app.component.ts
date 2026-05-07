import { Component } from '@angular/core';

import { AuthService } from './core';

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
  constructor(public readonly auth: AuthService) {}

  logout(): void {
    this.auth.logout();
  }
}
