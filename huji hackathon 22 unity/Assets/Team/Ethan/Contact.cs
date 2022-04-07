using System;
using System.Collections.Generic;
using UnityEngine;
using static Team.Ethan.Communicator;
using Random = System.Random;

namespace Team.Ethan
{
    public class Contact
    {
        // constants
        private const int DailyLoss = 3;
        
        //unique identifiers
        private string _name;
        private string _phoneNumber;

        private Dictionary<ComTypes, Communicator> _communicators = 
                                                    new Dictionary<ComTypes, Communicator>();
        private Texture _avatar;
        private int _health;
        private int _circleNum;
        private bool _shouldNotify;
        private bool _isOnline;

        public Contact(string name, string phoneNumber, Texture avatar, int circleNum=0)
        {
            _phoneNumber = phoneNumber;
            _name = name;
            _avatar = avatar;
            _health = 100; // the health of your relationship
            _circleNum = circleNum;
            _shouldNotify = false;
            _isOnline = false;
        }
        
        public void AddCommunicator(ComTypes comType, int interval, DateTime lastComTime, int daysSubtracted=0)
        {
            _communicators.Add(comType, new Communicator(comType, interval, lastComTime, daysSubtracted));
        }

        public void UpdateOnlineStatus()
        {
            //TODO: fetch data from server
            Random rand = new System.Random();
            bool serverResult = Convert.ToBoolean(rand.Next(2));
            
            _isOnline = serverResult;
        }

        public int UpdateHealth()
        {
            foreach (var com in _communicators.Values)
            {
                var daysToGo = com.DaysToNextInterval();
                
                if (daysToGo + com.DaysSubtracted < 0)
                {
                    var deduction = -(daysToGo + com.DaysSubtracted);
                    _health -= deduction * DailyLoss;
                    com.DaysSubtracted += deduction;
                    
                    if (_health < 0)
                        _health = 0;
                }
            }
            
            return _health;
        }
        
        public string Name
        {
            get { return _name; }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
        }
        
        public int Health
        {
            get => _health;
            set => _health = value;
        }
        
        public int CircleNum
        {
            get => _circleNum;
            set => _circleNum = value;
        }

        public IDictionary<ComTypes, Communicator> Communicators
        {
            get { return _communicators; }
        }

        public Texture Avatar
        {
            get => _avatar;
            set => _avatar = value;
        }

        public bool ShouldNotify 
        {
            get => _shouldNotify;
            set => _shouldNotify = value;
        }

        public bool IsOnline 
        {
            get => _isOnline;
            set => _isOnline = value;
        }
    }
}