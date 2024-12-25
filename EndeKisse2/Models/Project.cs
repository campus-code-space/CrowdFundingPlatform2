using System.ComponentModel.DataAnnotations;

namespace EndeKissie2.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; } // Primary Key
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;
        public bool Deleted { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = null!;

        [Required]
        public string Size { get; set; } = null!;

        public byte[]? ProjectImage { get; set; } // direct DB store

        public DateTime NeedDeadLineTime { get; set; }

        public DateTime? SubmissionDeadLineTime { get; set; } // after being funded user fill this column to show his repaying date

        public double TargetAmount { get; set; }

        public string Description { get; set; } = null!;

        // Navigation Properties
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ImageStore>? Images { get; set; }

        public int? ProjectStatusId { get; set; }
        public ProjectStatus? ProjectStatus { get; set; }
        public int? ProgressId { get; set; }
        public Progress? Progress { get; set; }
    }
}
