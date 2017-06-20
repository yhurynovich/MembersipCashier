CREATE TABLE [dbo].[ProductPriceHistory] (
    [ProductId]  INT   NOT NULL,
    [ChangeDate] DATE  NOT NULL,
    [Price]      MONEY NOT NULL,
    CONSTRAINT [PK_ProductPriceHistory] PRIMARY KEY CLUSTERED ([ProductId] ASC, [ChangeDate] ASC),
    CONSTRAINT [FK_ProductPriceHistory_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);



