CREATE TABLE [dbo].[ProfileCredits] (
    [UserId]         INT           NOT NULL,
    [LocationId]     INT           NOT NULL,
    [ProductId]      INT           CONSTRAINT [DF_ProfileCredits_ProductId] DEFAULT ((-1)) NOT NULL,
    [BallanceUnits]  SMALLMONEY    NOT NULL,
    [Ballance]       MONEY         CONSTRAINT [DF_ProfileCredits_CreditedItemsCount] DEFAULT ((0)) NOT NULL,
    [CalculatedTime] DATETIME2 (7) CONSTRAINT [DF_ProfileCredits_CalculatedTime] DEFAULT (getdate()) NOT NULL,
    [HasBallance]    AS            (CONVERT([bit],case when [Ballance]=(0) AND [BallanceUnits]=(0) then (0) else (1) end)),
    CONSTRAINT [PK_ProfileCredits] PRIMARY KEY CLUSTERED ([UserId] ASC, [LocationId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_ProfileCredits_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_ProfileCredits_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]),
    CONSTRAINT [FK_ProfileCredits_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);














GO
CREATE NONCLUSTERED INDEX [ProfileCredits_HasBAllance]
    ON [dbo].[ProfileCredits]([HasBallance] ASC)
    INCLUDE([UserId], [ProductId]);

