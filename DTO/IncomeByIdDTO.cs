namespace MoneyTrackerApp.DTO
{
    public class IncomeByIdDTO
    {
        public string UserId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }


        public string Category { get; set; }
    }
}
