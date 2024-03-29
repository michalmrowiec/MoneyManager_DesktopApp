﻿using System;
using MoneyManager_DesktopApp.Models.Entities.Interfaces;

namespace MoneyManager_DesktopApp.Models.Entities
{
    public class PlannedBudget : IIdentifier
    {
        public int Id { get; set; } 
        public DateTime PlanForMonth { get; set; }
        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
