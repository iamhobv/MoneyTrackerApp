using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.Models
{
    public class Expense : BaseModel
    {
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }



        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        public Category? Category { get; set; }




        [ForeignKey("User")]
        public string User_Id { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
