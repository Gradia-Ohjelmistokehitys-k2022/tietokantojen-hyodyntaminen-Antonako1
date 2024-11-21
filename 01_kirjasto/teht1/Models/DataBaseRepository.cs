using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using static teht1.Models.BookModel;

namespace teht1.Models
{


    internal class DataBaseRepository
    {
        private string _connectionString;

        public DataBaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string IsDBConnectionEstablished()
        {
            using var connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                return "Connection established!";
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }

            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }




        /*+++
        BOOK
        ---*/

        public List<BookModel.Book> BookQuery(string query="SELECT * FROM Book; ") 
        {
            List<BookModel.Book> books = new();
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();
            while (reader.Read()) 
            {
                BookModel.Book book = new()
                {
                    Id = Convert.ToInt32(reader["BookId"]),
                    Title = Convert.ToString(reader["Title"]),
                    ISBN = Convert.ToString(reader["ISBN"]),
                    PublicationYear = Convert.ToInt32(reader["PublicationYear"]),
                    AvailableCopies = Convert.ToInt32(reader["AvailableCopies"])
                };
                books.Add(book);
            }
            return books;
        }
        public List<BookModel.Book> GetAllBooksWithin5Years()
        {
            string current_year = (DateTime.Now.Year - 5).ToString();
            return BookQuery("SELECT * FROM Book Where PublicationYear > " + current_year + "; ");
        }


        public string GetMostBorrowedBook()
        {
            string query = @"
        SELECT TOP 1 
            B.Title, COUNT(L.BookId) AS LoanCount
        FROM 
            Loan L
        INNER JOIN 
            Book B ON L.BookId = B.BookId
        GROUP BY 
            B.Title
        ORDER BY 
            LoanCount DESC;
    ";

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return Convert.ToString(reader["Title"]) ?? "No books found";
            }

            return "No books found";
        }




        /*+++
         USER/MEMBER 
         ---*/

        public List<UserModel.User> UserQuery(string query = "SELECT * FROM Member; ")
        {
            List<UserModel.User> users = new();
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                UserModel.User user = new()
                {
                    Id = Convert.ToInt32(reader["MemberId"]),
                    FirstName = Convert.ToString(reader["FirstName"]),
                    LastName = Convert.ToString(reader["LastName"]),
                    Address = Convert.ToString(reader["Address"]),
                    PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                    Email = Convert.ToString(reader["Email"]),
                    RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"])
                };
                users.Add(user);
            }
            return users;
        }
        public Double GetAverageUserAge()
        {
            var users = UserQuery("SELECT * FROM Member; ");

            int count = users.Count();
            int amount = 0;
            foreach (var user in users)
            {
                amount += user.RegistrationDate!.Value.Year;
            }
            return Convert.ToDouble(amount / count);
        }




        /*+++
        LOAN
        ---*/
        public List<LoanModel.Loan> LoanQuery(string query = "SELECT * FROM Loan; ")
        {
            List<LoanModel.Loan> loans = new();
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                LoanModel.Loan loan = new()
                {
                    Id = Convert.ToInt32(reader["MemberId"]),
                    BookId = Convert.ToInt32(reader["BookId"]),
                    MemberId = Convert.ToInt32(reader["MemberId"]),
                    LoanDate = Convert.ToDateTime(reader["LoanDate"]),
                    DueDate = Convert.ToDateTime(reader["DueDate"]),
                    ReturnDate =
                        reader["ReturnDate"].GetType() == typeof(DBNull) 
                        ? null 
                        : Convert.ToDateTime(reader["ReturnDate"]),
                };
                loans.Add(loan);
            }
            return loans;
        }

        public void PrintMembersWithBorrowedBooks()
        {
            string query = @"
        SELECT 
            M.FirstName, 
            M.LastName, 
            B.ISBN
        FROM 
            Member M
        INNER JOIN 
            Loan L ON M.MemberId = L.MemberId
        INNER JOIN 
            Book B ON L.BookId = B.BookId
        ORDER BY 
            M.LastName, M.FirstName;
    ";

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string firstName = Convert.ToString(reader["FirstName"]);
                string lastName = Convert.ToString(reader["LastName"]);
                string isbn = Convert.ToString(reader["ISBN"]);

                Console.WriteLine($"{firstName} {lastName} borrowed book with ISBN: {isbn}");
            }
            connection.Close();
        }



        public void PrintTopThreeMostBorrowedBooks()
        {
            string query = @"
        SELECT TOP 3
            B.BookId,
            B.Title,
            B.ISBN,
            B.PublicationYear,
            B.AvailableCopies,
            COUNT(L.LoanId) AS LoanCount
        FROM 
            Book B
        INNER JOIN 
            Loan L ON B.BookId = L.BookId
        GROUP BY 
            B.BookId, B.Title, B.ISBN, B.PublicationYear, B.AvailableCopies
        ORDER BY 
            LoanCount DESC;
    ";

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            Console.WriteLine("Top 3 Most Borrowed Books:");
            Console.WriteLine("--------------------------------------------------");
            while (reader.Read())
            {
                int bookId = Convert.ToInt32(reader["BookId"]);
                string title = Convert.ToString(reader["Title"]);
                string isbn = Convert.ToString(reader["ISBN"]);
                int publicationYear = Convert.ToInt32(reader["PublicationYear"]);
                int availableCopies = Convert.ToInt32(reader["AvailableCopies"]);
                int loanCount = Convert.ToInt32(reader["LoanCount"]);

                Console.WriteLine($"Book ID: {bookId}, Title: {title}, ISBN: {isbn}, " +
                                  $"Publication Year: {publicationYear}, Available Copies: {availableCopies}, " +
                                  $"Loan Count: {loanCount}");
            }
            connection.Close();
        }
    }
}