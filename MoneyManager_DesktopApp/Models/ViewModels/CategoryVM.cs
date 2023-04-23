using System.ComponentModel.DataAnnotations;
using MoneyManager_DesktopApp.Models.ViewModels.Interfaces;

namespace MoneyManager_DesktopApp.Models.ViewModels
{
    public class CategoryVM : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Category name is too long.")]
        public string Name { get; set; } = null!;
    }
}
