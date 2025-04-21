using Microsoft.AspNetCore.Components;
using AfryToll.Model.Dtos;
using AfryTollApp.Services.Contracts;

namespace AfryTollApp.Pages
{
    public class TollBase : ComponentBase
    {
        [Inject]
        public ITollService TollService { get; set; }

       


    }
}
