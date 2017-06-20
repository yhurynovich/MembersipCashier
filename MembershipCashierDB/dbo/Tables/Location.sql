CREATE TABLE [dbo].[Location] (
    [LocationId]        INT            NOT NULL,
    [AddressId]         BIGINT         NULL,
    [LocationCode]      NVARCHAR (8)   NULL,
    [Description]       NVARCHAR (255) NULL,
    [TimeZoneCode]      VARCHAR (5)    NULL,
    [IsCredeitReversed] BIT            CONSTRAINT [DF_Location_IsCredeitReversed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([LocationId] ASC),
    CONSTRAINT [FK_Location_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([AddressId])
);







