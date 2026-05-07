import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { TokenStorageService } from '../services';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  // #region Constructor
  constructor(
    private readonly tokenStorage: TokenStorageService,
    private readonly router: Router
  ) {}
  // #endregion

  // #region Public Methods
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const authorized = this.attachToken(request);

    return next.handle(authorized).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) this.handleUnauthorized();
        return throwError(() => error);
      })
    );
  }
  // #endregion

  // #region Private Methods
  private attachToken(request: HttpRequest<unknown>): HttpRequest<unknown> {
    const token = this.tokenStorage.getToken();
    if (!token) return request;

    return request.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  private handleUnauthorized(): void {
    this.tokenStorage.clear();
    this.router.navigate(['/login']);
  }
  // #endregion
}
