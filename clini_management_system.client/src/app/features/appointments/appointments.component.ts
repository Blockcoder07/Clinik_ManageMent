import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import {
  Appointment,
  AppointmentCreateRequest,
  AppointmentStatus,
  AppointmentsService,
  Patient,
  PatientsService
} from '../../core';

const DAY_MS = 86_400_000;
const DEFAULT_RANGE_DAYS = 30;

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css']
})
export class AppointmentsComponent implements OnInit {
  // #region Public Properties
  appointments: Appointment[] = [];
  patients: Patient[] = [];

  from = this.toDateInput(new Date());
  to = this.toDateInput(new Date(Date.now() + DEFAULT_RANGE_DAYS * DAY_MS));

  showForm = false;
  busy = false;
  errorMessage = '';

  newAppointment: AppointmentCreateRequest = this.emptyAppointment();
  // #endregion

  // #region Constructor
  constructor(
    private readonly appointmentsService: AppointmentsService,
    private readonly patientsService: PatientsService
  ) {}
  // #endregion

  // #region Lifecycle
  ngOnInit(): void {
    this.loadAppointments();
    this.loadPatients();
  }
  // #endregion

  // #region Public Methods
  loadAppointments(): void {
    this.appointmentsService.list(this.from, this.to).subscribe({
      next: result => (this.appointments = result.items),
      error: (err: HttpErrorResponse) => (this.errorMessage = err.error?.message ?? 'Failed to load appointments.')
    });
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
    if (this.showForm) this.newAppointment = this.emptyAppointment();
  }

  create(): void {
    this.busy = true;
    this.errorMessage = '';

    this.appointmentsService
      .create({
        patientId: this.newAppointment.patientId,
        doctorName: this.newAppointment.doctorName,
        appointmentDate: new Date(this.newAppointment.appointmentDate).toISOString()
      })
      .subscribe({
        next: () => {
          this.busy = false;
          this.showForm = false;
          this.loadAppointments();
        },
        error: (err: HttpErrorResponse) => {
          this.busy = false;
          this.errorMessage = err.error?.message ?? 'Failed to create appointment.';
        }
      });
  }

  updateStatus(appointment: Appointment, status: AppointmentStatus): void {
    this.appointmentsService.updateStatus(appointment.id, status, appointment.rowVersion).subscribe({
      next: updated => this.replaceAppointment(updated),
      error: (err: HttpErrorResponse) => (this.errorMessage = err.error?.message ?? 'Failed to update appointment.')
    });
  }

  statusLabel(status: AppointmentStatus): string {
    return AppointmentStatus[status];
  }

  isScheduled(status: AppointmentStatus): boolean {
    return status === AppointmentStatus.Scheduled;
  }
  // #endregion

  // #region Private Methods
  private loadPatients(): void {
    this.patientsService.list().subscribe({
      next: result => (this.patients = result.items)
    });
  }

  private replaceAppointment(updated: Appointment): void {
    const index = this.appointments.findIndex(a => a.id === updated.id);
    if (index >= 0) this.appointments[index] = updated;
  }

  private emptyAppointment(): AppointmentCreateRequest {
    return {
      patientId: 0,
      doctorName: '',
      appointmentDate: this.toDatetimeInput(new Date(Date.now() + DAY_MS))
    };
  }

  private toDateInput(date: Date): string {
    return date.toISOString().slice(0, 10);
  }

  private toDatetimeInput(date: Date): string {
    return date.toISOString().slice(0, 16);
  }

  // Expose enum to template
  protected readonly Status = AppointmentStatus;
  protected readonly CompletedStatus = AppointmentStatus.Completed;
  protected readonly CancelledStatus = AppointmentStatus.Cancelled;
  // #endregion
}
