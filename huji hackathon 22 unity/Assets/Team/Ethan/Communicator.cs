using System;

namespace Team.Ethan
{
    public class Communicator
    {
        public enum ComTypes
        {
            VoiceCall,
            VideoCall,
            Text,
            Meeting
        }
        
        private DateTime _lastComDate;
        private ComTypes _comType;
        private int _interval;
        
        public Communicator(ComTypes comType, int interval)
        {
            _lastComDate = DateTime.Now;
            _comType = comType;
            _interval = interval;
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
        
        
    }
}