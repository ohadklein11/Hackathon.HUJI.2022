using System;

namespace Team.Ethan
{
    public class Communicator
    {
        private string[] names = new string[] {"Text", "Voice Call", "Video Call", "Meeting"};
        public enum ComTypes
        {
            Text,
            VoiceCall,
            VideoCall,
            Meeting
        }
        
        private DateTime _lastComDate;
        private ComTypes _comType;
        private int _interval; // in days. -1 means never.
        
        public Communicator(ComTypes comType, int interval=0)
        {
            _lastComDate = DateTime.Now;
            _comType = comType;
            _interval = interval;
        }

        /**
         * returns days until next time to communicate.
         * if days is less than 0, returns 0.
         * if interval set to 0 (never), returns -1.
         */
        public int DaysToNextInterval()
        {
            if (_interval == 0)
                return -1;
            
            TimeSpan interval = new TimeSpan(_interval*24);
            DateTime next = _lastComDate.Add(interval);

            int daysToGo = next.Subtract(DateTime.Now).Days;
       
            return daysToGo > 0 ? daysToGo : 0;
        }

        public void UpdateComDate()
        {
            _lastComDate = DateTime.Now;
        }

        public DateTime LastComDate
        {
            get => _lastComDate;
        }

        public ComTypes ComType
        {
            get => _comType;
        }

        public int Interval
        {
            get => _interval;
            set => _interval = value;
        }

        public string GetStringName()
        {
            return names[(int) _comType];
        }
    }
}