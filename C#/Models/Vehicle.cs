using System;
using System.Linq;

namespace TollFeeCalculator
{
    public class Vehicle : IVehicle, IModel
    {
        #region Variables
        public int Id { get; init; } // Autoincrement in db.
        public string LicensePlate { get; init; } // Recommend not using id since some foreign cars have the same format.
        public VehicleType VehicleType { get; set; }
        public VehicleBrand? VehicleBrand { get; set; } // Allowing null for testing purposes.
        public VehicleModel? VehicleModel { get; set; } // Allowing null for testing purposes.
        public Owner? Owner { get; set; } // If the vehicle is toll free then owner is irrelevant.
        public bool IsTollFree { get; set; } = false;

        #endregion

        #region Constructors
        public Vehicle(VehicleType vehicleType)
        {
            VehicleType = vehicleType;
            IsTollFree = SetTollFreeStatus();
        }

        public Vehicle(VehicleType vehicleType, Owner owner)
        {
            VehicleType = vehicleType;
            Owner = owner;
            IsTollFree = SetTollFreeStatus();
        }

        public Vehicle(VehicleType vehicleType, VehicleBrand vehicleBrand, VehicleModel vehicleModel, Owner owner = null)
        {
            VehicleType = vehicleType;
            VehicleBrand = vehicleBrand;
            VehicleModel = vehicleModel;
            Owner = owner;
            IsTollFree = SetTollFreeStatus();
        }
        #endregion

        #region Methods

        #region TollStatus
        public bool SetTollFreeStatus()
        {
            return Enum.IsDefined(typeof(TollFreeVehicles), VehicleType.ToString());
        }

        public void ChangeTollFreeStatus(bool isTollFree)
        {
            IsTollFree = isTollFree;
        }
        #endregion

        #region VehicleType
        public string GetVehicleType()
        {
            return this.VehicleType.ToString();
        }

        public void ChangeVehicleType(VehicleType vehicleType)
        {
            VehicleType = vehicleType;
        }
        #endregion

        #region TollStationPassages
        // These might be better suited in TollStationPassage.cs.
        public TollStationPassage[] GetAllTollStationPassages()
        {
            return TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == this).ToArray();
        }

        public TollStationPassage[] GetTollStationPassagesBetweenDates(DateTime startDate, DateTime endDate)
        {
            return TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == this && v.Time.Date >= startDate.Date && v.Time.Date <= endDate.Date).ToArray();
        }

        public TollStationPassage[] GetTollStationPassagesForDate(DateTime date)
        {
            return TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == this && v.Time.Date == date.Date).ToArray();
        }

        public TollStationPassage[] GetTollStationPassagesForMonth(int year, int month)
        {
            return TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == this && v.Time.Year == year && v.Time.Month == month).ToArray();
        }

        #endregion

        #endregion
    }
}