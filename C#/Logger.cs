using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Logger : LogBase
    {
        private string CurrentDirectory
        {
            get;
            set;
        }

        private string FileName
        {
            get;
            set;
        }

        private string FilePath
        {
            get;
            set;
        }

        public Logger()
        {
            this.CurrentDirectory = Directory.GetCurrentDirectory();
            this.FileName = "Log.txt";
            this.FilePath = this.CurrentDirectory + "/" + this.FileName;
        }

        public override void Log(string Message)
        {
            using (StreamWriter w = File.AppendText(this.FilePath))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine("{0}" , Message);
                w.WriteLine("---------------------------------");

            }
        }
    }
}
