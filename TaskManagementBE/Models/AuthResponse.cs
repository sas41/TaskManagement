namespace TaskManagementBE.Models
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Roles { get; set; }
        public string? Username { get; set; }
        public string? Id { get; set; }
        public string? Error { get; set; }
    }
}
