CREATE TABLE [dbo].[SquareUserProfile] (
    [SquareUserId] VARCHAR (60) NOT NULL,
    [UserId]       INT          NOT NULL,
    CONSTRAINT [PK_SquareUserProfile] PRIMARY KEY CLUSTERED ([SquareUserId] ASC),
    CONSTRAINT [FK_SquareUserProfile_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);

