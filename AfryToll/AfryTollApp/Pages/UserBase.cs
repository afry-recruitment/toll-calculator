using AfryToll.Model.Dtos;
using AfryTollApp.Services.Contracts;
using Microsoft.AspNetCore.Components;



namespace AfryTollApp.Pages
{
    public class UserBase : ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        public UserDto User { get; set; }
        public string PlateNumber { get; set; }
        public string Password { get; set; }
        public string LabelerrorText { get; private set; }
        protected async Task AuthenticateUser()
        {

            User = await UserService.GetUserItem(PlateNumber, Password);


            if (User == null)
            {
                LabelerrorText = "This account does not exist in the system. Please make sure that the Username and Password are correct, or if you are not a member in the system, register in the system first. ";
            }

            else
            { 
                var category = await CategoryService.GetItem(User.CategoryId);

                if (!category.Status)
                {
                    NavigationManager.NavigateTo($"/AddToll/{User.CategoryId}/{User.UserId}");
                }
                else
                {
                    LabelerrorText = "This Car is Fee-Free";
                }


               
            }
        }






    }



}

