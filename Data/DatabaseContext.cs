using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyTrackerApp.Models;

namespace MoneyTrackerApp.Data
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }


    }
}
