using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MoneyTrackerApp.Models;

namespace MoneyTrackerApp.Data
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : BaseModel
    {
        private readonly DatabaseContext context;

        public GeneralRepository(DatabaseContext context)
        {
            this.context = context;
        }


        public void Add(T Item)
        {
            context.Set<T>().Add(Item);
        }

        public IQueryable<T> GetFilter(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public T GetByID(int id)
        {
            return GetFilter(x => x.ID == id).FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entity = GetByID(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
            }
        }

        public void Update(T Item)
        {
            context.Set<T>().Update(Item);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
