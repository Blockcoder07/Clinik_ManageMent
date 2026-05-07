export interface Patient {
  id: number;
  name: string;
  mobileNumber: string;
}

export interface PatientCreateRequest {
  name: string;
  mobileNumber: string;
}
