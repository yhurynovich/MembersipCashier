CREATE TABLE [dbo].[ProductVsLocation] (
    [ProductId]  INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_ProductVsLocation] PRIMARY KEY CLUSTERED ([ProductId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_ProductVsLocation_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_ProductVsLocation_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);

