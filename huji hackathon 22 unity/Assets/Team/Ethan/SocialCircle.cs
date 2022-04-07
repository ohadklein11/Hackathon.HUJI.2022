using System;
using System.Collections.Generic;

namespace Team.Ethan
{
    public class SocialCircle
    {
        private Dictionary<string, Contact>_contacts = new Dictionary<string, Contact>();
        private int _number;
        private string _name;
        private Tuple<int, int> _location;

        public SocialCircle(int num, string name, Tuple<int, int> location)
        {
            _number = num;
            _name = name;
            _location = location;
        }

        public void AddContact(Contact contact)
        {
            _contacts.Add(contact.Name, contact);
        }

        public Dictionary<string, Contact> Contacts => _contacts;
    
        public int Number => _number;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public Tuple<int, int> Location
        {
            get => _location;
            set => _location = value;
        }
    }
}