CREATE TABLE [dbo].[Payment] (
    [PaymentId]     BIGINT      NOT NULL,
    [Sequence]      SMALLINT    NOT NULL,
    [PaymentMethod] CHAR (1)    NOT NULL,
    [Amount]        MONEY       NULL,
    [Currency]      VARCHAR (3) NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED ([PaymentId] ASC, [Sequence] ASC),
    CONSTRAINT [FK_Payment_CreditTransactions] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[CreditTransactions] ([CreditTransactionId])
);





