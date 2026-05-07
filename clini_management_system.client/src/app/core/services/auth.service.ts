import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthResponse, LoginRequest, RegisterRequest } from '../models';
import { ApiClient } from './api-client.service';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  // #region Constructor
  constructor(
    private readonly api: ApiClient,
    private readonly tokenStorage: TokenStorageService,
    private readonly router: Router
  ) {}
  // #endregion

  // #region Public Methods
  login(payload: LoginRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('/auth/login', payload).pipe(
      map(response => this.persist(response.data))
    );
  }

  register(payload: RegisterRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('/auth/register', payload).pipe(
      map(response => this.persist(response.data))
    );
  }

  isAuthenticated(): boolean {
    return this.tokenStorage.hasToken();
  }

  logout(): void {
    this.tokenStorage.clear();
    this.router.navigate(['/login']);
  }
  // #endregion

  // #region Private Methods
  private persist(auth: AuthResponse): AuthResponse {
    this.tokenStorage.setToken(auth.token);
    return auth;
  }
  // #endregion
}
