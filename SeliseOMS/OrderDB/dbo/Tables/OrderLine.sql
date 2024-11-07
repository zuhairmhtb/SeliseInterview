CREATE TABLE [dbo].[OrderLine]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderId] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL DEFAULT (0), 
    [UnitPrice] DECIMAL NOT NULL DEFAULT (0)
)
