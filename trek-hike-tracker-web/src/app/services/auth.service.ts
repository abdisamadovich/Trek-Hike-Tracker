import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
  passwordConfirm: string;
}

export interface AuthResponse {
  userId: number;
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(
    this.getStoredAuth()
  );
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  private getStoredAuth(): AuthResponse | null {
    const stored = localStorage.getItem('auth');
    return stored ? JSON.parse(stored) : null;
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, data)
      .pipe(
        tap(response => {
          this.setAuth(response);
        })
      );
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, data)
      .pipe(
        tap(response => {
          this.setAuth(response);
        })
      );
  }

  private setAuth(auth: AuthResponse): void {
    localStorage.setItem('auth', JSON.stringify(auth));
    this.currentUserSubject.next(auth);
  }

  logout(): void {
    localStorage.removeItem('auth');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return this.getStoredAuth()?.accessToken || null;
  }

  isLoggedIn(): boolean {
    return !!this.getStoredAuth();
  }
}
