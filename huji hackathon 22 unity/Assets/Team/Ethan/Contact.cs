using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Team.Ethan.Communicator;
using Random = System.Random;

namespace Team.Ethan
{
    public class Contact
    {
        //unique identifiers
        private string _name;
        private string _phoneNumber;

        private Dictionary<ComTypes, Communicator> _communicators = 
                                                    new Dictionary<ComTypes, Communicator>();
        private Sprite _avatar;
        private int _health;
        private bool _shouldNotify;
        private bool _isOnline;

        public Contact(string name, string phoneNumber, Sprite avatar)
        {
            _phoneNumber = phoneNumber;
            _name = name;
            _avatar = avatar;
            _health = 100; // the health of your relationship
            _shouldNotify = false;
            _isOnline = false;

            CreateDefaultComs();
        }

        private void CreateDefaultComs()
        {
            foreach (ComTypes comType in Enum.GetValues(typeof(ComTypes)))
            {
                _communicators.Add(comType, new Communicator(comType));
            }
        }

        public void AddCommunicator(ComTypes comType, int interval)
        {
            _communicators.Add(comType, new Communicator(comType, interval));
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
            //TODO: Aviel is doing this

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
            get { return _health; }
        }

        public IDictionary<ComTypes, Communicator> Communicators
        {
            get { return _communicators; }
        }

        public Sprite Avatar
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