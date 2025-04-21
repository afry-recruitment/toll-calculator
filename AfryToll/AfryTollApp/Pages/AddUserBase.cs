using AfryToll.Model.Dtos;
using AfryTollApp.Services;
using AfryTollApp.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace AfryTollApp.Pages
{
    public class AddUserBase : ComponentBase
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string LabelMassage { get; private set; }
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public IUserService UserService { get; set; }

        public UserAddDto User { get; set; } = new UserAddDto();
        public List<UserDto> Users { get; set; }

        [Inject]
        public ICategoryService CategoryService { get; set; }
        public CategoryDto Category { get; set; } = new CategoryDto();
        public List<CategoryDto> Categories { get; set; }


        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Categories = await CategoryService.GetItems();
        }
        protected async Task Add()
        {
            try
            {

                var UserDto = await UserService.AddItem(User);
                if (UserDto != null)
                {
                    SuccessMessage = "ok ✅";
                    ErrorMessage = null;

                    
                    User = new UserAddDto();
                }
                else
                {
                    ErrorMessage = " fel❌";
                    SuccessMessage = null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"fel: {ex.Message}";
                SuccessMessage = null;
            }
        }

        protected void SelectCategory(int categoryId)
        {
            User.CategoryId = categoryId;
            LabelMassage = $"Category {categoryId} selected."; 
        }

    }
}
