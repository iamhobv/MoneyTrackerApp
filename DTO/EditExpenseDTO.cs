using MoneyTrackerApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoneyTrackerApp.DTO
{

    public class EditExpenseDTO

    {

        public int Id { get; set; }
        public double? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public int? Category_Id { get; set; }
        public string UserId { get; set; }

    }
}
