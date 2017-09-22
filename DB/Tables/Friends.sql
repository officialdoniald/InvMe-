CREATE TABLE [dbo].[Friends]
(
	[id] INT NOT NULL IDENTITY,
	[SUID] INT NOT NULL,
	[GUID] INT NOT NULL,
    CONSTRAINT [pk_friends] PRIMARY KEY ([id]), 
    CONSTRAINT [FK_Friends_ToUserSend] FOREIGN KEY ([SUID]) REFERENCES [User]([id]), 
    CONSTRAINT [FK_Friends_ToUserGet] FOREIGN KEY ([GUID]) REFERENCES [User]([id]), 
)
