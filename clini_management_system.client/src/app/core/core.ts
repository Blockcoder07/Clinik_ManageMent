import { HttpClient, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface LoginRequest { username: string; password: string; }
export interface RegisterRequest { clinicName: string; username: string; password: string; role: string; }
export interface AuthResponse { token: string; expiresAt: string; username: string; role: string; clinicId: number; }

export enum AppointmentStatus { Scheduled = 1, Completed = 2, Cancelled = 3 }

export interface Appointment {
  id: number;
  patientId: number;
  patientName: string;
  doctorName: string;
  appointmentDate: string;
  status: AppointmentStatus;
  rowVersion: string;
}

export interface AppointmentCreateRequest {
  patientId: number;
  doctorName: string;
  appointmentDate: string;
}

export interface Patient { id: number; name: string; mobileNumber: string; }
export interface PatientCreateRequest { name: string; mobileNumber: string; }

export interface RevenueSummary {
  totalRevenue: number;
  totalAppointments: number;
  completedAppointments: number;
  cancelledAppointments: number;
}

const TOKEN_KEY = 'clinic.jwt';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  setToken(token: string): void { localStorage.setItem(TOKEN_KEY, token); }
  getToken(): string | null { return localStorage.getItem(TOKEN_KEY); }
  clear(): void { localStorage.removeItem(TOKEN_KEY); }
}

@Injectable({ providedIn: 'root' })
export class ApiClient {
  private readonly baseUrl = '/api';

  constructor(private readonly http: HttpClient) {}

  get<T>(url: string, params?: Record<string, string | number | boolean>): Observable<ApiResponse<T>> {
    let httpParams = new HttpParams();
    Object.entries(params ?? {}).forEach(([k, v]) => { httpParams = httpParams.set(k, String(v)); });
    return this.http.get<ApiResponse<T>>(`${this.baseUrl}${url}`, { params: httpParams });
  }

  post<T>(url: string, body: unknown): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(`${this.baseUrl}${url}`, body);
  }

  patch<T>(url: string, body: unknown): Observable<ApiResponse<T>> {
    return this.http.patch<ApiResponse<T>>(`${this.baseUrl}${url}`, body);
  }
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private readonly api: ApiClient, private readonly token: TokenStorageService, private readonly router: Router) {}

  login(payload: LoginRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('/auth/login', payload).pipe(map(r => {
      this.token.setToken(r.data.token);
      return r.data;
    }));
  }

  register(payload: RegisterRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('/auth/register', payload).pipe(map(r => {
      this.token.setToken(r.data.token);
      return r.data;
    }));
  }

  isAuthenticated(): boolean { return !!this.token.getToken(); }

  logout(): void {
    this.token.clear();
    this.router.navigate(['/login']);
  }
}

@Injectable({ providedIn: 'root' })
export class AppointmentsService {
  constructor(private readonly api: ApiClient) {}

  list(from: string, to: string, pageNumber = 1, pageSize = 20): Observable<PagedResult<Appointment>> {
    return this.api.get<PagedResult<Appointment>>('/appointments', { from, to, pageNumber, pageSize }).pipe(map(r => r.data));
  }

  create(payload: AppointmentCreateRequest): Observable<Appointment> {
    return this.api.post<Appointment>('/appointments', payload).pipe(map(r => r.data));
  }

  updateStatus(id: number, status: AppointmentStatus, rowVersion: string): Observable<Appointment> {
    return this.api.patch<Appointment>(`/appointments/${id}/status`, { status, rowVersion }).pipe(map(r => r.data));
  }
}

@Injectable({ providedIn: 'root' })
export class PatientsService {
  constructor(private readonly api: ApiClient) {}

  list(pageNumber = 1, pageSize = 50, search = ''): Observable<PagedResult<Patient>> {
    return this.api.get<PagedResult<Patient>>('/patients', { pageNumber, pageSize, search }).pipe(map(r => r.data));
  }

  create(payload: PatientCreateRequest): Observable<Patient> {
    return this.api.post<Patient>('/patients', payload).pipe(map(r => r.data));
  }
}

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private readonly api: ApiClient) {}

  summary(from: string, to: string): Observable<RevenueSummary> {
    return this.api.get<RevenueSummary>('/dashboard/revenue-summary', { from, to }).pipe(map(r => r.data));
  }
}

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private readonly auth: AuthService, private readonly router: Router) {}

  canActivate(): boolean {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }
}

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private readonly token: TokenStorageService, private readonly router: Router) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.token.getToken();
    const cloned = token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

    return next.handle(cloned).pipe(catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        this.token.clear();
        this.router.navigate(['/login']);
      }
      return throwError(() => error);
    }));
  }
}
