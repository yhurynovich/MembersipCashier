CREATE TABLE [dbo].[webpages_UsersInRolesAudit] (
    [ModifiedBy]       INT      NOT NULL,
    [ModificationTime] DATETIME NOT NULL,
    [Action]           TINYINT  NOT NULL,
    [UserId]           INT      NOT NULL,
    [RoleId]           INT      NOT NULL,
    CONSTRAINT [PK_webpages_UsersInRolesAudit] PRIMARY KEY CLUSTERED ([ModifiedBy] ASC, [ModificationTime] ASC),
    CONSTRAINT [FK_webpages_UsersInRolesAudit_UserProfile] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [FK_webpages_UsersInRolesAudit_UserProfile1] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [FK_webpages_UsersInRolesAudit_webpages_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId])
);

