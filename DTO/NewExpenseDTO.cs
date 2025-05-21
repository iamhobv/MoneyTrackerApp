using MoneyTrackerApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.DTO
{
    public class NewExpenseDTO
    {
        [Required]
        public double Amount { get; set; }
        [Required]

        public DateTime Date { get; set; }

        public string? Description { get; set; }

        [Required]

        public int Category_Id { get; set; }

        [Required]

        public string User_Id { get; set; }


    }
}
