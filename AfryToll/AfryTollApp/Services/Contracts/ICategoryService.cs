using AfryToll.Model.Dtos;


namespace AfryTollApp.Services.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetItems();
        Task<CategoryDto> AddItem(CategoryDto CategoryDto);
        Task<List<CategoryDto>> GetstatusItems(bool Status);
        Task<CategoryDto> GetItem(int id);
    }

}
