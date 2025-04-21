using AfryToll.Model.Dtos;
using AfryTollApp.Services.Contracts;
using Microsoft.AspNetCore.Components;
using AfryTollApp.Services.Contracts;

namespace AfryTollApp.Pages
{
    public class ManageCategoryBase : ComponentBase
    {

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
                var categoryDto = await CategoryService.AddItem(Category);
                NavigationManager.NavigateTo($"/ManageCategory", true);

            }
            catch (Exception)
            {


            }
        }


    }
}
