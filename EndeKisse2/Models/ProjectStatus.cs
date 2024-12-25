using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EndeKissie.Models
{
    public class ProjectStatus
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public bool Banned { get; set; } = false;

        public bool Funded { get; set; } = false;

        public string? Description { get; set; }

        public DateTime? SubmissionDeadLineTime { get; set; } // after being funded user fill this column to show his repaying date

        public double FundedAmout { get; set; }
        // if the deadline passed without this property be 0 then send founder into to inverstots

        // Navigation Properties
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
