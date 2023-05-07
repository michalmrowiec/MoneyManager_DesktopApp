using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MoneyManager_DesktopApp.Models.ViewModels.Interfaces;

namespace MoneyManager_DesktopApp.Models.ViewModels
{
    public class RecordVM : IRecord, IId, IRecordWithDate
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Name is too short.")]
        [StringLength(50,ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = null!;
        public string? CategoryName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        /// <summary>
        /// Transaction Date
        /// </summary>
        public DateTime TransactionDate { get; set; }
        public int? CategoryId { get; set; }
        public CategoryVM? Category { get; set; }
    }
    
}
