IF DB_ID(N'EnhanzerDb') IS NULL
BEGIN
    CREATE DATABASE EnhanzerDb;
END;
GO

USE EnhanzerDb;
GO

IF OBJECT_ID(N'dbo.Location_Details', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Location_Details
    (
        Id int IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Location_Details PRIMARY KEY,
        LocationCode nvarchar(max) NOT NULL,
        LocationName nvarchar(max) NOT NULL
    );
END;
GO
