using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using MoneyTrackerApp.Services;
using System.IO;
using System.Threading.Tasks;

namespace MoneyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportsController : ControllerBase
    {
        private readonly IExpenseServices expenseServices;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IIncomeServices incomeServices;

        public ExportsController(IExpenseServices expenseServices, UserManager<ApplicationUser> userManager, IIncomeServices incomeServices)
        {
            this.expenseServices = expenseServices;
            this.userManager = userManager;
            this.incomeServices = incomeServices;
        }
        [HttpGet("ExportExpenses/{UserName:regex(^[[A-Za-z0-9]]+$)}")]
        public async Task<ActionResult<GeneralResponse>> GetAllExpenseByUser(string UserName)
        {
            //ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            //if (currentUser == null || currentUser.UserName != UserName)
            //{
            //    return new GeneralResponse()
            //    {
            //        IsPass = false,
            //        Data = "Unauthorized access or user mismatch"
            //    };
            //}

            IEnumerable<GetIExpenseDTO> expenses = await expenseServices.GetAllByUser(UserName);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Expenses");

            worksheet.Cell(1, 1).Value = "Amount";
            worksheet.Cell(1, 2).Value = "Date";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "Category";

            var headerRange = worksheet.Range("A1:D1");
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontSize = 12;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;

            foreach (var expense in expenses)
            {
                worksheet.Cell(row, 1).Value = expense.Amount;
                worksheet.Cell(row, 2).Value = expense.Date.ToShortDateString();
                worksheet.Cell(row, 3).Value = expense.Description;
                worksheet.Cell(row, 4).Value = expense.Category;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Expenses.xlsx");

        }

        [HttpGet("ExportIncome/{UserName:regex(^[[A-Za-z0-9]]+$)}")]
        public async Task<ActionResult<GeneralResponse>> GetAllIncomeByUser(string UserName)
        {
            //ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            //if (currentUser == null || currentUser.UserName != UserName)
            //{
            //    return new GeneralResponse()
            //    {
            //        IsPass = false,
            //        Data = "Unauthorized access or user mismatch"
            //    };
            //}

            IEnumerable<GetIncomeDTO> Incomes = await incomeServices.GetAllByUser(UserName);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Incomes");

            worksheet.Cell(1, 1).Value = "Amount";
            worksheet.Cell(1, 2).Value = "Date";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "Category";

            var headerRange = worksheet.Range("A1:D1");
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontSize = 12;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;

            foreach (var income in Incomes)
            {
                worksheet.Cell(row, 1).Value = income.Amount;
                worksheet.Cell(row, 2).Value = income.Date.ToShortDateString();
                worksheet.Cell(row, 3).Value = income.Description;
                worksheet.Cell(row, 4).Value = income.Category;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Expenses.xlsx");

        }






    }
}
