namespace MoneyManager_DesktopApp.Models.ViewModels.Interfaces
{
    public interface IRecord : IId
    {
        //public int Id { get; set; }
        public decimal Amount { get; set; }

    }
}
