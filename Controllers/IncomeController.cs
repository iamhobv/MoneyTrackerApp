using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoneyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeServices incomeServices;
        private readonly UserManager<ApplicationUser> userManager;

        public IncomeController(IIncomeServices incomeServices, UserManager<ApplicationUser> userManager)
        {
            this.incomeServices = incomeServices;
            this.userManager = userManager;
        }

        [HttpPost("AddIncome")]
        public async Task<ActionResult<GeneralResponse>> AddIncome(NewIncomeDTO IncomeData)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != IncomeData.User_Id)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }



                if (IncomeData.Category_Id == 0)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Please choose a valid category"
                    };
                }
                try
                {
                    Income income = new Income()
                    {
                        Amount = IncomeData.Amount,
                        Category_Id = IncomeData.Category_Id,
                        CreatedAt = DateTime.Now,
                        Date = IncomeData.Date.Date,
                        Description = IncomeData.Description ?? "",
                        IsDeleted = false,
                        User_Id = IncomeData.User_Id,
                    };
                    incomeServices.Add(income);
                    incomeServices.Save();
                    return new GeneralResponse()
                    {
                        IsPass = true,
                        Data = "Income added successfully"
                    };
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.InnerException.Message);
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "There are some errors while saving ... "
                    };
                }



            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState
            };


        }




        [HttpGet("GetAllIncome/{UserName:regex(^[[A-Za-z0-9]]+$)}")]
        public async Task<ActionResult<GeneralResponse>> GetAllByUser(string UserName)
        {
            ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.UserName != UserName)
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Unauthorized access or user mismatch"
                };
            }

            ApplicationUser? user = await userManager.FindByNameAsync(UserName);

            if (user != null)
            {

                IEnumerable<GetIncomeDTO> incomes = await incomeServices.GetAllByUser(UserName);
                if (incomes.Count() > 0)
                {
                    return new GeneralResponse()
                    {
                        IsPass = true,
                        Data = incomes
                    };
                }
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "No Income"
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = "Invalid User"
            };
        }


        [HttpGet("GetIncome/{id:int}")]
        public async Task<ActionResult<GeneralResponse>> GetSingleIncome(int id)
        {
            var income = incomeServices.GetByIDSelected(id);



            if (income != null)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != income.UserId)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }



                GetIncomeDTO incomeDTO = new GetIncomeDTO()
                {
                    Amount = income.Amount,
                    Category = income.Category,
                    Date = income.Date,
                    Description = income.Description
                };
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = incomeDTO
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = "Not valid income "
            };
        }

        [HttpPut("EditIncome")]
        public async Task<ActionResult<GeneralResponse>> EditIncome(EditIncomeDTO incomeFromEdit)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != incomeFromEdit.UserId)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }


                Income res = incomeServices.GetByID(incomeFromEdit.Id);
                if (res != null)
                {

                    try
                    {
                        res.Amount = incomeFromEdit.Amount ?? res.Amount;
                        res.Category_Id = incomeFromEdit.Category_Id ?? res.Category_Id;
                        res.Date = incomeFromEdit.Date ?? res.Date;
                        res.Description = incomeFromEdit.Description ?? res.Description;
                        incomeServices.Update(res);
                        incomeServices.Save();
                        return new GeneralResponse()
                        {
                            IsPass = true,
                            Data = "Income updated successfully"
                        };
                    }
                    catch (Exception e)
                    {

                        ModelState.AddModelError("error", e.InnerException.Message);
                        return new GeneralResponse()
                        {
                            IsPass = false,
                            Data = ModelState
                        };
                    }

                }
                else
                {

                    ModelState.AddModelError("error", "Income is not valid");
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = ModelState
                    };
                }
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState
            };
        }


        [HttpDelete("del/{id:int}")]
        public async Task<ActionResult<GeneralResponse>> DeleteIncome(int id)
        {
            Income income = incomeServices.GetByID(id);
            if (income != null)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != income.User_Id)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }
                incomeServices.Remove(id);
                incomeServices.Save();
                return new GeneralResponse() { IsPass = true, Data = "Income Deleted" };

            }
            return new GeneralResponse() { IsPass = false, Data = "Invalid income" };
        }
    }
}
