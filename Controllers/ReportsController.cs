using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using MoneyTrackerApp.Services;

namespace MoneyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IExpenseServices expenseServices;
        private readonly ICategoryServices categoryServices;
        private readonly IIncomeServices incomeServices;

        public ReportsController(UserManager<ApplicationUser> userManager, IExpenseServices expenseServices, ICategoryServices categoryServices, IIncomeServices incomeServices)
        {
            this.userManager = userManager;
            this.expenseServices = expenseServices;
            this.categoryServices = categoryServices;
            this.incomeServices = incomeServices;
        }
        [HttpGet("summary")]
        public async Task<ActionResult<GeneralResponse>> IncomeExpenseReportsAsync(IEReportsDTO ReportsDTO)
        {
            ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.UserName != ReportsDTO.UserName)
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Unauthorized access or user mismatch"
                };
            }
            IEnumerable<GetIncomeDTO> income = await incomeServices.GetAllByUser(ReportsDTO.UserName);
            double incomes = income.IncomeSum();
            IEnumerable<GetIExpenseDTO> expense = await expenseServices.GetAllByUser(ReportsDTO.UserName);
            double expenses = expense.ExpenseSum();
            double savings = incomes - expenses;

            return new GeneralResponse()
            {
                IsPass = true,
                Data = new
                {
                    income = incomes,
                    expense = expenses,
                    saving = savings
                }
            };

        }

        [HttpGet("Category")]

        public async Task<ActionResult<GeneralResponse>> GetReportsByCategoryNameAsync(CategoryReportsDTO CatReportDTO)
        {
            ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.UserName != CatReportDTO.UserName)
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Unauthorized access or user mismatch"
                };
            }

            Category category = categoryServices.GetByID(CatReportDTO.CategoryId);
            if (category == null)
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Category is not exist!"
                };
            }

            IEnumerable<GetIncomeDTO> income = await incomeServices.GetAllByCategoryUser(CatReportDTO.UserName, CatReportDTO.CategoryId);
            double incomes = income.IncomeSum();
            IEnumerable<GetIExpenseDTO> expense = await expenseServices.GetAllByCategoryUser(CatReportDTO.UserName, CatReportDTO.CategoryId);
            double expenses = expense.ExpenseSum();
            return new GeneralResponse()
            {
                IsPass = true,
                Data = new
                {
                    category = category.Name,
                    income = incomes,
                    expense = expenses
                }
            };
        }


    }
}
