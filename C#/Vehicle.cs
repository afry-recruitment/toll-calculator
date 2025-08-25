using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public interface IVehicle
    {
        String GetVehicleType();
        bool IsTollFree();
    }

    public abstract class RoadVehicle : IVehicle
    {
        public abstract string GetVehicleType();
        public virtual bool IsTollFree() => false;
    }

    public class Car : RoadVehicle
    {
        public override String GetVehicleType() => "Car";
        public override bool IsTollFree() => false;
    }

    public class EmergencyVehicle : RoadVehicle
    {
        public override String GetVehicleType() => "EmergencyVehicle";
        public override bool IsTollFree() => true;
    }
    
    public class Tractor : RoadVehicle
    {
        public override String GetVehicleType() => "Tractor";
        public override bool IsTollFree() => true;
    }

    public class DiplomatVehicle : RoadVehicle
    {
        public override String GetVehicleType() => "DiplomatVehicle";
        public override bool IsTollFree() => true;
    }
    
    public class ForeignVehicle : RoadVehicle
    {
        public override String GetVehicleType() => "ForeignVehicle";
        public override bool IsTollFree() => false;
    }

    public class MilitaryVehicle : RoadVehicle
    {
        public override String GetVehicleType() => "MilitaryVehicle";
        public override bool IsTollFree() => true;
    }
    
}