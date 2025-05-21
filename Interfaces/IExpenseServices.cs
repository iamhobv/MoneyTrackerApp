using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Models;
using System.Linq.Expressions;

namespace MoneyTrackerApp.Interfaces
{
    public interface IExpenseServices
    {
        public void Add(Expense Item);

        public IEnumerable<Expense> GetFilter(Expression<Func<Expense, bool>> expression);
        public IEnumerable<Expense> GetAll();

        public Expense GetByID(int id);

        public void Remove(int id);

        public void Update(Expense Item);

        public void Save();
        public Task<IEnumerable<GetIExpenseDTO>> GetAllByUser(string UserName);
        public Task<IEnumerable<GetIExpenseDTO>> GetAllByCategoryUser(string UserName, int CategoryId);

        public ExpenseByIdDTO GetByIDSelected(int id);
    }
}
