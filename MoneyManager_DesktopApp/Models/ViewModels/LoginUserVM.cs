using System.ComponentModel.DataAnnotations;

namespace MoneyManager_DesktopApp.Models.ViewModels
{
    public class LoginUserVM
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
