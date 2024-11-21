-- CREATE DATABASE teht1_library;
-- GO

USE teht1_library;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Book' AND xtype='U')
BEGIN
    CREATE TABLE Book
    (
        BookId INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(255) NOT NULL,
        ISBN NVARCHAR(13) NOT NULL,
        PublicationYear INT,
        AvailableCopies INT NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Member' AND xtype='U')
BEGIN
    CREATE TABLE Member
    (
        MemberId INT PRIMARY KEY IDENTITY(1,1),
        FirstName NVARCHAR(255) NOT NULL,
        LastName NVARCHAR(255) NOT NULL,
        Address NVARCHAR(255),
        PhoneNumber NVARCHAR(20),
        Email NVARCHAR(255),
        RegistrationDate DATE NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Loan' AND xtype='U')
BEGIN
    CREATE TABLE Loan
    (
        LoanId INT PRIMARY KEY IDENTITY(1,1),
        BookId INT FOREIGN KEY REFERENCES Book(BookId),
        MemberId INT FOREIGN KEY REFERENCES Member(MemberId),
        LoanDate DATE NOT NULL,
        DueDate DATE NOT NULL,
        ReturnDate DATE
    );
END
GO



INSERT INTO Book (Title, ISBN, PublicationYear, AvailableCopies)
VALUES 
    ('To Kill a Mockingbird', '9780060935467', 1960, 3),
    ('1984', '9780451524935', 1949, 2),
    ('The Catcher in the Rye', '9780316769174', 1951, 4),
    ('Pride and Prejudice', '9780141439518', 1813, 5),
    ('The Great Gatsby', '9780743273565', 1925, 6),
    ('Moby Dick', '9781503280786', 1851, 3),
    ('The Hobbit', '9780547928227', 1937, 4),
    ('Brave New World', '9780060850524', 1932, 2),
    ('Fahrenheit 451', '9781451673319', 1953, 7),
    ('Animal Farm', '9780451526342', 1945, 3),
    ('The Lord of the Rings', '9780544003415', 1954, 5),
    ('Jane Eyre', '9780141441146', 1847, 2),
    ('Wuthering Heights', '9780141439556', 1847, 1);

INSERT INTO Member (FirstName, LastName, Address, PhoneNumber, Email, RegistrationDate)
VALUES 
    ('John', 'Doe', '123 Main St', '555-1234', 'john.doe@example.com', '2020-01-01'),
    ('Jane', 'Smith', '456 Elm St', '555-5678', 'jane.smith@example.com', '2021-05-15'),
    ('Alice', 'Johnson', '789 Maple St', '555-9876', 'alice.j@example.com', '2022-03-10'),
    ('Bob', 'Williams', '321 Oak St', '555-6543', 'bob.w@example.com', '2019-11-20'),
    ('Emily', 'Davis', '654 Pine St', '555-4321', 'emily.d@example.com', '2023-07-04'),
    ('Michael', 'Brown', '987 Cedar St', '555-1111', 'michael.b@example.com', '2018-09-15'),
    ('Chris', 'Wilson', '888 Spruce St', '555-3333', 'chris.w@example.com', '2022-10-10'),
    ('Jessica', 'Taylor', '111 Redwood St', '555-4444', 'jessica.t@example.com', '2020-06-30'),
    ('David', 'Anderson', '222 Chestnut St', '555-5555', 'david.a@example.com', '2019-02-14'),
    ('Laura', 'Martinez', '333 Willow St', '555-6666', 'laura.m@example.com', '2021-08-08'),
    ('Sophia', 'Clark', '555 Palm St', '555-8888', 'sophia.c@example.com', '2023-03-17');

INSERT INTO Loan (BookId, MemberId, LoanDate, DueDate)
VALUES 
    (1, 1, '2022-01-05', '2022-01-19'),
    (2, 2, '2022-02-10', '2022-02-24'),
    (3, 1, '2022-03-01', '2022-03-15'),
    (4, 3, '2022-04-10', '2022-04-24'),
    (5, 4, '2022-05-05', '2022-05-19'),
    (6, 5, '2022-06-15', '2022-06-29'),
    (7, 6, '2022-07-01', '2022-07-15'),
    (8, 7, '2022-08-10', '2022-08-24'),
    (9, 8, '2022-09-05', '2022-09-19'),
    (10, 9, '2022-10-01', '2022-10-15');

DECLARE @i INT = 1;
WHILE @i <= 10
BEGIN
    INSERT INTO Member (FirstName, LastName, Address, PhoneNumber, Email, RegistrationDate)
    VALUES ('Member_' + CAST(@i AS NVARCHAR), 
            'Lastname_' + CAST(@i AS NVARCHAR), 
            'Address_ ' + CAST(@i AS NVARCHAR), 
            '555-' + RIGHT('0000' + CAST(@i AS NVARCHAR), 4), 
            'member' + CAST(@i AS NVARCHAR) + '@example.com', 
            DATEADD(DAY, -@i * 10, GETDATE()));
    SET @i = @i + 1;
END
GO


DECLARE @j INT = 1;
WHILE @j <= 20
BEGIN
    INSERT INTO Book (Title, ISBN, PublicationYear, AvailableCopies)
    VALUES ('Book ' + CAST(@j AS NVARCHAR), 
            '978' + RIGHT('000000000' + CAST(@j AS NVARCHAR), 9),
            2000 + (@j % 121),
            1 + (@j % 10));
    SET @j = @j + 1;
END
GO
