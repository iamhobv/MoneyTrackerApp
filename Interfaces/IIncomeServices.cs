using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Models;
using System.Linq.Expressions;

namespace MoneyTrackerApp.Interfaces
{
    public interface IIncomeServices
    {
        public void Add(Income Item);

        public IEnumerable<Income> GetFilter(Expression<Func<Income, bool>> expression);

        public IEnumerable<Income> GetAll();
        public Task<IEnumerable<GetIncomeDTO>> GetAllByUser(string UserName);
        public Task<IEnumerable<GetIncomeDTO>> GetAllByCategoryUser(string UserName, int CategoryId);

        public Income GetByID(int id);
        public IncomeByIdDTO GetByIDSelected(int id);
        public void Remove(int id);

        public void SoftDelete(int id);



        public void Update(Income Item);

        public void Save();

    }
}
