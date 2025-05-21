using MoneyTrackerApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.DTO
{
    public class EditIncomeDTO

    {
        public int Id { get; set; }
        public double? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public int? Category_Id { get; set; }
        public string UserId { get; set; }

    }
}
