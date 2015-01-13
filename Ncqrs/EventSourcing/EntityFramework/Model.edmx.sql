
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/09/2010 16:01:00
-- Generated from EDMX file: C:\Workspaces\tfs02\TheKitchen\Cqrs\TheKitchen\Ncqrs\EventSourcing\EntityFramework\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SVLabEvents];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Events];
GO
IF OBJECT_ID(N'[dbo].[Snapshots]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Snapshots];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Data] varbinary(max)  NOT NULL,
    [AggregateId] uniqueidentifier  NOT NULL,
    [Version] bigint  NOT NULL,
    [Type] nchar(128)  NOT NULL
);
GO

-- Creating table 'Snapshots'
CREATE TABLE [dbo].[Snapshots] (
    [Data] varbinary(max)  NOT NULL,
    [AggregateId] uniqueidentifier  NOT NULL,
    [Version] bigint  NOT NULL,
    [Type] nchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AggregateId], [Version] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([AggregateId], [Version] ASC);
GO

-- Creating primary key on [AggregateId], [Version] in table 'Snapshots'
ALTER TABLE [dbo].[Snapshots]
ADD CONSTRAINT [PK_Snapshots]
    PRIMARY KEY CLUSTERED ([AggregateId], [Version] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------