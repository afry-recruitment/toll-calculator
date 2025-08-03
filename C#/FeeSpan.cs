using System;

namespace Test_C__app.Afry_toll_calculator_fork.C_
{
    public class FeeSpan
    {
        public TimeSpan StartTime { get => _startTime; }
        public int Fee { get => _fee; }
        public FeeSpan(TimeSpan startTime, int fee)
        {
            _fee = fee;
            _startTime = startTime;
        }
        private int _fee;
        private TimeSpan _startTime;
    }
}
