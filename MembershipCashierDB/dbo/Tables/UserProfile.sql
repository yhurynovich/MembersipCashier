CREATE TABLE [dbo].[UserProfile] (
    [UserId]       INT           IDENTITY (1, 1) NOT NULL,
    [UserStatusId] TINYINT       NOT NULL,
    [UserName]     NVARCHAR (56) NOT NULL,
    [EmailId]      VARCHAR (70)  NULL,
    [FirstName]    VARCHAR (35)  NULL,
    [LastName]     VARCHAR (35)  NULL,
    [Photo]        VARCHAR (MAX) NULL,
    [Phone]        VARCHAR (20)  NULL,
    [LdapAccount]  VARCHAR (300) NULL,
    [PersonalId]   VARCHAR (50)  NULL,
    [AddressId]    BIGINT        NULL,
    CONSTRAINT [PK__UserProf__1788CC4C7F60ED59] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_UserProfile_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([AddressId]),
    CONSTRAINT [UQ__UserProf__C9F28456023D5A04] UNIQUE NONCLUSTERED ([UserName] ASC)
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_UserProfile_PersonalId]
    ON [dbo].[UserProfile]([PersonalId] ASC);

