import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthStatus, AuthUser } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthState {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly USER_KEY = 'auth_user';
  private readonly TOKEN_EXPIRY_KEY = 'token_expiry';
  private isBrowser: boolean;

  private authStatusSubject = new BehaviorSubject<AuthStatus>({
    isAuthenticated: false,
    token: null,
    user: null
  });

  public authStatus$: Observable<AuthStatus> = this.authStatusSubject.asObservable();

  constructor(@Inject(PLATFORM_ID) platformId: Object) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.loadAuthStatus();
  }

  private loadAuthStatus(): void {
    const token = this.getToken();
    const user = this.getUser();
    
    if (token && !this.isTokenExpired()) {
      this.authStatusSubject.next({
        isAuthenticated: true,
        token,
        user
      });
    } else if (token && this.isTokenExpired()) {
      this.clearAuthData();
    }
  }

  setAuthData(token: string, refreshToken: string, user: AuthUser, expiresIn: number): void {
    const expiryTime = Date.now() + (expiresIn * 1000);
    
    if (this.isBrowser) {
      try {
        localStorage.setItem(this.TOKEN_KEY, token);
        localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
        localStorage.setItem(this.USER_KEY, JSON.stringify(user));
        localStorage.setItem(this.TOKEN_EXPIRY_KEY, expiryTime.toString());
      } catch (error) {
        console.error('Failed to save auth data to storage:', error);
      }
    }

    this.authStatusSubject.next({
      isAuthenticated: true,
      token,
      user
    });
  }

  updateToken(token: string, refreshToken: string, expiresIn: number): void {
    const expiryTime = Date.now() + (expiresIn * 1000);
    
    if (this.isBrowser) {
      try {
        localStorage.setItem(this.TOKEN_KEY, token);
        localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
        localStorage.setItem(this.TOKEN_EXPIRY_KEY, expiryTime.toString());
      } catch (error) {
        console.error('Failed to update token in storage:', error);
      }
    }

    const currentStatus = this.authStatusSubject.value;
    this.authStatusSubject.next({
      ...currentStatus,
      token
    });
  }

  clearAuthData(): void {
    if (this.isBrowser) {
      try {
        localStorage.removeItem(this.TOKEN_KEY);
        localStorage.removeItem(this.REFRESH_TOKEN_KEY);
        localStorage.removeItem(this.USER_KEY);
        localStorage.removeItem(this.TOKEN_EXPIRY_KEY);
      } catch (error) {
        console.error('Failed to clear auth data from storage:', error);
      }
    }

    this.authStatusSubject.next({
      isAuthenticated: false,
      token: null,
      user: null
    });
  }

  getToken(): string | null {
    if (this.isBrowser) {
      try {
        return localStorage.getItem(this.TOKEN_KEY);
      } catch (error) {
        console.error('Failed to get token from storage:', error);
        return null;
      }
    }
    return null;
  }

  getRefreshToken(): string | null {
    if (this.isBrowser) {
      try {
        return localStorage.getItem(this.REFRESH_TOKEN_KEY);
      } catch (error) {
        console.error('Failed to get refresh token from storage:', error);
        return null;
      }
    }
    return null;
  }

  getUser(): AuthUser | null {
    if (this.isBrowser) {
      try {
        const userJson = localStorage.getItem(this.USER_KEY);
        return userJson ? JSON.parse(userJson) : null;
      } catch (error) {
        console.error('Failed to get user from storage:', error);
        return null;
      }
    }
    return null;
  }

  isAuthenticated(): boolean {
    return this.authStatusSubject.value.isAuthenticated && !this.isTokenExpired();
  }

  isTokenExpired(): boolean {
    if (!this.isBrowser) return true;
    
    try {
      const expiryTime = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
      if (!expiryTime) return true;
      
      return Date.now() >= parseInt(expiryTime, 10);
    } catch (error) {
      console.error('Failed to check token expiry:', error);
      return true;
    }
  }

  isTokenExpiringSoon(thresholdSeconds: number = 300): boolean {
    if (!this.isBrowser) return true;
    
    try {
      const expiryTime = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
      if (!expiryTime) return true;
      
      const timeUntilExpiry = parseInt(expiryTime, 10) - Date.now();
      return timeUntilExpiry <= (thresholdSeconds * 1000);
    } catch (error) {
      console.error('Failed to check token expiry:', error);
      return true;
    }
  }

  getAuthStatus(): AuthStatus {
    return this.authStatusSubject.value;
  }
}
