using Microsoft.AspNetCore.Components;
using System.Net.Http;
using AfryToll.Model.Dtos;
using AfryTollApp.Services;
using AfryTollApp.Services.Contracts;

namespace AfryTollApp.Pages
{
    public class ManageTollCostBase : ComponentBase
    {
        public string LabelMassage { get; private set; }
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public ITollCostService TollCostService { get; set; }

        public TollCostAddDto TollCost { get; set; } = new TollCostAddDto();
        public List<TollCostDto> TollCosts { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private string SelectedDayName = "";

        protected override async Task OnInitializedAsync()
        {
            TollCosts = await TollCostService.GetCategorItem(CategoryId);
        }
        
        protected async Task Add() => await AddTollCost(isRushHour: false);

        protected async Task AddRushHour() => await AddTollCost(isRushHour: true, rushHourCost: 18);

        private async Task AddTollCost(bool isRushHour, int rushHourCost = 18)
        {
            try
            {

                var timePattern = @"^([01]\d|2[0-3]):[0-5]\d$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(TollCost.Time, timePattern))
                {
                    LabelMassage = "Time must be in HH:mm format (e.g., 08:00)";
                    return;
                }


                bool isDuplicate = TollCosts.Any(c =>
                    c.Time == TollCost.Time &&
                    c.CategoryId == CategoryId &&
                    c.Traffic == (isRushHour ? 1 : 0)
                );

                if (isDuplicate)
                {
                    LabelMassage = "This time already exists!";
                    return;
                }

                TollCost.CategoryId = CategoryId;
                TollCost.Traffic = isRushHour ? 1 : 0;
                if (isRushHour)
                    TollCost.Cost = rushHourCost;

                var result = await TollCostService.AddItem(TollCost);

                NavigationManager.NavigateTo($"/ManageTollCost/{CategoryId}", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
      
        private TollCostDto GetTranspotationCost(int id)
        {
            return TollCosts.FirstOrDefault(i => i.Id == id);
        }

        private async Task RemoveTollCost(int id)
        {
            var ItemDto = GetTranspotationCost(id);

            TollCosts.Remove(ItemDto);



        }
        protected async Task Delete_Click(int id)
        {
            var ProductTollDto = await TollCostService.DeleteItem(id);

            await RemoveTollCost(id);
            NavigationManager.NavigateTo($"/ManageTollCost/{CategoryId}", true);

        }


    }
}
