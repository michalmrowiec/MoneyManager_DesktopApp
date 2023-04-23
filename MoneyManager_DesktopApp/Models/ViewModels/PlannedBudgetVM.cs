using System;
using MoneyManager_DesktopApp.Models.ViewModels.Interfaces;

namespace MoneyManager_DesktopApp.Models.ViewModels
{
    public class PlannedBudgetVM : IRecord, IId, IRecordWithDate
    {
        public int Id { get; set; }
        /// <summary>
        /// Category Name
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Plan For Month
        /// </summary>
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }
        public int? CategoryId { get; set; }

    }
}
