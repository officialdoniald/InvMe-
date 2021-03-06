﻿CREATE TABLE [dbo].[Events]
(
	[id] INT NOT NULL IDENTITY,
	[EVENTNAME] NVARCHAR(MAX) NOT NULL,
	[DESCRIPTION] NVARCHAR(MAX) NULL,
	[FROM] DATE NULL,
	[TO] DATE NULL,
	[ONLINE] BINARY NOT NULL,
	[TOWN] NVARCHAR(100) NULL,
	[PLACE] NVARCHAR(100) NULL,
	[MDESCRIPTION] NVARCHAR(MAX) NULL,
	[HOWMANY] INT NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([id]) 
)
