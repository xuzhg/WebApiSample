====================
Test DateTime, DateTimeOffset, smalldatetime with EF      
====================

The DB created by CODE FIRST from EF is:

```SQL
CREATE TABLE [dbo].[Customers] (
    [Id]             INT                IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (MAX)     NULL,
    [Birthday]       SMALLDATETIME      NOT NULL,
    [ExpirationDate] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
);
```

Thanks.
Sam. 

