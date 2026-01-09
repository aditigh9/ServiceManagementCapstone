import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenService {

  set(token: string) {
    localStorage.setItem('token', token);
  }

  get() {
    return localStorage.getItem('token');
  }

  clear() {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.get();
  }

  getRole(): string | null {
    const token = this.get();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  }
}
