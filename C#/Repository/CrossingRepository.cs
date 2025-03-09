using Microsoft.EntityFrameworkCore.Diagnostics;
using TollFeeCalculator.Data;
using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Repository
{
    public class CrossingRepository : ICrossingRepository
    {
        private readonly DataContext _context;

        public CrossingRepository(DataContext context)
        {
            this._context = context;
        }

        public bool CreateCrossing(Crossing crossing)
        {
            _context.Add(crossing);

            return Save();
        }

        public Crossing GetCrossing(int id)
        {
            return _context.Crossing.Where(c => c.Id == id).FirstOrDefault();
        }

        public Crossing GetCrossing(string registrationNumber)
        {
            return _context.Crossing.Where(c => c.Vehicle.RegistrationNumber == registrationNumber).FirstOrDefault();
        }

        public ICollection<Crossing> GetCrossings()
        {
            return _context.Crossing.OrderBy(c => c.Id).ToList();
        }

        ICollection<Crossing> GetCrossingsMonth(string registrationNumber, DateTime date)
        {
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            return _context.Crossings
                .Where(c => c.Vehicle.RegistrationNumber == registrationNumber
                            && c.CrossingDate >= oneMonthAgo)
                .OrderByDescending(c => c.CrossingDate) // Sortér nyeste først
                .ToList();
        }

        public bool CrossingExists(int crossingId)
        {
            return _context.Crossing.Any(c => c.Id == crossingId);
        }

        bool UpdateCrossing(Crossing crossing) {
            _context.Update(crossing);
            return Save();
        };

        bool DeleteCrossing(Crossing crossing)
        {
            _context.Remove(crossing);
            return Save();
        };

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
