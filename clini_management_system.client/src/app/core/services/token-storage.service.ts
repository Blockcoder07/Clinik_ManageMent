import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  // #region Constants
  private readonly storageKey = 'clinic.jwt';
  // #endregion

  // #region Public Methods
  setToken(token: string): void {
    localStorage.setItem(this.storageKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.storageKey);
  }

  clear(): void {
    localStorage.removeItem(this.storageKey);
  }

  hasToken(): boolean {
    return !!this.getToken();
  }
  // #endregion
}
