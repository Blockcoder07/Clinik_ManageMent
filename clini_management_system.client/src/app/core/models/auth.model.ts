export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  clinicName: string;
  username: string;
  password: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  username: string;
  role: string;
  clinicId: number;
}
