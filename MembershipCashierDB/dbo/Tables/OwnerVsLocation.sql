CREATE TABLE [dbo].[OwnerVsLocation] (
    [OwnerId]    INT NOT NULL,
    [LocationId] INT NOT NULL,
    [IsCurrent]  BIT CONSTRAINT [DF_OwnerVsLocation_IsCurrent] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OwnerVsLocation] PRIMARY KEY CLUSTERED ([OwnerId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_OwnerVsLocation_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_OwnerVsLocation_Owner] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Owner] ([OwnerId])
);



