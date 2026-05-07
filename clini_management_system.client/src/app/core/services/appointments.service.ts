import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import {
  Appointment,
  AppointmentCreateRequest,
  AppointmentStatus,
  AppointmentStatusUpdateRequest,
  PagedResult
} from '../models';
import { ApiClient } from './api-client.service';

@Injectable({ providedIn: 'root' })
export class AppointmentsService {
  // #region Constants
  private readonly endpoint = '/appointments';
  private readonly defaultPageSize = 20;
  // #endregion

  // #region Constructor
  constructor(private readonly api: ApiClient) {}
  // #endregion

  // #region Public Methods
  list(from: string, to: string, pageNumber = 1, pageSize = this.defaultPageSize): Observable<PagedResult<Appointment>> {
    return this.api
      .get<PagedResult<Appointment>>(this.endpoint, { from, to, pageNumber, pageSize })
      .pipe(map(response => response.data));
  }

  create(payload: AppointmentCreateRequest): Observable<Appointment> {
    return this.api
      .post<Appointment>(this.endpoint, payload)
      .pipe(map(response => response.data));
  }

  updateStatus(id: number, status: AppointmentStatus, rowVersion: string): Observable<Appointment> {
    const payload: AppointmentStatusUpdateRequest = { status, rowVersion };
    return this.api
      .patch<Appointment>(`${this.endpoint}/${id}/status`, payload)
      .pipe(map(response => response.data));
  }
  // #endregion
}
