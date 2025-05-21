using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Models;
using System.Linq.Expressions;

namespace MoneyTrackerApp.Interfaces
{
    public interface IRecurringTransactionServices
    {
        public void Add(RecurringTransaction Item);

        public IEnumerable<RecurringTransaction> GetAll();

        public RecurringTransaction GetByID(int id);

        public IEnumerable<RecurringTransaction> GetFilter(Expression<Func<RecurringTransaction, bool>> expression);
        public void Remove(int id);

        public void Update(RecurringTransaction Item);

        public void Save();
        public Task<IEnumerable<GetRecuuringTransactionsDTO>> GetAllByUser(string User_Id);
        public GetRecuuringTransactionsDTO GetAllByNameAndUser(string User_Id, string JobName);
        public RecurringTransaction GetAllByNameAndUserToUpdate(string User_Id, string JobName);

        public GetRecuuringTransactionsDTO GetByIDSelected(int id);
    }
}
