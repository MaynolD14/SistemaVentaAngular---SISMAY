import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private role: string | null = null; // Permitir que sea null inicialmente

  constructor(private http: HttpClient) {}

  login(credentials: any): Observable<any> {
    return this.http.post<any>('api/auth/login', credentials).pipe(
      map(response => {
        const token = response.token;
        const decodedToken: any = jwtDecode(token);
        this.role = decodedToken.role;
        localStorage.setItem('token', token);
        return response;
      })
    );
  }

  getRole(): string | null {
    // Si el rol no está en memoria, tratar de cargarlo desde el token en localStorage
    if (!this.role) {
      const token = localStorage.getItem('token');
      if (token) {
        const decodedToken: any = jwtDecode(token);
        this.role = decodedToken.role;
      }
    }
    return this.role;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    this.role = null;
  }
}
