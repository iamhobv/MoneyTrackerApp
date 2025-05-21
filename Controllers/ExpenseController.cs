using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using MoneyTrackerApp.Services;

namespace MoneyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseServices expenseServices;
        private readonly UserManager<ApplicationUser> userManager;


        public ExpenseController(IExpenseServices expenseServices, UserManager<ApplicationUser> userManager)
        {
            this.expenseServices = expenseServices;
            this.userManager = userManager;
        }

        [HttpPost("AddExpense")]
        public async Task<ActionResult<GeneralResponse>> AddExpense(NewExpenseDTO ExpenseData)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != ExpenseData.User_Id)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                if (ExpenseData.Category_Id == 0)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Please choose a valid category"
                    };
                }

                try
                {
                    Expense expense = new Expense()
                    {
                        Amount = ExpenseData.Amount,
                        Category_Id = ExpenseData.Category_Id,
                        CreatedAt = DateTime.Now,
                        Date = ExpenseData.Date.Date,
                        Description = ExpenseData.Description ?? "",
                        IsDeleted = false,
                        User_Id = ExpenseData.User_Id,
                    };
                    expenseServices.Add(expense);
                    expenseServices.Save();
                    IEnumerable<GetIExpenseDTO> expenseTotal = await expenseServices.GetAllByUser(currentUser.UserName);
                    double expenses = expenseTotal.ExpenseSum();
                    if (expenses >= currentUser.ExpenseLimit)
                    {
                        Console.WriteLine("watch out you have reached your expenses limit");
                    }
                    return new GeneralResponse()
                    {
                        IsPass = true,
                        Data = "Expense added successfully"
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


        [HttpPost("SetExpenseLimit")]
        public async Task<ActionResult<GeneralResponse>> SetExpenseLimit(SetExpenseLimitDTO limitDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != limitDTO.UserId)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                currentUser.ExpenseLimit = limitDTO.Limit;
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return new GeneralResponse()
                    {
                        IsPass = true,
                        Data = "Expense limit set successfully"
                    };
                }
                else
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Failed to set expense limit"
                    };
                }
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState
            };
        }




        [HttpGet("GetAllExpense/{UserName:regex(^[[A-Za-z0-9]]+$)}")]
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

            IEnumerable<GetIExpenseDTO> expenses = await expenseServices.GetAllByUser(UserName);

            if (expenses.Count() > 0)
            {
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = expenses
                };
            }
            else
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "No Expense"
                };
            }

        }


        [HttpGet("GetExpense/{id:int}")]
        public async Task<ActionResult<GeneralResponse>> GetSingleExpense(int id)
        {
            var Expense = expenseServices.GetByIDSelected(id);
            if (Expense != null)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != Expense.UserId)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                GetIExpenseDTO expenseDTO = new GetIExpenseDTO()
                {
                    Amount = Expense.Amount,
                    Category = Expense.Category,
                    Date = Expense.Date,
                    Description = Expense.Description
                };
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = expenseDTO
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = "Not valid expense "
            };
        }

        [HttpPut("EditExpense")]
        public async Task<ActionResult<GeneralResponse>> EditExpense(EditExpenseDTO expenseFromEdit)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != expenseFromEdit.UserId)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                Expense res = expenseServices.GetByID(expenseFromEdit.Id);
                if (res != null)
                {

                    try
                    {
                        res.Amount = expenseFromEdit.Amount ?? res.Amount;
                        res.Category_Id = expenseFromEdit.Category_Id ?? res.Category_Id;
                        res.Date = expenseFromEdit.Date ?? res.Date;
                        res.Description = expenseFromEdit.Description ?? res.Description;
                        expenseServices.Update(res);
                        expenseServices.Save();
                        return new GeneralResponse()
                        {
                            IsPass = true,
                            Data = "Expense updated successfully"
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
                    ModelState.AddModelError("error", "Expense is not valid");
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
        public async Task<ActionResult<GeneralResponse>> DeleteExpense(int id)
        {
            Expense expense = expenseServices.GetByID(id);
            if (expense != null)
            {
                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != expense.User_Id)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }
                expenseServices.Remove(id);
                expenseServices.Save();
                return new GeneralResponse() { IsPass = true, Data = "expense Deleted" };

            }
            return new GeneralResponse() { IsPass = false, Data = "Invalid expense" };
        }
    }
}
