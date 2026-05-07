import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './core';
import { AppointmentsComponent } from './features/appointments/appointments.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { LoginComponent } from './features/login/login.component';

const routes: Routes = [
  { path: 'login',        component: LoginComponent },
  { path: 'appointments', component: AppointmentsComponent, canActivate: [AuthGuard] },
  { path: 'dashboard',    component: DashboardComponent,    canActivate: [AuthGuard] },
  { path: '',             redirectTo: '/appointments', pathMatch: 'full' },
  { path: '**',           redirectTo: '/appointments' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
