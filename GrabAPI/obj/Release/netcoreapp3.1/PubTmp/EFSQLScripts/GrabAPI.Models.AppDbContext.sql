IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [AccountType] (
        [Id] int NOT NULL IDENTITY,
        [AType1] nvarchar(max) NULL,
        CONSTRAINT [PK_AccountType] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [ItemTypes] (
        [Id] int NOT NULL IDENTITY,
        [itemType] nvarchar(max) NULL,
        CONSTRAINT [PK_ItemTypes] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [Stores] (
        [Id] int NOT NULL IDENTITY,
        [StoreName] nvarchar(max) NULL,
        CONSTRAINT [PK_Stores] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Email] nvarchar(max) NULL,
        [Name] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        [ContactNumber] nvarchar(max) NULL,
        [Image] nvarchar(max) NULL,
        [AccountType] int NOT NULL,
        [Unit] int NULL,
        [Street] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [PostalCode] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_AccountType_AccountType] FOREIGN KEY ([AccountType]) REFERENCES [AccountType] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [Items] (
        [Id] int NOT NULL IDENTITY,
        [ItemTypeID] int NULL,
        [Cost] float NOT NULL,
        [Weight] float NOT NULL,
        [Name] nvarchar(max) NULL,
        [Image] nvarchar(max) NULL,
        [StoreId] int NOT NULL,
        CONSTRAINT [PK_Items] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Items_ItemTypes_ItemTypeID] FOREIGN KEY ([ItemTypeID]) REFERENCES [ItemTypes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Items_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [Stores] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [StoreAddresses] (
        [Id] int NOT NULL IDENTITY,
        [Unit] int NULL,
        [Street] nvarchar(max) NULL,
        [PostalCode] nvarchar(max) NULL,
        [StoreID] int NULL,
        CONSTRAINT [PK_StoreAddresses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StoreAddresses_Stores_StoreID] FOREIGN KEY ([StoreID]) REFERENCES [Stores] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [Orders] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [OrderTotal] float NOT NULL,
        [Status] nvarchar(max) NULL,
        [PaymentStatus] nvarchar(max) NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [Carts] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [ItemId] int NOT NULL,
        [StoreId] int NOT NULL,
        [Count] int NOT NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Carts_Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Items] ([Id]),
        CONSTRAINT [FK_Carts_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [Stores] ([Id]),
        CONSTRAINT [FK_Carts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE TABLE [OrderDetails] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [ItemId] int NOT NULL,
        [ServiceId] int NULL,
        [Count] int NOT NULL,
        [StoreId] int NOT NULL,
        [Price] float NOT NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]),
        CONSTRAINT [FK_OrderDetails_Items_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [Items] ([Id]),
        CONSTRAINT [FK_OrderDetails_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [Stores] ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_Carts_ItemId] ON [Carts] ([ItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_Carts_StoreId] ON [Carts] ([StoreId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_Items_ItemTypeID] ON [Items] ([ItemTypeID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_Items_StoreId] ON [Items] ([StoreId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_OrderDetails_ServiceId] ON [OrderDetails] ([ServiceId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_OrderDetails_StoreId] ON [OrderDetails] ([StoreId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_StoreAddresses_StoreID] ON [StoreAddresses] ([StoreID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    CREATE INDEX [IX_Users_AccountType] ON [Users] ([AccountType]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321054722_Create Database')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200321054722_Create Database', N'3.1.2');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    ALTER TABLE [OrderDetails] DROP CONSTRAINT [FK_OrderDetails_Items_ServiceId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    DROP INDEX [IX_OrderDetails_ServiceId] ON [OrderDetails];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderDetails]') AND [c].[name] = N'ServiceId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [OrderDetails] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [OrderDetails] DROP COLUMN [ServiceId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AccountType]') AND [c].[name] = N'AType1');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AccountType] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [AccountType] DROP COLUMN [AType1];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    ALTER TABLE [AccountType] ADD [AcoountType] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    CREATE INDEX [IX_OrderDetails_ItemId] ON [OrderDetails] ([ItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    ALTER TABLE [OrderDetails] ADD CONSTRAINT [FK_OrderDetails_Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Items] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200321060409_Column Changes')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200321060409_Column Changes', N'3.1.2');
END;

GO

