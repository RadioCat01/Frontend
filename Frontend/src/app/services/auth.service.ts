import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, Location } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = '/api';
  private readonly tokenKey = 'enhanzer_token';
  private readonly locationsKey = 'enhanzer_locations';
  readonly isAuthenticated = signal(this.hasToken());

  constructor(private readonly http: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Auth/login`, credentials).pipe(
      tap((response) => {
        if (response.success && response.token) {
          localStorage.setItem(this.tokenKey, response.token);
          localStorage.setItem(this.locationsKey, JSON.stringify(response.locations ?? []));
          this.isAuthenticated.set(true);
        }
      })
    );
  }

  getLocations(): Observable<Location[]> {
    return this.http.get<Location[]>(`${this.apiUrl}/Locations`);
  }

  getStoredLocations(): Location[] {
    try {
      return JSON.parse(localStorage.getItem(this.locationsKey) ?? '[]') as Location[];
    } catch {
      return [];
    }
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.locationsKey);
    this.isAuthenticated.set(false);
  }

  private hasToken(): boolean {
    return Boolean(localStorage.getItem(this.tokenKey));
  }
}
