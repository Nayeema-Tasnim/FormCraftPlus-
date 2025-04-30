using System;

namespace finalproject.Models
{
    public class UserAdminViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public DateTime? LockoutEnd { get; set; }
    }
}
