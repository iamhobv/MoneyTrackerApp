using Microsoft.AspNetCore.Identity;
using MoneyTrackerApp.Data;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using System.Linq.Expressions;

namespace MoneyTrackerApp.Services
{
    public class RecurringTransactionServices : IRecurringTransactionServices
    {
        private readonly IGeneralRepository<RecurringTransaction> recurringTransactionRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public RecurringTransactionServices(IGeneralRepository<RecurringTransaction> RecurringTransactionRepository, UserManager<ApplicationUser> userManager)
        {
            recurringTransactionRepository = RecurringTransactionRepository;
            this.userManager = userManager;
        }

        public void Add(RecurringTransaction Item)
        {
            recurringTransactionRepository.Add(Item);
        }

        public IEnumerable<RecurringTransaction> GetFilter(Expression<Func<RecurringTransaction, bool>> expression)
        {
            return recurringTransactionRepository.GetFilter(expression);
        }

        public IEnumerable<RecurringTransaction> GetAll()
        {
            return recurringTransactionRepository.GetAll();
        }

        public RecurringTransaction GetByID(int id)
        {
            return recurringTransactionRepository.GetFilter(x => x.ID == id).FirstOrDefault();
        }

        public void Remove(int id)
        {

            recurringTransactionRepository.Remove(id);

        }

        public void Update(RecurringTransaction Item)
        {
            recurringTransactionRepository.Update(Item);
        }

        public void Save()
        {
            recurringTransactionRepository.Save();
        }

        public async Task<IEnumerable<GetRecuuringTransactionsDTO>> GetAllByUser(string User_Id)
        {
            //ApplicationUser? user = await userManager.FindByNameAsync(UserName);
            return recurringTransactionRepository
                .GetFilter(r => r.User_Id.Equals(User_Id))
                .Select(r => new GetRecuuringTransactionsDTO()
                {
                    Amount = r.Amount,
                    Name = r.Name,
                    StartDate = r.StartDate,
                    Frequency = r.Frequency,
                    NextOccuranceDate = r.NextOccuranceDate,
                    Status = r.Status,
                    Category_Id = r.Category_Id,
                    EndDate = r.EndDate

                });
        }

        public GetRecuuringTransactionsDTO GetAllByNameAndUser(string User_Id, string JobName)
        {
            return recurringTransactionRepository
                .GetFilter(r => r.User_Id.Equals(User_Id) && r.Name.Equals(JobName))
                .Select(r => new GetRecuuringTransactionsDTO()
                {
                    Amount = r.Amount,
                    Name = r.Name,
                    StartDate = r.StartDate,
                    Frequency = r.Frequency,
                    NextOccuranceDate = r.NextOccuranceDate,
                    Status = r.Status,
                    Category_Id = r.Category_Id,
                    EndDate = r.EndDate

                }).FirstOrDefault();
        }


        public GetRecuuringTransactionsDTO GetByIDSelected(int id)
        {
            GetRecuuringTransactionsDTO? RecuuringTransaction = recurringTransactionRepository.GetFilter(r => r.ID == id).Select(r => new GetRecuuringTransactionsDTO()
            {
                Amount = r.Amount,
                Name = r.Name,
                StartDate = r.StartDate,
                Frequency = r.Frequency,
                NextOccuranceDate = r.NextOccuranceDate,
                Status = r.Status,
                Category_Id = r.Category_Id,
                EndDate = r.EndDate

            }).FirstOrDefault();
            return RecuuringTransaction;
        }

        public RecurringTransaction GetAllByNameAndUserToUpdate(string User_Id, string JobName)
        {
            return recurringTransactionRepository
                            .GetFilter(r => r.User_Id.Equals(User_Id) && r.Name.Equals(JobName))
                            .FirstOrDefault();
        }
    }
}
