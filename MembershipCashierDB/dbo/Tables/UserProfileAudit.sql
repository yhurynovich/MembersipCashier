CREATE TABLE [dbo].[UserProfileAudit] (
    [AuditId]          INT           IDENTITY (1, 1) NOT NULL,
    [ModifiedBy]       INT           NOT NULL,
    [ModificationTime] DATETIME      NOT NULL,
    [Action]           TINYINT       NOT NULL,
    [UserId]           INT           NOT NULL,
    [UserStatusId]     TINYINT       NOT NULL,
    [UserName]         NVARCHAR (56) NOT NULL,
    [EmailId]          VARCHAR (70)  NULL,
    [FirstName]        VARCHAR (35)  NULL,
    [LastName]         VARCHAR (35)  NULL,
    CONSTRAINT [PK_UserProfileAudit] PRIMARY KEY CLUSTERED ([AuditId] ASC),
    CONSTRAINT [FK_UserProfileAudit_UserProfile] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [FK_UserProfileAudit_UserProfile1] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);

