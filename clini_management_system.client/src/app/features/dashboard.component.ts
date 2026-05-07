import { Component, OnInit } from '@angular/core';
import { DashboardService, RevenueSummary } from '../core/core';

@Component({
  selector: 'app-dashboard',
  template: `
    <div class="container">
      <div class="card">
        <h2>Revenue Dashboard</h2>
        <div class="row">
          <label>From <input type="date" [(ngModel)]="from" /></label>
          <label>To <input type="date" [(ngModel)]="to" /></label>
          <button (click)="load()">Refresh</button>
        </div>
      </div>

      <div class="card" *ngIf="summary">
        <div class="row" style="justify-content: space-around;">
          <div style="text-align: center;">
            <div class="metric">{{ summary.totalRevenue | currency:'INR' }}</div>
            <div class="metric-label">Total Revenue</div>
          </div>
          <div style="text-align: center;">
            <div class="metric">{{ summary.totalAppointments }}</div>
            <div class="metric-label">Total Appointments</div>
          </div>
          <div style="text-align: center;">
            <div class="metric" style="color: #16a34a;">{{ summary.completedAppointments }}</div>
            <div class="metric-label">Completed</div>
          </div>
          <div style="text-align: center;">
            <div class="metric" style="color: #dc2626;">{{ summary.cancelledAppointments }}</div>
            <div class="metric-label">Cancelled</div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  summary?: RevenueSummary;
  from = new Date(Date.now() - 30 * 86_400_000).toISOString().slice(0, 10);
  to = new Date(Date.now() + 30 * 86_400_000).toISOString().slice(0, 10);

  constructor(private readonly service: DashboardService) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.service.summary(this.from, this.to).subscribe({ next: r => this.summary = r });
  }
}
