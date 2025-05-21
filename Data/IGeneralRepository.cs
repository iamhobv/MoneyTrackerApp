using System.Linq.Expressions;
using MoneyTrackerApp.Models;

namespace MoneyTrackerApp.Data
{
    public interface IGeneralRepository<T> where T : BaseModel
    {
        void Add(T category);
        void Update(T category);
        void Remove(int id);
        void Save();
        T GetByID(int id);
        IQueryable<T> GetAll();
        IQueryable<T> GetFilter(Expression<Func<T, bool>> expression);
    }
}
