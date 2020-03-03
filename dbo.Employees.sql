CREATE TABLE [dbo].[Employees]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[Code] INT NOT NULL,
	[Name] NVARCHAR(MAX) COLLATE Cyrillic_General_CI_AS NOT NULL,
    [Age] INT NOT NULL,
	[Salary] DECIMAL NOT NULL,
	[Department] INT NOT NULL,
    CONSTRAINT[PK_dbo.Employeees] PRIMARY KEY CLUSTERED([Id] ASC)
)

