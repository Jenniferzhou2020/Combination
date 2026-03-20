import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, firstValueFrom, map, shareReplay, tap } from 'rxjs';
import { AuthUser, LoginRequest, LoginResponse, WeatherForecast } from './auth.modles'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private base = 'https://localhost:7263/api/auth';
  //private base = '/api/auth';
  constructor(private http: HttpClient) { }

  login(body: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.base}/login`, body);
  }

  me(): Observable<AuthUser> {
    return this.http.get<AuthUser>(`https://localhost:7263/api/users/me`);
  }
  getForecasts$(): Observable<WeatherForecast[]> {
    let loginRequest: LoginRequest;
    loginRequest = { email: 'g_gennifer@hotmail.com', password: 'Teranet1!' };

    const token = this.loginAndGetToken(loginRequest); // Observable<LoginResponse>
    return this.http.get<WeatherForecast[]>('/weatherforecast', { headers: { Authorization: 'Bearer ${token.accessToken}' } });
  }
  async loginAndGetToken(loginRequest: LoginRequest): Promise<LoginResponse> {
    const resp = await firstValueFrom(this.login(loginRequest).pipe(
      // tap(res => { this.accessToken = res.accessToken; })
    )); 
    return resp;
  }
  /*
  logout() {
    this.clear();
  }
   */
}
