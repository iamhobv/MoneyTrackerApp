using System.ComponentModel.DataAnnotations.Schema;
using static MoneyTrackerApp.Enums.Enums;

namespace MoneyTrackerApp.Models
{
    public class RecurringTransaction : BaseModel
    {
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public RecurringTransactionFrequency Frequency { get; set; }
        public DateTime NextOccuranceDate { get; set; }
        public DateTime EndDate { get; set; }
        public RecurringTransactionStatus Status { get; set; }
        public string Name { get; set; }



        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        public Category? Category { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
