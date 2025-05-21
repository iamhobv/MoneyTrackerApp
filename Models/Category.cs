using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public List<Expense>? Expenses { get; set; }
        public List<Income>? Incomes { get; set; }
        public List<RecurringTransaction>? RecurringTransactions { get; set; }


        [ForeignKey("User")]
        public string? User_Id { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
