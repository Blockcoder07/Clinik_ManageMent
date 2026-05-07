import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiResponse } from '../models';

type QueryParams = Record<string, string | number | boolean | null | undefined>;

@Injectable({ providedIn: 'root' })
export class ApiClient {
  // #region Constants
  private readonly baseUrl = '/api';
  // #endregion

  // #region Constructor
  constructor(private readonly http: HttpClient) {}
  // #endregion

  // #region Public Methods
  get<T>(url: string, params?: QueryParams): Observable<ApiResponse<T>> {
    return this.http.get<ApiResponse<T>>(this.fullUrl(url), { params: this.buildParams(params) });
  }

  post<T>(url: string, body: unknown): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(this.fullUrl(url), body);
  }

  patch<T>(url: string, body: unknown): Observable<ApiResponse<T>> {
    return this.http.patch<ApiResponse<T>>(this.fullUrl(url), body);
  }
  // #endregion

  // #region Private Methods
  private fullUrl(url: string): string {
    return `${this.baseUrl}${url}`;
  }

  private buildParams(params?: QueryParams): HttpParams {
    let httpParams = new HttpParams();
    if (!params) return httpParams;

    for (const [key, value] of Object.entries(params)) {
      if (value === null || value === undefined || value === '') continue;
      httpParams = httpParams.set(key, String(value));
    }
    return httpParams;
  }
  // #endregion
}
