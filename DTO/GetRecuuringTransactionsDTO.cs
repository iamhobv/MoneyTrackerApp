using MoneyTrackerApp.Models;
using static MoneyTrackerApp.Enums.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.DTO
{
    public class GetRecuuringTransactionsDTO
    {
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public RecurringTransactionFrequency Frequency { get; set; }
        public DateTime NextOccuranceDate { get; set; }
        public DateTime EndDate { get; set; }
        public RecurringTransactionStatus Status { get; set; }
        public string Name { get; set; }
        public int Category_Id { get; set; }

    }
}
