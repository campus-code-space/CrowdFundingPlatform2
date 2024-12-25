using Microsoft.AspNetCore.Identity;

namespace EndeKissie.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int AccountNum { get; set; }
        public byte[] UserImage { get; set; } = null!;
        public bool Deleted { get; set; }

        public List<ImageStore>? Image { get; set; }
        public List<Project>? Projects { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Transaction>? Transactions { get; set; }
    }
}
