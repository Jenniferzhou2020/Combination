export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthUser {
  id: string;
  email: string;
  name: string;
  role?: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  user: AuthUser;
  expiresIn: number; // in seconds
}

export interface RefreshTokenResponse {
  token: string;
  refreshToken: string;
  expiresIn: number;
}

export interface AuthStatus {
  isAuthenticated: boolean;
  token: string | null;
  user: AuthUser | null;
}
