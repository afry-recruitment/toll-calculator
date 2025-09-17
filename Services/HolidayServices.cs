using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tollCalculator.Data;

namespace tollCalculator.Services
{
	public class HolidayServices
	{
		private readonly ApplicationDbContext _dbContext;
		

		public List<DateTime> ReadCsv (string filePath)
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("Filepath does not exist");
				return new List<DateTime>();
			}

			var holidayDates = new List<DateTime>();

			try
			{
				var lines = File.ReadAllLines(filePath);

				foreach (var line in lines)
				{
					if (DateTime.TryParse(line.Trim(), out DateTime date))
					{
						holidayDates.Add(date);
					}
					else
					{
						Console.WriteLine("Cannot read line");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error reading CSV: " + ex.Message);
			}

			return holidayDates;
		}

	}
}
