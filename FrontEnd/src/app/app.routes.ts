import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { RegisterComponent } from './features/register/register.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { CanchaDetalleComponent } from './features/cancha-detalle/cancha-detalle.component';
import { AuthGuard } from './core/auth.guard';
import { MisReservasComponent } from './features/mis-reservas/mis-reservas.component';
import { CrearCanchaComponent } from './features/crear-cancha/crear-cancha.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'cancha/:id', component: CanchaDetalleComponent, canActivate: [AuthGuard] },
  { path: 'mis-reservas', component: MisReservasComponent, canActivate: [AuthGuard] },
  { path: 'crear-cancha', component: CrearCanchaComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'login' }
];
