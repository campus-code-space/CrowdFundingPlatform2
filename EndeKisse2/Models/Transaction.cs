using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EndeKissie2.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public string? Description { get; set; }

        public double? CalcShare { get; set; }

        public DateTime TransactionDate { get; set; }

        // Navigation Properties
        public string? SenderId { get; set; }
        [ForeignKey("SenderId")]
        public ApplicationUser? Sender { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
