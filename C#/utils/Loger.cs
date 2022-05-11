using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TollFeeCalculator
{
    class Loger
    {
        public string LogPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        public Loger()
        {
            Directory.CreateDirectory(LogPath);
        }
        public void LogPassing(Vehicle vehicle, DateTime dateTime)
        {
            var file = Path.Combine(LogPath, dateTime.ToString("yyyy-MM-dd"));
            if (!File.Exists(file))
            {

            }
        }
    }
}
