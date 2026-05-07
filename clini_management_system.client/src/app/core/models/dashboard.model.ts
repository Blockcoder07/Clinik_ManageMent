export interface RevenueSummary {
  totalRevenue: number;
  totalAppointments: number;
  completedAppointments: number;
  cancelledAppointments: number;
}

export interface DateRange {
  from: string;
  to: string;
}
