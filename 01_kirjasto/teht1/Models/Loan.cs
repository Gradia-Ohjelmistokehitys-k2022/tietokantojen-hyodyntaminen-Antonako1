using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace teht1.Models
{
    internal class LoanModel
    {
        [Table("Loan")]
        public class Loan
        {
            [Key]
            [Column("LoanId")]
            public int Id { get; set; }

            //[ForeignKey("BookId")]
            public int? BookId { get; set; }
            
            //[ForeignKey("MemberId")]
            public int? MemberId { get; set; }
            public DateTime LoanDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime? ReturnDate { get; set; }
        }
    }
}