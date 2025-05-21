using Microsoft.AspNetCore.Identity;

namespace MoneyTrackerApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public double? ExpenseLimit { get; set; }
        public List<Expense>? Expenses { get; set; }
        public List<Income>? Incomes { get; set; }
        public List<Category>? Categories { get; set; }
        public List<RecurringTransaction>? RecurringTransactions { get; set; }
    }
}
