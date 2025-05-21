using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoneyTrackerApp.Models
{
    public class Income : BaseModel
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
