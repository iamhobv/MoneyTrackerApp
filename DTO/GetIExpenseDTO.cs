using MoneyTrackerApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.DTO
{
    public class GetIExpenseDTO
    {
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }


        public string Category { get; set; }



    }
}
