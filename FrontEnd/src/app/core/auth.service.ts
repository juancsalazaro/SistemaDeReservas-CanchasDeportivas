import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserDto } from '../models/user.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = `${environment.apiUrl}/auth`;
  constructor(private http: HttpClient) {}

  login(user: UserDto): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/login`, user).pipe(
      tap(res => localStorage.setItem('token', res.token))
    );
  }

  register(user: UserDto): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/register`, user);
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}