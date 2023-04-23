using MoneyManager_DesktopApp.Models.Entities.Interfaces;

namespace MoneyManager_DesktopApp.Models.Entities
{
    public class Category : IIdentifier
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
