using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagementBE.Models
{
    public struct UserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public ICollection<string>? Roles { get; set; }

        public UserViewModel(User user, ICollection<string> roles)
        {
            Id = user.Id.ToString();
            Username = user.UserName;
            Roles = roles;
        }

        public UserViewModel(User user)
        {
            Id = user.Id.ToString();
            Username = user.UserName;
        }
    }

    public class User: IdentityUser<Guid>
    {
        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        public static User Deserialize(string json)
        {
            return JsonSerializer.Deserialize<User>(json)!;
        }
    }
}
