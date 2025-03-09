using Microsoft.EntityFrameworkCore.Diagnostics;
using TollFeeCalculator.Data;
using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Repository
{
    public class BillingInfoRepository : IBillingInfoRepository
    {
        private readonly DataContext _context;

        public BillingInfoRepository(DataContext context)
        {
            this._context = context;
        }

        public bool CreateBillingInfo(BillingInfo billingInfo)
        {
            _context.Add(BillingInfo);

            return Save();
        }

        public BillingInfo GetBillingInfo(int id)
        {
            return _context.BillingInfo.Where(b => b.Id == id).FirstOrDefault();
        }

        public BillingInfo GetBillingInfoByVehicle(string registrationNumber)
        {
            const Vehicle = _context.Crossings.Where(c => c.Vehicle.RegistrationNumber == registrationNumber);
            return _context.BillingInfo.Where(c => c.VehicleId == Vehicle.Id).FirstOrDefault();
        }

        public ICollection<BillingInfo> GetBillingInfos()
        {
            return _context.BillingInfo.OrderBy(c => c.Id).ToList();
        }

        public bool BillingInfoExists(int BillingInfoId)
        {
            return _context.BillingInfo.Any(c => c.Id == BillingInfoId);
        }

        bool UpdateBillingInfo(BillingInfo BillingInfo)
        {
            _context.Update(BillingInfo);
            return Save();
        };

        bool DeleteBillingInfo(BillingInfo BillingInfo)
        {
            _context.Remove(BillingInfo);
            return Save();
        };

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
