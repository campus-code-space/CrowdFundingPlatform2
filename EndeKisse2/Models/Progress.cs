using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EndeKissie.Models
{

    public class Progress
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public int? ProgressLevel { get; set; }

        public string? Description { get; set; }

        // Navigation Properties
        public int? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
