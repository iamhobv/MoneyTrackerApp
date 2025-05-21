using Microsoft.AspNetCore.Identity;
using MoneyTrackerApp.Data;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using System.Linq.Expressions;

namespace MoneyTrackerApp.Services
{
    public class ExpenseServices : IExpenseServices
    {
        private readonly IGeneralRepository<Expense> expenseRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ExpenseServices(IGeneralRepository<Expense> ExpenseRepository, UserManager<ApplicationUser> userManager)
        {
            expenseRepository = ExpenseRepository;
            this.userManager = userManager;
        }

        public void Add(Expense Item)
        {
            expenseRepository.Add(Item);
        }

        public IEnumerable<Expense> GetFilter(Expression<Func<Expense, bool>> expression)
        {
            return expenseRepository.GetFilter(expression);
        }

        public IEnumerable<Expense> GetAll()
        {
            return expenseRepository.GetAll();
        }

        public Expense GetByID(int id)
        {
            return expenseRepository.GetFilter(x => x.ID == id).FirstOrDefault();
        }

        public void Remove(int id)
        {

            expenseRepository.Remove(id);

        }

        public void Update(Expense Item)
        {
            expenseRepository.Update(Item);
        }

        public void Save()
        {
            expenseRepository.Save();
        }

        public async Task<IEnumerable<GetIExpenseDTO>> GetAllByUser(string UserName)
        {
            ApplicationUser? user = await userManager.FindByNameAsync(UserName);
            return expenseRepository.GetFilter(i => i.User_Id.Equals(user.Id)).Select(i => new GetIExpenseDTO()
            {
                Amount = i.Amount,
                Category = i.Category.Name,
                Date = i.Date,
                Description = i.Description

            });
        }

        public async Task<IEnumerable<GetIExpenseDTO>> GetAllByCategoryUser(string UserName, int CategoryId)
        {
            ApplicationUser? user = await userManager.FindByNameAsync(UserName);
            return expenseRepository.GetFilter(i => i.User_Id.Equals(user.Id) && i.Category_Id == CategoryId).Select(i => new GetIExpenseDTO()
            {
                Amount = i.Amount,
                Category = i.Category.Name,
                Date = i.Date,
                Description = i.Description

            });
        }

        public ExpenseByIdDTO GetByIDSelected(int id)
        {

            ExpenseByIdDTO? income = expenseRepository.GetFilter(i => i.ID == id).Select(i => new ExpenseByIdDTO()
            {
                Amount = i.Amount,
                Category = i.Category.Name,
                Date = i.Date,
                Description = i.Description,
                UserId = i.User_Id

            }).FirstOrDefault();

            return income;
        }

    }
}
