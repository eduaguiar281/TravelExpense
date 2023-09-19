DELETE FROM Expenses
DELETE FROM Travels

SET IDENTITY_INSERT travels ON

DECLARE @StartDateTime datetime2 = Getdate()-2
DECLARE @EndDateTime datetime2 = Getdate()+2

INSERT INTO Travels(Id, [Description], EmployeeRegistration, EmployeeName, StartedIn, EndedIn, [Status], TotalExpenses, CreatedAt, UpdatedAt)
            Values (1, 'Viagem para visita cliente em Madrid', '22-56829-4', 'Eduardo Aguiar',@StartDateTime, @EndDateTime, 'Open', 0.00, getdate(), getdate())

SET @StartDateTime = Getdate()+3
SET @EndDateTime = Getdate()+6

INSERT INTO Travels(Id, [Description], EmployeeRegistration, EmployeeName, StartedIn, EndedIn, [Status], TotalExpenses, CreatedAt, UpdatedAt)
            Values (2, 'Viagem para visita cliente em Lisboa', '22-56829-4', 'Eduardo Aguiar',@StartDateTime, @EndDateTime, 'Open', 1500.00, getdate(), getdate())

SET IDENTITY_INSERT travels OFF


SET IDENTITY_INSERT Expenses ON


INSERT INTO Expenses(Id, RelatedTo, [Description], VoucherId, [Value], [Date], [Status], Comments, TravelId, CreatedAt, UpdatedAt) 
            Values  (1, 'Passagens aéreas', 'Passagens aérea AirFrance', null, 1500.00, @StartDateTime, 'Registered', 'A agencia possui apenas passagens classe vip', 2, getdate(), getdate())

SET IDENTITY_INSERT Expenses OFF

DBCC Checkident('travels', RESEED, 2)
DBCC Checkident('expenses', RESEED, 1)
