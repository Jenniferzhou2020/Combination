import { Injectable } from '@angular/core';
import { AuthUser, LoginRequest } from './auth.modles';

@Injectable({ providedIn: 'root' })
export class TokenService {
  private accessTokenKey = 'access_token';
  private loginCredentialKey = 'login_credential';
  private loginUserKey = 'login_user';

  
  get accessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  set accessToken(token: string | null) {
    if (token) localStorage.setItem(this.accessTokenKey, token);
    else localStorage.removeItem(this.accessTokenKey);
  }

  get loginCredential(): LoginRequest | null {
    const logincredential = localStorage.getItem(this.loginCredentialKey);
    if (logincredential) {
      const credential = JSON.parse(logincredential);
      return credential;
    }
    return null;
  }

  set loginCredential(credential: LoginRequest | null) {
    if (credential) localStorage.setItem(this.loginCredentialKey, JSON.stringify( credential));
    else localStorage.removeItem(this.loginCredentialKey);
  }


  get loginUser(): AuthUser | null {
    const loginuser = localStorage.getItem(this.loginUserKey);
    if (loginuser) {
      const user = JSON.parse(loginuser);
      return user;
    }
    return null;
  }

  set loginUser(user: AuthUser | null) {
    if (user) localStorage.setItem(this.loginUserKey, JSON.stringify(user));
    else localStorage.removeItem(this.loginUserKey)
  }



  clear() {
    this.accessToken = null;
    this.loginCredential = null;
    this.loginUser = null;
  }
}
