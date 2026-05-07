import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { RevenueSummary } from '../models';
import { ApiClient } from './api-client.service';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  // #region Constants
  private readonly endpoint = '/dashboard';
  // #endregion

  // #region Constructor
  constructor(private readonly api: ApiClient) {}
  // #endregion

  // #region Public Methods
  getRevenueSummary(from: string, to: string): Observable<RevenueSummary> {
    return this.api
      .get<RevenueSummary>(`${this.endpoint}/revenue-summary`, { from, to })
      .pipe(map(response => response.data));
  }
  // #endregion
}
