import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { DashboardService, RevenueSummary } from '../../core';

const DAY_MS = 86_400_000;
const RANGE_DAYS = 30;

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  // #region Public Properties
  summary?: RevenueSummary;
  errorMessage = '';

  from = this.toDateInput(new Date(Date.now() - RANGE_DAYS * DAY_MS));
  to = this.toDateInput(new Date(Date.now() + RANGE_DAYS * DAY_MS));
  // #endregion

  // #region Constructor
  constructor(private readonly dashboard: DashboardService) {}
  // #endregion

  // #region Lifecycle
  ngOnInit(): void {
    this.load();
  }
  // #endregion

  // #region Public Methods
  load(): void {
    this.errorMessage = '';
    this.dashboard.getRevenueSummary(this.from, this.to).subscribe({
      next: result => (this.summary = result),
      error: (err: HttpErrorResponse) => (this.errorMessage = err.error?.message ?? 'Failed to load dashboard.')
    });
  }
  // #endregion

  // #region Private Methods
  private toDateInput(date: Date): string {
    return date.toISOString().slice(0, 10);
  }
  // #endregion
}
