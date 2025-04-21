using Microsoft.AspNetCore.Components;
using System.Reflection.Emit;
using AfryToll.Model.Dtos;
using AfryTollApp.Common;
using AfryTollApp.Services.Contracts;
using Microsoft.AspNetCore.Http;
using AfryTollApp.Services;
using System;
using System.Data;

namespace AfryTollApp.Pages
{
    public class AddTollBase : ComponentBase
    {
        private int TpcID, Cost, usercost, temp, costtemp, UserEnterDay;

        private string Date, UserEnterDate;
        private bool UserEnterHoliday;


        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public int CategoryId { get; set; }

        [Inject]
        public ITollService TollService { get; set; }
        public List<TollDto> Tolls { get; set; }
        public List<TollDto> LoadTolls { get; set; }
        public string PlateNumber { get; set; }
        public TollCostDto TollCost { get; set; }
        public TollDto Tolltime { get; set; }
        public TollToAddDto TolltoAdd { get; set; } = new TollToAddDto();
        
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public string ErrorMessage { get; set; }


        protected bool btnEnterEnabled = true;


        public string LabelcurrentdateText { get; private set; }
        public string LabeldayofweekText { get; private set; }
        public string LabeltableText { get; private set; }
        public string LabelHolidayText { get; private set; }
        public string LabelCostText { get; private set; }



        protected override async Task OnInitializedAsync()
        {
          

            DateTime currentDate = DateTime.Now;
            DayOfWeek dayOfWeek = currentDate.DayOfWeek;
            bool isPublicHoliday = TollTools.IsPublicHoliday(currentDate);
            LabelcurrentdateText = currentDate.ToString("MM/dd/yyyy");
            LabeldayofweekText = dayOfWeek.ToString();
            if (isPublicHoliday == true || dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            { LabelHolidayText = "Today is Holiday and cost=0"; }
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                LabelHolidayText = "Today is Weekends and cost=0";

            }
            Date = currentDate.ToString("MMddyyyy");
            LoadTolls = await TollService.GetUserItem(UserId, Date);
            if (Tolls == null)
            {
                LabeltableText = "You have not toll today";
            }
        }

        protected async Task EnterUser()
        {

            int costTemp = 0;
            int cost = 0;
            int temp = 1;
            DateTime currentDate = DateTime.Now;
            string date = currentDate.ToString("MMddyyyy");
            string time = currentDate.ToString("HH:mm");
            DayOfWeek dayOfWeek = currentDate.DayOfWeek;

            UserEnterDate = date;
            UserEnterHoliday = TollTools.IsPublicHoliday(currentDate);
            bool isFreeDay = UserEnterHoliday || dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
            int totalCostToday = await TollService.GetTotalCostForDay(UserId, date);
            if (totalCostToday >= 60)
            {
                LabelCostText = "You have paid the maximum toll fee today. From this moment on, all your tolls for today will be zero.";
                return;
            }

            TollCost = await TollService.GetTollCost(CategoryId, time);

            Tolltime = await TollService.GetTolltime(UserId, time);


            TpcID = TollCost?.Id ?? 0;

            if (TollCost != null)
            {
                costTemp = isFreeDay ? 0 : (int)TollCost.Cost;
                int allowedCost = 60 - totalCostToday;
                if (costTemp > allowedCost)
                {
                    costTemp = allowedCost;
                }
            }
            if (Tolltime == null)
            {
                if (TollCost != null)
                {
                    cost = costTemp;

                }
                LabelCostText = cost.ToString();
                await AddTollItem(UserId, cost, date, time, TpcID, temp, costTemp);
            }
            else
            {
                costTemp = isFreeDay ? 0 : (int)TollCost.Cost;
                int userCost = (int)Tolltime.UserCost;
                temp = 0;

                if (userCost < costTemp)
                {
                    cost = costTemp - userCost;
                    int allowedCost = 60 - totalCostToday;
                    if (cost > allowedCost)
                    {
                        cost = allowedCost;
                    }
                    int newUserCost = userCost + cost;
                    LabelCostText = cost.ToString();

                    await TollService.UpdateUserCost((int)Tolltime.Id, newUserCost);
                }

                await AddTollItem(UserId, cost, date, time, TpcID, temp, costTemp);
            }

            async Task AddTollItem(int userId, int cost, string date, string time, int tpcId, int temp, int costTemp)
            {
                var Toll = new TollToAddDto()
                {
                    UserId = userId,
                    UserCost = cost,
                    Date = date,
                    Time = time,
                    TollCostId = tpcId,
                    Temp = temp,
                    TempCost = costTemp
                };

                await TollService.AddItem(Toll);
            }

            LoadTolls = await TollService.GetUserItem(UserId, Date);

        }


        protected async Task NewTrip()
        {
            NavigationManager.NavigateTo($"/Category/{UserId}");
        }
    }
}

