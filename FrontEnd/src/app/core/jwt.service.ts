import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  decodeToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      const decodedPayload = atob(payload);
      return JSON.parse(decodedPayload);
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  getUserRole(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    const decoded = this.decodeToken(token);
    return decoded?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
           decoded?.role ||
           null;
  }

  getUsername(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    const decoded = this.decodeToken(token);
    return decoded?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
           decoded?.name ||
           null;
  }

  getUserId(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    const decoded = this.decodeToken(token);
    return decoded?.['http://schemas.xmlsoap.org/ws/2008/06/identity/claims/nameidentifier'] ||
           decoded?.sub ||
           null;
  }
}
