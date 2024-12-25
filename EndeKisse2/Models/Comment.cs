using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EndeKissie2.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; } // Primary Key
        public string? Description { get; set; }
        public bool Deleted { get; set; }

        // Navigation Properties
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        
        public int? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
