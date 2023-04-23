using System;

namespace MoneyManager_DesktopApp.Models.ViewModels.Interfaces
{
    public interface IRecordWithDate
    {
        DateTime TransactionDate { get; set; }
    }
}
