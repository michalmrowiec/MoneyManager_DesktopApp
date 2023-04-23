using System;
using MoneyManager_DesktopApp.Models.Entities.Interfaces;

namespace MoneyManager_DesktopApp.Models.Entities
{
    public class Record : IIdentifier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
