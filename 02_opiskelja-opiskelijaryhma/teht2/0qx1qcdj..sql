--DROP DATABASE Opiskelijat
--GO

CREATE DATABASE Opiskelijat
GO

USE Opiskelijat;
GO


CREATE TABLE opiskelijaryhmat (
    ID INT IDENTITY(1,1) PRIMARY KEY,
	nimi varchar(50),
);

CREATE TABLE opiskelijat (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    etunimi VARCHAR(50),
    sukunimi VARCHAR(50),
    ryhma_id INT
);
SET IDENTITY_INSERT opiskelijat OFF;
SET IDENTITY_INSERT opiskelijaryhmat ON;

INSERT INTO opiskelijaryhmat (ID, nimi)
VALUES
(1, 'Ryhmä 1'),
(2, 'Ryhmä 2'),
(3, 'Ryhmä 3');
SET IDENTITY_INSERT opiskelijaryhmat OFF;
SET IDENTITY_INSERT opiskelijat ON;
INSERT INTO opiskelijat (ID, etunimi, sukunimi, ryhma_id) 
VALUES 
(1, 'Matti', 'Meikäläinen', 1),
(2, 'Maija', 'Mallikas', 2),
(3, 'Pekka', 'Kuusi', 3),
(4, 'Liisa', 'Kuusela', 1),
(5, 'Jussi', 'Jokunen', 2),
(6, 'Kaisa', 'Kuusinen', 3),
(7, 'Johanna', 'Jokunen', 1),
(8, 'Mikko', 'Mallikas', 2),
(9, 'Maija', 'Kuusela', 3),
(10, 'Matti', 'Kuusi', 1)