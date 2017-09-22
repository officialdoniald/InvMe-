﻿CREATE TABLE [dbo].[User]
(
	[id] INT NOT NULL IDENTITY,
	[EMAIL] NVARCHAR(100) NOT NULL UNIQUE,
	[FIRSTNAME] NVARCHAR(50) NOT NULL,
	[LASTNAME] NVARCHAR(50) NOT NULL,
	[BORNDATE] DATETIME NULL,
	[PROFILEPICTURE] VARBINARY NULL,
	[FACEBOOK] NVARCHAR(200) NULL,
	[PASSWORD] NVARCHAR(200) NOT NULL,
    CONSTRAINT [pk_user] PRIMARY KEY ([id]) 
)
