using System;
using System.IO;

namespace TollFeeCalculator
{
    public class Loger
    {
        private string LogPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        /// <summary>
        /// initizilse log class
        /// </summary>
        public Loger()
        {
            Directory.CreateDirectory(LogPath);
        }

        /// <summary>
        /// Log Passing Vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle passed toll</param>
        /// <param name="dateTime">Time of pasage</param>
        public void LogPassing(Vehicle vehicle, DateTime dateTime)
        {
            var path = Path.Combine(LogPath, dateTime.ToString("yyyy-MM-dd") + ".csv");
            var logString = $"{dateTime.TimeOfDay},{vehicle.VehicleType},{vehicle.LicensePlate}";
            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine(logString);
            }
        }
    }
}