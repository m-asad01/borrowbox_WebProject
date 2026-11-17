using Microsoft.AspNetCore.Identity;

namespace BorrowBox.Models
{
    // Our own user class. It adds two extra fields to the normal Identity user.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        // Admin can block a user. A blocked user cannot login.
        public bool IsBlocked { get; set; }
    }
}
