/******************************************************************************
    Header        : Vishal Kumar
    Procedure     : dbo.usp_GetRevenueSummary
    Description   : Aggregates revenue and appointment counts for a single
                    clinic within an inclusive date range.
    Created Date  : 07-May-2026
    Example       : EXEC dbo.usp_GetRevenueSummary
                        @ClinicId = 1,
                        @From     = '2026-05-01',
                        @To       = '2026-05-31';
******************************************************************************/
CREATE OR ALTER PROCEDURE dbo.usp_GetRevenueSummary
    @ClinicId INT,
    @From     DATETIME2,
    @To       DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StatusCompleted INT = 2;
    DECLARE @StatusCancelled INT = 3;

    SELECT
        TotalRevenue          = CAST(ISNULL(SUM(i.Amount), 0) AS DECIMAL(18, 2)),
        TotalAppointments     = COUNT(a.Id),
        CompletedAppointments = SUM(CASE WHEN a.Status = @StatusCompleted THEN 1 ELSE 0 END),
        CancelledAppointments = SUM(CASE WHEN a.Status = @StatusCancelled THEN 1 ELSE 0 END)
    FROM       dbo.Appointments AS a
    LEFT JOIN  dbo.Invoices     AS i
            ON i.AppointmentId = a.Id
           AND i.ClinicId      = a.ClinicId
    WHERE a.ClinicId         = @ClinicId
      AND a.AppointmentDate >= @From
      AND a.AppointmentDate <= @To;
END;
GO
