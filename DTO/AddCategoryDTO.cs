using MoneyTrackerApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.DTO
{
    public class AddCategoryDTO
    {
        [Required]
        public string Name { get; set; }


        public string? User_Id { get; set; }

    }
}
