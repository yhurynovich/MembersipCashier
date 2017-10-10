CREATE TABLE [dbo].[SquareCard] (
    [CardNonce]   VARBINARY (60) NOT NULL,
    [UserId]      INT            NOT NULL,
    [CreatedTime] DATETIME2 (7)  NOT NULL,
    [IsCurrent]   BIT            CONSTRAINT [DF_SquareCard_IsCurrent] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SquareCard] PRIMARY KEY CLUSTERED ([CardNonce] ASC),
    CONSTRAINT [FK_SquareCard_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);

