import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UsuarioServicioService } from '../../services/usuario-servicio.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  formLogin: FormGroup;
  hidePassword: boolean = true;
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private _snackBar: MatSnackBar,
    private _usuarioServicio: UsuarioServicioService
  ) {
    this.formLogin = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {}

  onLogin() {
    this.loading = true;

    const email = this.formLogin.value.email;
    const password = this.formLogin.value.password;

    this._usuarioServicio.getIniciarSesion(email, password).subscribe({
      next: (data) => {
        if (data.status) {
          const role = this._usuarioServicio.getRole();
          if (role === 'Administrador') {
            this.router.navigate(['pages/dashboard']);
          } else if (role === 'Empleado') {
            this.router.navigate(['pages/vender']);
          } else {
            this._snackBar.open("Rol no autorizado", 'Oops!', { duration: 3000 });
          }
        } else {
          this._snackBar.open("No se encontraron coincidencias", 'Oops!', { duration: 3000 });
        }
      },
      error: () => {
        this._snackBar.open("Hubo un error", 'Oops!', { duration: 3000 });
      },
      complete: () => {
        this.loading = false;
      }
    });
  }
}
