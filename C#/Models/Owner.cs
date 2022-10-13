using System;
using System.Collections.Generic;
using System.Linq;

namespace TollFeeCalculator.Models
{
    public class Owner : IModel
    {
        public int Id { get; init; }
        public OwnerType OwnerType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public Address? Address { get; set; }
        public int CurrentDebt { get; private set; } = 0;

        public Owner(OwnerType ownerType)
        {
            OwnerType = ownerType;
        }

        public IEnumerable<IGrouping<Vehicle, TollStationPassage>> GetPassagesForAllOwnedCars()
        {
            // Output for testing and verification.
            var allPassages = TollStationPassage.AllTollStationPassages.Where(x => x.Vehicle.Owner == this).GroupBy(v => v.Vehicle);
            foreach (var item in allPassages)
            {
                Console.WriteLine(item.Key.VehicleType);
                foreach (var i in item)
                {
                    Console.WriteLine(i.Time);
                    Console.WriteLine(i.TollFee);
                }
            }

            return TollStationPassage.AllTollStationPassages.Where(x => x.Vehicle.Owner == this).GroupBy(v => v.Vehicle);
        }

        public void UpdateCurrentDebt(int debt)
        {
            CurrentDebt = debt;
        }
    }
}