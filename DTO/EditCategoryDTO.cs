using MoneyTrackerApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackerApp.DTO
{
    public class EditCategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }

    }
}
