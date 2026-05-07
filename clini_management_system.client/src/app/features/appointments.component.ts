import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Appointment, AppointmentStatus, AppointmentsService, Patient, PatientsService } from '../core/core';

@Component({
  selector: 'app-appointments',
  template: `
    <div class="container">
      <div class="card">
        <h2>Appointments</h2>
        <div class="row">
          <label>From <input type="date" [(ngModel)]="from" /></label>
          <label>To <input type="date" [(ngModel)]="to" /></label>
          <button (click)="load()">Filter</button>
          <div style="flex: 1"></div>
          <button (click)="showForm = !showForm">{{ showForm ? 'Cancel' : '+ New Appointment' }}</button>
        </div>
      </div>

      <div class="card" *ngIf="showForm">
        <h3>New Appointment</h3>
        <div class="row">
          <select [(ngModel)]="newAppointment.patientId">
            <option [ngValue]="0" disabled>-- Select Patient --</option>
            <option *ngFor="let p of patients" [ngValue]="p.id">{{ p.name }} ({{ p.mobileNumber }})</option>
          </select>
          <input [(ngModel)]="newAppointment.doctorName" placeholder="Doctor Name" />
          <input type="datetime-local" [(ngModel)]="newAppointment.appointmentDate" />
          <button (click)="create()" [disabled]="busy">Create</button>
        </div>
        <p class="muted" *ngIf="patients.length === 0">No patients yet. Add one first via the Patients page (coming soon) or call <code>POST /api/patients</code>.</p>
        <p class="error" *ngIf="errorMessage">{{ errorMessage }}</p>
      </div>

      <div class="card">
        <table>
          <thead>
            <tr>
              <th>Patient</th>
              <th>Doctor</th>
              <th>Date</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let a of appointments">
              <td>{{ a.patientName }}</td>
              <td>{{ a.doctorName }}</td>
              <td>{{ a.appointmentDate | date:'medium' }}</td>
              <td>{{ statusLabel(a.status) }}</td>
              <td>
                <button class="secondary" (click)="updateStatus(a, 2)" *ngIf="a.status === 1">Complete</button>
                <button class="secondary" (click)="updateStatus(a, 3)" *ngIf="a.status === 1">Cancel</button>
              </td>
            </tr>
            <tr *ngIf="appointments.length === 0">
              <td colspan="5" class="muted" style="text-align: center;">No appointments in selected range.</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  `
})
export class AppointmentsComponent implements OnInit {
  appointments: Appointment[] = [];
  patients: Patient[] = [];
  from = new Date().toISOString().slice(0, 10);
  to = new Date(Date.now() + 30 * 86_400_000).toISOString().slice(0, 10);
  showForm = false;
  busy = false;
  errorMessage = '';

  newAppointment = {
    patientId: 0,
    doctorName: '',
    appointmentDate: new Date(Date.now() + 86_400_000).toISOString().slice(0, 16)
  };

  constructor(
    private readonly appointmentsService: AppointmentsService,
    private readonly patientsService: PatientsService
  ) {}

  ngOnInit(): void {
    this.load();
    this.loadPatients();
  }

  load(): void {
    this.appointmentsService.list(this.from, this.to).subscribe({
      next: result => this.appointments = result.items
    });
  }

  loadPatients(): void {
    this.patientsService.list().subscribe({ next: r => this.patients = r.items });
  }

  create(): void {
    this.busy = true;
    this.errorMessage = '';
    this.appointmentsService.create({
      patientId: this.newAppointment.patientId,
      doctorName: this.newAppointment.doctorName,
      appointmentDate: new Date(this.newAppointment.appointmentDate).toISOString()
    }).subscribe({
      next: () => { this.busy = false; this.showForm = false; this.load(); },
      error: (err: HttpErrorResponse) => { this.busy = false; this.errorMessage = err.error?.message ?? 'Failed to create.'; }
    });
  }

  updateStatus(appointment: Appointment, status: AppointmentStatus): void {
    this.appointmentsService.updateStatus(appointment.id, status, appointment.rowVersion).subscribe({
      next: updated => {
        const idx = this.appointments.findIndex(x => x.id === appointment.id);
        if (idx >= 0) this.appointments[idx] = updated;
      },
      error: (err: HttpErrorResponse) => { this.errorMessage = err.error?.message ?? 'Failed to update.'; }
    });
  }

  statusLabel(status: AppointmentStatus): string {
    return AppointmentStatus[status];
  }
}
