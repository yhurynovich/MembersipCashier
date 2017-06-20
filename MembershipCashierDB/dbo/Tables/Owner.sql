CREATE TABLE [dbo].[Owner] (
    [OwnerId]     INT NOT NULL,
    [OwnerUserId] INT NULL,
    CONSTRAINT [PK_Owner] PRIMARY KEY CLUSTERED ([OwnerId] ASC),
    CONSTRAINT [FK_Owner_UserProfile] FOREIGN KEY ([OwnerUserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Owner Has User Profile Too', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Owner', @level2type = N'CONSTRAINT', @level2name = N'FK_Owner_UserProfile';

