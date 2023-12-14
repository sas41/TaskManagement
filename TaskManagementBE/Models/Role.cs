using Microsoft.AspNetCore.Identity;

namespace TaskManagementBE.Models
{
    public enum UserRole
    {
        Admin,
        Manager,
        User
    }

    public class Role : IdentityRole<Guid>
    {
        public Role() : base() { }
        public Role(string roleName) : base(roleName)
        {
            this.NormalizedName = roleName;
        }
    }
}
