using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Models;
using System.Linq.Expressions;
using static MoneyTrackerApp.Enums.Enums;

namespace MoneyTrackerApp.Interfaces
{
    public interface ICategoryServices
    {
        public void Add(Category Item);

        public IEnumerable<Category> GetFilter(Expression<Func<Category, bool>> expression);
        public IEnumerable<Category> GetAll();

        public Category GetByID(int id);

        public void Remove(int id);

        public void Update(Category Item);

        public void Save();

        public bool CheckCategoryAvailability(string UserName, string Name);
        public Task<List<Category>> GetAllCategoryForUserAsync(string username);
        public CategoryOwner CheckCategoryOwner(int Id);

    }
}
