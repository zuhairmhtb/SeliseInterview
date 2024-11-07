CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Status] VARCHAR(50) NOT NULL, 
    [CreatedAt] DATETIME NOT NULL DEFAULT (getdate()), 
    [Total] DECIMAL NULL
)
