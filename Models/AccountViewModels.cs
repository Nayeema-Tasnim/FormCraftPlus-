using System.ComponentModel.DataAnnotations;

namespace finalproject.Models.Account
{
    public class RegisterViewModel
    {
           [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name="Confirm password")]
        [Compare("Password", ErrorMessage="Password and confirmation do not match.")]
        public string? ConfirmPassword { get; set; }
    }
    }
    

     public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [Display(Name="Remember me?")]
        public bool RememberMe { get; set; }
    }

