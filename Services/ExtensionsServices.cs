using System.Linq;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Models;

namespace MoneyTrackerApp.Services
{
    public static class ExtensionsServices
    {
        public static double IncomeSum(this IEnumerable<GetIncomeDTO> income)
        {
            double sum = 0;
            foreach (var item in income)
            {
                sum += item.Amount;
            }
            return sum;
        }
        public static double ExpenseSum(this IEnumerable<GetIExpenseDTO> expense)
        {
            double sum = 0;
            foreach (var item in expense)
            {
                sum += item.Amount;
            }
            return sum;
        }


    }
}
