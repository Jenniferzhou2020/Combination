import { Injectable, effect, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { AuthUser, LoginRequest } from './auth.modles';
import { Subscription, timer } from 'rxjs';

type StorageLike = Pick<Storage, 'getItem' | 'setItem' | 'removeItem'>;

const STORAGE_KEYS = {
  access: 'auth.accessToken',
  //refresh: 'auth.refreshToken',
  loginCredential: 'login_credential',
  expiry: 'auth.expiresAt' // epoch ms
} as const;

@Injectable({ providedIn: 'root' })
export class AuthFacade {
  private api = inject(AuthService);
  private router = inject(Router);

  private store: StorageLike = localStorage; // switch to sessionStorage if you prefer

  // --- State (signals) ---
  private _user = signal<AuthUser | null>(null);
  private _accessToken = signal<string | null>(null);
  //private _refreshToken = signal<string | null>(null);
  private _expiresAt = signal<string | null>(null); // epoch ms
  private _loading = signal(false);
  private _error = signal<string | null>(null);

  private _credentials = signal<LoginRequest | null>(null);

  // --- Public read-only selectors ---
  user = this._user.asReadonly();
  accessToken = this._accessToken.asReadonly();
  isAuthenticated = signal<boolean>(false);
  loading = this._loading.asReadonly();
  error = this._error.asReadonly();
  credentials = this._credentials.asReadonly();

  private refreshSub?: Subscription;

  constructor() {
    // Keep isAuthenticated in sync
    //effect(() => {
    //  const token = this._accessToken();
    //  this.isAuthenticated.set(!!token);
   // });

    // Boot from storage on construction
    this.loadFromStorage();
  }

  // === Intents ===

  async login(credentials: LoginRequest) {
    this._error.set(null);
    this._loading.set(true);
    this._credentials.set(credentials);
    this.cancelRefreshTimer();

    this.api.login(credentials).subscribe({
      next: (res) => {
        const now = Date.now();
        const expiresAt = res.expires;
        this._accessToken.set(res.accessToken);
        this._expiresAt.set(expiresAt);
        this.isAuthenticated.set(true);
        this._user.set(res.user);
        this.persist();

        this.scheduleRefresh(expiresAt);
        this._loading.set(false);
        this.router.navigate(['']);
        //this.router.navigateByUrl('/'); // change to your home/dashboard
      },
      error: (err) => {
        this._loading.set(false);
        this._error.set(this.readError(err));
        this.isAuthenticated.set(false);
      }
    });
  }

  logout() {
    this._user.set(null);
    this._accessToken.set(null);
    this._credentials.set(null);
    this._expiresAt.set(null);
    this._error.set(null);
    this.persist(); // clears storage
    this.cancelRefreshTimer();
    this.router.navigateByUrl('/login');
  }

  /** Read tokens from storage and fetch /me if we have an accessToken */
  loadFromStorage() {
    const access = this.store.getItem(STORAGE_KEYS.access);
    const credentials = this.store.getItem(STORAGE_KEYS.loginCredential);
    const expiryStr = this.store.getItem(STORAGE_KEYS.expiry);

    if (!access || !credentials || !expiryStr) {
      this.clearStorage();
      return;
    }

    const expiresAt = (new Date(expiryStr)).getTime();
    if (Number.isNaN(expiresAt) || Date.now() >= expiresAt) {
      // expired; try to refresh once
     // const credential = this.store.getItem(STORAGE_KEYS.loginCredential);
      this.tryRefresh(JSON.parse(credentials));
      return;
    }

    this._accessToken.set(access);
    this._credentials.set(JSON.parse(credentials));
    this._expiresAt.set(expiryStr);

    // Fetch current user (optional if backend provides user in token)
    this._loading.set(true);
    this.api.me().subscribe({
      next: (u) => {
        this._user.set(u);
        this._loading.set(false);
        this.scheduleRefresh(expiryStr);
      },
      error: () => {
        // token invalid
        this._loading.set(false);
        this.logout();
      }
    });
  }

  // === Internals ===

  private tryRefresh(body: LoginRequest) {
    this._loading.set(true);
    this.api.login(body).subscribe({
      next: (res) => {
        //const now = Date.now();
        const expiresAt = res.expires;
        this._accessToken.set(res.accessToken);
        this._expiresAt.set(expiresAt);
        this._credentials.set(body);
        this.isAuthenticated.set(true);
        this.persist();
        this._loading.set(false);

        // Optionally refetch user if needed
        if (!this._user()) {
          this.api.me().subscribe({ next: (u) => this._user.set(u) });
        }

        this.scheduleRefresh(expiresAt);
      },
      error: () => {
        this._loading.set(false);
        this.isAuthenticated.set(true);
        this.logout();
      }
    });
  }

  private scheduleRefresh(expiresAt: string) {
    this.cancelRefreshTimer();
    const msUntilExpiry = (new Date(expiresAt)).getTime() - Date.now();
    // refresh 30 seconds before expiry (never below 0)
    const msUntilRefresh = Math.max(msUntilExpiry - 30_000, 0);

    this.refreshSub = timer(msUntilRefresh).subscribe(() => {
      const credential = this._credentials();
      if (credential) {
        this.tryRefresh(credential);
      } else {
        this.logout();
      }
    });
  }

  private cancelRefreshTimer() {
    this.refreshSub?.unsubscribe();
    this.refreshSub = undefined;
  }

  private persist() {
    const access = this._accessToken();
    const cred = this._credentials();
    const expires = this._expiresAt();

    if (access && cred && expires) {
      this.store.setItem(STORAGE_KEYS.access, access);
      this.store.setItem(STORAGE_KEYS.loginCredential, JSON.stringify( cred));
      this.store.setItem(STORAGE_KEYS.expiry, String(expires));
    } else {
      this.clearStorage();
    }
  }

  private clearStorage() {
    this.store.removeItem(STORAGE_KEYS.access);
    this.store.removeItem(STORAGE_KEYS.loginCredential);
    this.store.removeItem(STORAGE_KEYS.expiry);
  }

  private readError(err: any): string {
    if (err?.error?.message) return err.error.message;
    if (typeof err?.message === 'string') return err.message;
    return 'Login failed. Please try again.';
  }
}
