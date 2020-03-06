CREATE TABLE [dbo].[Departments] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Code] INT            DEFAULT ((0)) NOT NULL,
    [Name] NVARCHAR (MAX) COLLATE Cyrillic_General_CI_AS DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_dbo.Departments] PRIMARY KEY CLUSTERED ([Id] ASC)
);
