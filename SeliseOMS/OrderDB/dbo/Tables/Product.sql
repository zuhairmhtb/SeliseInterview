CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [UnitPrice] DECIMAL NOT NULL, 
    [AvailableQuantity] INT NOT NULL
)
GO;
