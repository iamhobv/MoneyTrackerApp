namespace MoneyTrackerApp.Enums
{
    public class Enums
    {
        public enum RecurringTransactionFrequency
        {
            Daily = 1,
            Weekly = 2,
            Monthly = 8,
            Yearly = 16
        }

        public enum RecurringTransactionStatus
        {
            Pause = 1,
            Resume = 2
        }

        public enum CategoryOwner
        {
            User = 1,
            App = 2,
            NotMatched = 4,
        }


    }
}
