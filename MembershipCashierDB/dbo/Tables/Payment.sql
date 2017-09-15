CREATE TABLE [dbo].[Payment] (
    [CreditTransactionId] BIGINT      NOT NULL,
    [Sequence]            SMALLINT    NOT NULL,
    [PaymentMethod]       CHAR (1)    NOT NULL,
    [Amount]              MONEY       NULL,
    [Currency]            VARCHAR (3) NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED ([CreditTransactionId] ASC, [Sequence] ASC),
    CONSTRAINT [FK_Payment_CreditTransactions] FOREIGN KEY ([CreditTransactionId]) REFERENCES [dbo].[CreditTransactions] ([CreditTransactionId])
);







