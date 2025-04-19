IF DB_ID('{{meetupDbName}}') IS NULL
    CREATE DATABASE [{{meetupDbName}}];
GO

-- Use the database
USE [{{meetupDbName}}];
GO

-- Create tables
CREATE TABLE [Meetup] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [Date] DATETIME2 NOT NULL,
    [MaxAttendees] INT NOT NULL
);
GO
CREATE TABLE [Attendee] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [MeetupId] INT NOT NULL,
    FOREIGN KEY (MeetupId) REFERENCES Meetup(Id)
);
GO

-- Seeding data
INSERT INTO [Meetup] ([Name], [Description], [Date], [MaxAttendees])
VALUES
    ('Best Meetup', 'The best meetup in Uusimmaa area. Learn some cool stuff.', '2025-06-10 10:00:00', 20);
