using TollFeeCalculator.Models;

namespace TollFeeCalculator.Interfaces
{
    public interface iCrossingRepository
    {
        ICollection<Crossing> GetCrossings();
        ICollection<Crossing> GetCrossingsMonth(string registrationNumber, DateTime date);
        Crossing GetCrossing(int id);
        Crossing GetCrossing(string registrationNumber);
        bool CrossingExist(int crossingId);
        bool CreateCrossing(Crossing crossing);
        bool UpdateCrossing(Crossing crossing);
        bool DeleteCrossing(Crossing crossing);
        bool Save();
    }
}
