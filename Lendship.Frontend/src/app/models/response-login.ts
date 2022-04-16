export interface LoginResponse {
  token: string;
  expiration: number;
  refreshToken: string | null;
}
