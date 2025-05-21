using Microsoft.AspNetCore.Identity;
using MoneyTrackerApp.Data;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyTrackerApp.Services
{
    public class IncomeSrevices : IIncomeServices
    {
        private readonly IGeneralRepository<Income> incomeRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public IncomeSrevices(IGeneralRepository<Income> IncomeRepository, UserManager<ApplicationUser> userManager)
        {
            incomeRepository = IncomeRepository;
            this.userManager = userManager;
        }

        public void Add(Income Item)
        {
            incomeRepository.Add(Item);
        }

        public IEnumerable<Income> GetFilter(Expression<Func<Income, bool>> expression)
        {
            return incomeRepository.GetFilter(expression);
        }

        public IEnumerable<Income> GetAll()
        {
            return incomeRepository.GetAll();
        }
        public async Task<IEnumerable<GetIncomeDTO>> GetAllByUser(string UserName)
        {
            ApplicationUser? user = await userManager.FindByNameAsync(UserName);
            return incomeRepository.GetFilter(i => i.User_Id.Equals(user.Id)).Select(i => new GetIncomeDTO()
            {
                Amount = i.Amount,
                Category = i.Category.Name,
                Date = i.Date,
                Description = i.Description

            });
        }
        public async Task<IEnumerable<GetIncomeDTO>> GetAllByCategoryUser(string UserName, int CategoryId)
        {
            ApplicationUser? user = await userManager.FindByNameAsync(UserName);
            return incomeRepository.GetFilter(i => i.User_Id.Equals(user.Id) && i.Category_Id == CategoryId).Select(i => new GetIncomeDTO()
            {
                Amount = i.Amount,
                Category = i.Category.Name,
                Date = i.Date,
                Description = i.Description

            });
        }
        public IncomeByIdDTO GetByIDSelected(int id)
        {

            IncomeByIdDTO? income = incomeRepository.GetFilter(i => i.ID == id).Select(i => new IncomeByIdDTO()
            {
                Amount = i.Amount,
                Category = i.Category.Name,
                Date = i.Date,
                Description = i.Description,
                UserId = i.User_Id

            }).FirstOrDefault();

            return income;
        }

        public Income GetByID(int id)
        {
            return incomeRepository.GetFilter(x => x.ID == id).FirstOrDefault();
        }

        public void Remove(int id)
        {

            incomeRepository.Remove(id);

        }
        public void SoftDelete(int id)
        {
            Income item = GetByID(id);
            item.IsDeleted = true;
            incomeRepository.Update(item);

        }


        public void Update(Income Item)
        {
            incomeRepository.Update(Item);
        }

        public void Save()
        {
            incomeRepository.Save();
        }

    }
}
