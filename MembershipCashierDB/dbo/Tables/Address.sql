CREATE TABLE [dbo].[Address] (
    [AddressId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [Address]       NVARCHAR (100) NULL,
    [PostalCode]    VARCHAR (9)    NULL,
    [City]          NVARCHAR (35)  NULL,
    [Province]      CHAR (2)       NULL,
    [ProvinceName]  NVARCHAR (50)  NULL,
    [Country]       NVARCHAR (2)   NULL,
    [IsResidential] BIT            NULL,
    [ValidityLevel] TINYINT        NOT NULL,
    [ValidatedTime] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressId] ASC)
);

