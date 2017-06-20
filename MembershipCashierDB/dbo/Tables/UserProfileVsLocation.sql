CREATE TABLE [dbo].[UserProfileVsLocation] (
    [UserId]     INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_UserProfileVsLocation] PRIMARY KEY CLUSTERED ([UserId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_UserProfileVsLocation_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_UserProfileVsLocation_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);

