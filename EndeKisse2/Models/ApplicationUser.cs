using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EndeKissie2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int AccountNum { get; set; }
        public string FaydaIdNum { get; set; } = null!;
        public byte[] UserImage { get; set; } = null!;
        public bool Deleted { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; } = null!;
        public string Residence { get; set; } = null!;
        public string role { get; set; } = null!;

        [NotMapped]
        public IFormFile? IdImageFile { get; set; }
        public string IdImageUrl { get; set; } = "https://via.placeholder.com/150";

        [NotMapped]
        public IFormFile? UserImageFile { get; set; }
        public string UserImageUrl { get; set; } = "https://via.placeholder.com/150";


        public List<ImageStore>? Image { get; set; }
        public List<Project>? Projects { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Transaction>? Transactions { get; set; }
    }
}
