export enum AppointmentStatus {
  Scheduled = 1,
  Completed = 2,
  Cancelled = 3
}

export interface Appointment {
  id: number;
  patientId: number;
  patientName: string;
  doctorName: string;
  appointmentDate: string;
  status: AppointmentStatus;
  rowVersion: string;
}

export interface AppointmentCreateRequest {
  patientId: number;
  doctorName: string;
  appointmentDate: string;
}

export interface AppointmentStatusUpdateRequest {
  status: AppointmentStatus;
  rowVersion: string;
}
