using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using MoneyTrackerApp.Services;
using NCrontab;
using static MoneyTrackerApp.Enums.Enums;

namespace MoneyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequentBillController : ControllerBase
    {
        private readonly IRecurringTransactionServices recurringTransactionServices;
        private readonly UserManager<ApplicationUser> userManager;

        public FrequentBillController(IRecurringTransactionServices recurringTransactionServices, UserManager<ApplicationUser> userManager)
        {
            this.recurringTransactionServices = recurringTransactionServices;
            this.userManager = userManager;
        }

        [HttpPost("addBill")]
        public async Task<ActionResult<GeneralResponse>> AddBill(AddRecurringTransactionDTO recurringTransactionDTO)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != recurringTransactionDTO.User_Id)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                var existsBill = recurringTransactionServices.GetAllByNameAndUser(recurringTransactionDTO.User_Id, recurringTransactionDTO.Name);
                if (existsBill == null)
                {
                    return AddNewBill(recurringTransactionDTO);
                }
                else
                {
                    while (existsBill != null)
                    {
                        recurringTransactionDTO.Name = recurringTransactionDTO.Name + " New ";

                        existsBill = recurringTransactionServices.GetAllByNameAndUser(recurringTransactionDTO.User_Id, recurringTransactionDTO.Name);
                    }

                    return AddNewBill(recurringTransactionDTO);
                }


            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState
            };



        }

        [HttpGet("Bills")]
        public async Task<ActionResult<GeneralResponse>> GetBills(getAllBillDto getAllBill)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != getAllBill.User_Id)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                var bills = recurringTransactionServices.GetAllByUser(getAllBill.User_Id);
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = bills.Result
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState
            };
        }


        [HttpPut("PauseBill")]
        public async Task<ActionResult<GeneralResponse>> PauseBill(PauseBillDto PauseBillDto)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != PauseBillDto.UserID)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }
                RecurringJob.RemoveIfExists(PauseBillDto.BillName);
                var existsBill = recurringTransactionServices.GetAllByNameAndUserToUpdate(PauseBillDto.UserID, PauseBillDto.BillName);
                existsBill.Status = RecurringTransactionStatus.Pause;
                //RecurringTransaction recurringTransaction = new RecurringTransaction() { }
                recurringTransactionServices.Update(existsBill);
                recurringTransactionServices.Save();

                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = "Bill updated successfully"
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState
            };
        }

        [HttpPut("ResumeBill")]
        public async Task<ActionResult<GeneralResponse>> ResumeBill(PauseBillDto PauseBillDto)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser? currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != PauseBillDto.UserID)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                var existsBill = recurringTransactionServices.GetAllByNameAndUserToUpdate(PauseBillDto.UserID, PauseBillDto.BillName);
                if (existsBill != null)
                {

                    string CornExpression = GetCronExpression(existsBill.Frequency, existsBill.NextOccuranceDate.Hour, existsBill.NextOccuranceDate.DayOfWeek, existsBill.NextOccuranceDate.Day, existsBill.NextOccuranceDate.Month);

                    RecurringJob.AddOrUpdate($"{existsBill.Name}",
                        () => recurringTransactionHandler(existsBill), CornExpression);
                    existsBill.Status = RecurringTransactionStatus.Resume;
                    recurringTransactionServices.Update(existsBill);
                    recurringTransactionServices.Save();
                    return new GeneralResponse()
                    {
                        IsPass = true,
                        Data = "Bill updated successfully"
                    };
                }
                else
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Bill is not found"
                    };

                }

            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState
            };
        }




        [NonAction]
        public GeneralResponse AddNewBill(AddRecurringTransactionDTO recurringTransactionDTO)
        {
            try
            {
                string CornExpression = GetCronExpression(recurringTransactionDTO.Frequency, recurringTransactionDTO.hour, recurringTransactionDTO.dayOfWeek, recurringTransactionDTO.day, recurringTransactionDTO.month);


                RecurringTransaction newBill = new RecurringTransaction()
                {
                    Amount = recurringTransactionDTO.Amount,
                    Category_Id = recurringTransactionDTO.Category_Id,
                    CreatedAt = DateTime.Now,
                    EndDate = recurringTransactionDTO.EndDate ?? DateTime.MaxValue,
                    Frequency = recurringTransactionDTO.Frequency,
                    Name = recurringTransactionDTO.Name,
                    StartDate = recurringTransactionDTO.StartDate,
                    User_Id = recurringTransactionDTO.User_Id,
                    Status = RecurringTransactionStatus.Resume,
                    IsDeleted = false,
                    NextOccuranceDate = CrontabSchedule.Parse(CornExpression).GetNextOccurrence(recurringTransactionDTO.StartDate)
                };
                recurringTransactionServices.Add(newBill);
                recurringTransactionServices.Save();

                RecurringJob.AddOrUpdate($"{recurringTransactionDTO.Name}",
                    () => recurringTransactionHandler(newBill), CornExpression);
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = "Your bill has been successfully added"
                };
            }
            catch (Exception)
            {

                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Something went wronge"
                };
            }

        }






        [NonAction]
        public void recurringTransactionHandler(RecurringTransaction bill)
        {
            if (bill.EndDate.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                bill.Status = RecurringTransactionStatus.Pause;
                recurringTransactionServices.Update(bill);
                recurringTransactionServices.Save();
            }
            Console.WriteLine($"its time to pay {bill.Amount} to  {bill.Name} bill ");
        }

        [NonAction]
        public string GetCronExpression(RecurringTransactionFrequency frequency, int? hour, DayOfWeek? dayOfWeek, int? day, int? month)
        {

            return frequency switch
            {
                RecurringTransactionFrequency.Daily => Cron.Daily((int)hour),
                RecurringTransactionFrequency.Weekly => Cron.Weekly((DayOfWeek)dayOfWeek),
                RecurringTransactionFrequency.Monthly => Cron.Monthly((int)day),
                RecurringTransactionFrequency.Yearly => Cron.Yearly((int)month),
                _ => Cron.Daily()
            };
        }

    }
}
