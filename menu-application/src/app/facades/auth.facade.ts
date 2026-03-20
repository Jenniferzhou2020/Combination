import { Injectable } from '@angular/core';
import { Observable, throwError, timer } from 'rxjs';
import { catchError, switchMap, tap, map } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { AuthState } from '../store/auth.state';
import { AuthStatus, AuthUser, LoginRequest, LoginResponse, RefreshTokenResponse } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthFacade {
  public authStatus$: Observable<AuthStatus>;
  private refreshTokenTimer: any;

  constructor(
    private authService: AuthService,
    private authState: AuthState
  ) {
    this.authStatus$ = this.authState.authStatus$;
    this.initializeTokenRefresh();
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.authService.login(credentials).pipe(
      tap(response => {
        this.authState.setAuthData(
          response.token,
          response.refreshToken,
          response.user,
          response.expiresIn
        );
        this.scheduleTokenRefresh(response.expiresIn);
      }),
      catchError(error => {
        console.error('Login failed:', error);
        return throwError(() => error);
      })
    );
  }

  logout(): Observable<void> {
    this.cancelTokenRefresh();
    this.authState.clearAuthData();
    return this.authService.logout();
  }

  refreshToken(): Observable<RefreshTokenResponse> {
    const refreshToken = this.authState.getRefreshToken();
    
    if (!refreshToken) {
      this.logout();
      return throwError(() => new Error('No refresh token available'));
    }

    return this.authService.refreshToken(refreshToken).pipe(
      tap(response => {
        this.authState.updateToken(
          response.token,
          response.refreshToken,
          response.expiresIn
        );
        this.scheduleTokenRefresh(response.expiresIn);
      }),
      catchError(error => {
        console.error('Token refresh failed:', error);
        this.logout();
        return throwError(() => error);
      })
    );
  }

  private initializeTokenRefresh(): void {
    if (this.authState.isAuthenticated() && this.authState.isTokenExpiringSoon()) {
      this.refreshToken().subscribe();
    }
  }

  private scheduleTokenRefresh(expiresIn: number): void {
    this.cancelTokenRefresh();
    
    // Refresh token 5 minutes before expiry
    const refreshTime = (expiresIn - 300) * 1000;
    
    if (refreshTime > 0) {
      this.refreshTokenTimer = timer(refreshTime).pipe(
        switchMap(() => this.refreshToken())
      ).subscribe();
    }
  }

  private cancelTokenRefresh(): void {
    if (this.refreshTokenTimer) {
      this.refreshTokenTimer.unsubscribe();
      this.refreshTokenTimer = null;
    }
  }

  isAuthenticated(): boolean {
    return this.authState.isAuthenticated();
  }

  getUser(): AuthUser | null {
    return this.authState.getUser();
  }

  getToken(): string | null {
    return this.authState.getToken();
  }
}
