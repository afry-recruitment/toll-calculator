using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class VehicleTollFeeCalculatorFactory
    {
        public IVehicleTollCalculator CreateVehicleTollFeeCalculatorFactory(Vehicle vehicle)
        {
            if (vehicle.VehicleType == VehicleEnum.Diplomat.ToString())
            {
                return new DiplomatTollCalculator(new Diplomat());
            }
            else if(vehicle.VehicleType == VehicleEnum.Emergency.ToString())
            {
                return new EmergencyTollCalculator(new Emergency());
            }
            else if(vehicle.VehicleType == VehicleEnum.Foreign.ToString())
            {
                return new ForeignTollCalculator(new Foreign());
            }
            else if(vehicle.VehicleType == VehicleEnum.Military.ToString())
            {
                return new MilitaryTollCalculator(new Military());
            }
            else if (vehicle.VehicleType == VehicleEnum.Motorbike.ToString())
            {
                return new MotorbikeTollCalculator(new Motorbike());
            }
            else if (vehicle.VehicleType == VehicleEnum.Tractor.ToString())
            {
                return new TractorTollCalculator(new Tractor());
            }
            else
            {
                return new CarTollCalculator(new Car());
            }
        }

    }
}
