import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiBase: string = '/api/usuario/';
  private role: string | null = null;

  constructor(private http: HttpClient) {}

  login(credentials: { correo: string, clave: string }): Observable<any> {
    return this.http.get<any>(`${this.apiBase}IniciarSesion`, { params: credentials }).pipe(
      map(response => {
        if (response.status) {
          const usuario = response.value.usuario;
          this.role = response.value.rol;
          localStorage.setItem('usuario', JSON.stringify(usuario));
          localStorage.setItem('role', this.role as string); // Asegura que this.role no es null
        }
        return response;
      })
    );
  }

  getRole(): string | null {
    if (!this.role) {
      this.role = localStorage.getItem('role');
    }
    return this.role;
  }

  logout(): void {
    localStorage.removeItem('usuario');
    localStorage.removeItem('role');
    this.role = null;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('usuario');
  }
}
