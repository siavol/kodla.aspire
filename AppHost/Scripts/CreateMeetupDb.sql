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
    [Date] DATETIME2 NOT NULL
);
GO
CREATE TABLE [Attendee] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [MeetupId] INT NOT NULL,
    FOREIGN KEY (MeetupId) REFERENCES Meetup(Id)
);
GO
CREATE TABLE [Slot] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [MeetupId] INT NOT NULL,
    [AttendeeId] INT NULL, -- Nullable to allow unassigned slots
    [Version] ROWVERSION NOT NULL, -- ROWVERSION for concurrency control
    FOREIGN KEY (MeetupId) REFERENCES Meetup(Id),
    FOREIGN KEY (AttendeeId) REFERENCES Attendee(Id)
);
GO

-- Seeding data
INSERT INTO [Meetup] ([Name], [Description], [Date])
VALUES
    ('Best Meetup', 'The best meetup in Uusimmaa area. Learn some cool stuff.', '2025-06-10 10:00:00');
INSERT INTO [Slot] ([MeetupId])
VALUES
    (1), (1), (1), (1), (1)