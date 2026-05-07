import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/core';
import { LoginComponent } from './features/login.component';
import { AppointmentsComponent } from './features/appointments.component';
import { DashboardComponent } from './features/dashboard.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'appointments', component: AppointmentsComponent, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/appointments', pathMatch: 'full' },
  { path: '**', redirectTo: '/appointments' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
