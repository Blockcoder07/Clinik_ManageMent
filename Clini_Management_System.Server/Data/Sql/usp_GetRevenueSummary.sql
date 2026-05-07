/************************************************************************************************
Header: Vishal Kumar
Procedure Name: usp_GetRevenueSummary
Description: Retrieves clinic revenue summary details
Created Date: 07-May-2026
Example: EXEC usp_GetRevenueSummary @ClinicId = 1, @From = '2026-05-01', @To = '2026-05-31'
************************************************************************************************/
CREATE  PROCEDURE dbo.usp_GetRevenueSummary
    @ClinicId INT,
    @From DATETIME2,
    @To DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CAST(ISNULL(SUM(i.Amount), 0) AS DECIMAL(18,2)) AS TotalRevenue,
        COUNT(a.Id) AS TotalAppointments,
        SUM(CASE WHEN a.Status = 2 THEN 1 ELSE 0 END) AS CompletedAppointments,
        SUM(CASE WHEN a.Status = 3 THEN 1 ELSE 0 END) AS CancelledAppointments
    FROM Appointments a
    LEFT JOIN Invoices i ON i.AppointmentId = a.Id AND i.ClinicId = a.ClinicId
    WHERE a.ClinicId = @ClinicId
      AND a.AppointmentDate >= @From
      AND a.AppointmentDate <= @To;
END;
