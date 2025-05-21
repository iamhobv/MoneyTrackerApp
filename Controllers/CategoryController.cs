using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackerApp.Data;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using static MoneyTrackerApp.Enums.Enums;

namespace MoneyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoryServices categoryServices;

        public CategoryController(UserManager<ApplicationUser> userManager, ICategoryServices categoryServices)
        {
            this.userManager = userManager;
            this.categoryServices = categoryServices;
        }
        [HttpPost("Add")]
        public async Task<ActionResult<GeneralResponse>> AddCategory(AddCategoryDTO categoryFromRequest)
        {
            ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Id != categoryFromRequest.User_Id)
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Unauthorized access or user mismatch"
                };
            }

            bool result = categoryServices.CheckCategoryAvailability(categoryFromRequest.User_Id, categoryFromRequest.Name);
            if (result)
            {

                Category category = new Category()
                {
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    User_Id = categoryFromRequest.User_Id,
                    Name = categoryFromRequest.Name,

                };

                categoryServices.Add(category);
                categoryServices.Save();
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = "Category Added"
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = "Category is already exists"
            };

        }



        [HttpGet("Get")]
        public async Task<ActionResult<GeneralResponse>> GetCategory(GetCategoryDTO getCategoryDTO)
        {
            ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.UserName != getCategoryDTO.UserName)
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Unauthorized access or user mismatch"
                };
            }

            List<Category> categories = await categoryServices.GetAllCategoryForUserAsync(getCategoryDTO.UserName);
            return new GeneralResponse()
            {
                IsPass = true,
                Data = categories
            };
        }


        [HttpPut("Edit")]

        public async Task<ActionResult<GeneralResponse>> EditUserCategoryAsync(EditCategoryDTO editCategoryDTO)
        {
            try
            {

                ApplicationUser? currentUser = await userManager.GetUserAsync(User);

                if (currentUser == null || currentUser.UserName != editCategoryDTO.UserName)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }

                CategoryOwner catOwner = categoryServices.CheckCategoryOwner(editCategoryDTO.CategoryId);
                Category cat = categoryServices.GetByID(editCategoryDTO.CategoryId);

                if (cat == null || catOwner == CategoryOwner.NotMatched || cat.User_Id == null || cat.User_Id == "" || !cat.User_Id.Equals(currentUser.Id))
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Unauthorized access or user mismatch"
                    };
                }




                if (catOwner == CategoryOwner.User)
                {


                    cat.Name = editCategoryDTO.Name;

                    categoryServices.Update(cat);
                    categoryServices.Save();
                    return new GeneralResponse()
                    {
                        IsPass = true,
                        Data = "Category item has been edited!"
                    };
                }
                else
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "This Category item is not editable!"
                    };

                }


            }
            catch (Exception e)
            {

                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = e.InnerException.Message
                };
            }












        }


        [HttpDelete("Delete/{id:int}")]

        public async Task<ActionResult<GeneralResponse>> DeleteUserCategoryAsync(int id)
        {
            ApplicationUser? currentUser = await userManager.GetUserAsync(User);

            CategoryOwner catOwner = categoryServices.CheckCategoryOwner(id);
            Category cat = categoryServices.GetByID(id);


            if (cat == null || catOwner == CategoryOwner.NotMatched || cat.User_Id == null || cat.User_Id == "" || !cat.User_Id.Equals(currentUser.Id))
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Unauthorized access or user mismatch"
                };
            }




            if (catOwner == CategoryOwner.User)
            {
                categoryServices.Remove(id);
                categoryServices.Save();
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = "Category deleted successfully"
                };
            }
            else
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "This Category item is not editable!"
                };
            }

        }
    }
}
