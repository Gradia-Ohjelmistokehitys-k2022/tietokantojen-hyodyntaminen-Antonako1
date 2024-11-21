using System.Data.SqlClient;
using teht1.Models;

// See https://aka.ms/new-console-template for more information
namespace teht1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Trusted_Connection=true;server=(localdb)\\\\MSSQLLocalDB;database=teht1_library";
            var databaseRepository = new DataBaseRepository(connectionString);
            var text = databaseRepository.IsDBConnectionEstablished();
            Console.WriteLine(text);

            Console.WriteLine("Books within 5 years:");
            var results = databaseRepository.GetAllBooksWithin5Years();
            foreach (var book in results)
            {
                Console.WriteLine("  - '" + book.Title + "'," + book.PublicationYear);
            }

            Console.WriteLine("Average date: " + databaseRepository.GetAverageUserAge());
            Console.WriteLine("Most borrowed book: " + databaseRepository.GetMostBorrowedBook());
            databaseRepository.PrintMembersWithBorrowedBooks();
            databaseRepository.PrintTopThreeMostBorrowedBooks();


            databaseRepository.PrintBooksPublishedInLastTenYears();
        }
    }
}