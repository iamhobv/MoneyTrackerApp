using System.ComponentModel.DataAnnotations;

namespace MoneyTrackerApp.DTO
{
    public class LoginDTO
    {

        public string? UserName { get; set; }
        public string? Email { get; set; }





        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
