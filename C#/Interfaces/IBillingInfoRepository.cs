using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Interfaces
{
    public interface IBillingInfoRepository
    {
        ICollection<BillingInfo> GetBillingInfos();
        BillingInfo GetBillingInfo(int id);
        BillingInfo GetBillingInfo(string registrationNumber);
        bool BillingInfoExist(int billingInfoId);
        bool CreateBillingInfo(BillingInfo billingInfo);
        bool UpdateBillingInfo(BillingInfo billingInfo);
        bool DeleteBillingInfo(BillingInfo billingInfo);
        bool Save();
    }
}