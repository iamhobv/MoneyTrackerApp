using MoneyTrackerApp.Models;
using static MoneyTrackerApp.Enums.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.DTO
{
    public class AddRecurringTransactionDTO
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public RecurringTransactionFrequency Frequency { get; set; }
        public DateTime? EndDate { get; set; }
        public int? hour { get; set; }
        public DayOfWeek? dayOfWeek { get; set; }
        public int? day { get; set; }
        public int? month { get; set; }

        public int Category_Id { get; set; }

        public string User_Id { get; set; }

    }
}
