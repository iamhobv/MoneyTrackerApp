using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MoneyTrackerApp.Data;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Models;
using static MoneyTrackerApp.Enums.Enums;

namespace MoneyTrackerApp.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IGeneralRepository<Category> categoryRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoryServices(IGeneralRepository<Category> CategoryRepository, UserManager<ApplicationUser> userManager)
        {
            categoryRepository = CategoryRepository;
            this.userManager = userManager;
        }

        public void Add(Category Item)
        {
            categoryRepository.Add(Item);
        }

        public IEnumerable<Category> GetFilter(Expression<Func<Category, bool>> expression)
        {
            return categoryRepository.GetFilter(expression);
        }

        public IEnumerable<Category> GetAll()
        {
            return categoryRepository.GetAll();
        }

        public Category GetByID(int id)
        {
            return categoryRepository.GetFilter(x => x.ID == id).FirstOrDefault();
        }

        public void Remove(int id)
        {

            categoryRepository.Remove(id);

        }

        public void Update(Category Item)
        {
            categoryRepository.Update(Item);
        }

        public void Save()
        {
            categoryRepository.Save();
        }



        public bool CheckCategoryAvailability(string UserName, string Name)
        {

            if (string.IsNullOrEmpty(UserName))
            {
                var CategoryWithoutUser = categoryRepository.GetFilter(c => c.Name.Equals(Name) && (c.User_Id == null || c.User_Id == "")).ToList();
                if (CategoryWithoutUser.Count == 0)
                {
                    return true;
                }
            }
            else
            {
                var CategoryWithUser = categoryRepository.GetFilter(c => (c.User_Id.Equals(UserName) && c.Name.Equals(Name)) || (c.Name.Equals(Name) && (c.User_Id == null || c.User_Id == ""))).ToList();

                if (CategoryWithUser.Count == 0)
                {
                    return true;

                }
            }
            return false;


        }

        public async Task<List<Category>> GetAllCategoryForUserAsync(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                ApplicationUser? user = await userManager.FindByNameAsync(username);
                if (user != null)
                {
                    List<Category> Categories = categoryRepository.GetFilter(c => c.User_Id.Equals(user.Id) || (c.User_Id == null || c.User_Id == "")).ToList();
                    return Categories;
                }


            }
            return categoryRepository.GetFilter(c => (c.User_Id == null || c.User_Id == "")).ToList(); ;
        }

        public CategoryOwner CheckCategoryOwner(int Id)
        {

            Category category = categoryRepository.GetFilter(c => c.ID == Id).FirstOrDefault();
            if (category == null)
            {
                return CategoryOwner.NotMatched;
            }
            if (category.User_Id == null || category.User_Id == "")
            {
                return CategoryOwner.App;
            }
            else
            {
                return CategoryOwner.User;
            }



        }


    }
}
