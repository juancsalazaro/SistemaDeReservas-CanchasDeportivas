import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserDto, LoginResponse } from '../models/user.dto';
import { JwtService } from './jwt.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = `${environment.apiUrl}/auth`;

  constructor(
    private http: HttpClient,
    private jwtService: JwtService
  ) {}

  login(user: UserDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, user).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        if (res.user) {
          localStorage.setItem('userInfo', JSON.stringify(res.user));
        }
      })
    );
  }

  register(user: UserDto): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/register`, user);
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userInfo');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserRole(): string | null {
    return this.jwtService.getUserRole();
  }

  getUsername(): string | null {
    return this.jwtService.getUsername();
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'Administrador';
  }

  isCliente(): boolean {
    return this.getUserRole() === 'Cliente';
  }

  canCreateCanchas(): boolean {
    return this.isAdmin();
  }

  canMakeReservations(): boolean {
    return this.isCliente();
  }
}
