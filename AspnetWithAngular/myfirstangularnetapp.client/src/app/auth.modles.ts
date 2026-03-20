export interface LoginRequest { email: string; password: string; }
export interface LoginResponse {
  accessToken: string;
  expires: string;
  user: AuthUser;
}
export interface AuthUser {
  id: number,
  email: string,
  firstName: string,
  lastName: string,
  fullName: string,
  userRole?: Role
}

export interface Role {
  id: number,
  roleName: string,
}

export interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
