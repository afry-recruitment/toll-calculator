using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.interfaces;

namespace TollFeeCalculator.Models
{
    //Basic Info about total cost. Invoice. TODO: expand with the act of generating and sending invoices using the ownerID that would be stored in the vehicle(TODO).
    public class BillingInfo
    {   
        int Id { get; set; }
        int VehicleId { get; set; } //foreign key
        int TotalCostMonth { get; set; }
        int TotalNumberOfCrossings { get; set; }
        bool IsPaid { get; set; }
    }
}