import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Usuario } from '../interfaces/usuario';
import { ResponseApi } from '../interfaces/response-api';

@Injectable({
  providedIn: 'root'
})
export class UsuarioServicioService {
  apiBase: string = '/api/usuario/';
  private role: string | null = null;

  constructor(private http: HttpClient) {}

  getIniciarSesion(correo: string, clave: string): Observable<ResponseApi> {
    return this.http.get<ResponseApi>(`${this.apiBase}IniciarSesion?correo=${correo}&clave=${clave}`).pipe(
      map(response => {
        if (response.status) {
          const role = response.value.rol; // Ajusta para tomar el rol desde la respuesta
          this.role = role;
          localStorage.setItem('role', role);
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

  isLoggedIn(): boolean {
    return !!localStorage.getItem('role');
  }

  logout(): void {
    localStorage.removeItem('role');
    this.role = null;
  }

  getUsuarios(): Observable<ResponseApi> {
    return this.http.get<ResponseApi>(`${this.apiBase}Lista`);
  }

  saveUsuario(request: Usuario): Observable<ResponseApi> {
    return this.http.post<ResponseApi>(`${this.apiBase}Guardar`, request, { headers: { 'Content-Type': 'application/json;charset=utf-8' } });
  }

  editUsuario(request: Usuario): Observable<ResponseApi> {
    return this.http.put<ResponseApi>(`${this.apiBase}Editar`, request, { headers: { 'Content-Type': 'application/json;charset=utf-8' } });
  }

  deleteUsuario(id: number): Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.apiBase}Eliminar/${id}`);
  }
}
