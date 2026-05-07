import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { PagedResult, Patient, PatientCreateRequest } from '../models';
import { ApiClient } from './api-client.service';

@Injectable({ providedIn: 'root' })
export class PatientsService {
  // #region Constants
  private readonly endpoint = '/patients';
  private readonly defaultPageSize = 50;
  // #endregion

  // #region Constructor
  constructor(private readonly api: ApiClient) {}
  // #endregion

  // #region Public Methods
  list(pageNumber = 1, pageSize = this.defaultPageSize, search = ''): Observable<PagedResult<Patient>> {
    return this.api
      .get<PagedResult<Patient>>(this.endpoint, { pageNumber, pageSize, search })
      .pipe(map(response => response.data));
  }

  create(payload: PatientCreateRequest): Observable<Patient> {
    return this.api
      .post<Patient>(this.endpoint, payload)
      .pipe(map(response => response.data));
  }
  // #endregion
}
