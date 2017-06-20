CREATE TABLE [dbo].[CreditTransactions] (
    [CreditTransactionId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [LocationId]          INT           NOT NULL,
    [UserId]              INT           NOT NULL,
    [ProductId]           INT           CONSTRAINT [DF_CreditTransactions_ProductId] DEFAULT ((-1)) NOT NULL,
    [TransactionTime]     DATETIME2 (7) NOT NULL,
    [BallanceUnits]       SMALLMONEY    NOT NULL,
    [Description]         NTEXT         NULL,
    CONSTRAINT [PK_CreditTransactions] PRIMARY KEY CLUSTERED ([CreditTransactionId] ASC),
    CONSTRAINT [FK_CreditTransactions_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_CreditTransactions_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]),
    CONSTRAINT [FK_CreditTransactions_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);















