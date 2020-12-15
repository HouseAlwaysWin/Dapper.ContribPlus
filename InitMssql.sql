CREATE DATABASE DapperContribPlusDB
GO
USE DapperContribPlusDB

CREATE TABLE TestTable
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR (Max) NOT NULL,
    Price INT NOT NULL
)
GO
