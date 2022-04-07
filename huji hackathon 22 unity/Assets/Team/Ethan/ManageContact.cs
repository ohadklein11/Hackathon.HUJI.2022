using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;
using static Team.Ethan.Communicator;

namespace Team.Ethan
{
    public class ManageContact : MonoBehaviour
    {
        private Dictionary<string, Contact> contactsCollection;
        private Contact contact;
        private Texture defaultImage; //TODO: get default image
        
        public Image avatarImg;
        
        public TextField nameFld;
        public TextField phoneNumberFld;
        public TextField textMessageFld;
        public TextField voiceCallFld;
        public TextField videoCallFld;
        public TextField meetFld;
        
        public Dropdown phoneNumberDropdown;
        public Dropdown voiceCallDropdown;
        public Dropdown videoCallDropdown;
        public Dropdown meetDropdown;

        private Dropdown[] _dropdowns;
        private Dictionary<ComTypes,TextField> _comFldObjsDict;
        private Dictionary<string,int> _intervalValues = new Dictionary<string, int>() 
            {{"Never", 0},{"Every Day",1},{"Every Two Days",2},{"Every Three Days",3},{"Every Four Days",4}
                ,{"Every Five Days",5},{"Every Six Days",6},{"Every Week",7},{"Every Two Weeks",14},
                {"Every Three Weeks",21},{"Every Month",31},{"Every Two Months",62},{"Every Three Months",92},
                {"Every Six Months",183},{"Yearly",365}};

        // Start is called before the first frame update
        void Start()
        {
            _dropdowns = new Dropdown[] {phoneNumberDropdown, voiceCallDropdown, videoCallDropdown, meetDropdown};
            _comFldObjsDict = new Dictionary<ComTypes,TextField>() {{ComTypes.Text, textMessageFld}, 
                {ComTypes.VoiceCall, voiceCallFld}, 
                {ComTypes.VideoCall, videoCallFld}, 
                {ComTypes.Meeting, meetFld}};
            
            if (contact != null)
                PrepareEditMode();
            else
                avatarImg.image = defaultImage;
            
            SetUpDropdownLists();
        }

        private void SetUpDropdownLists()
        {
            List<Dropdown.OptionData> items = new List<Dropdown.OptionData>();
            foreach (var text in _intervalValues.Keys)
            {
                var item = new Dropdown.OptionData();
                item.text = text;
                items.Add(item);
            }
            
            foreach (var dropdown in _dropdowns)
            {
                dropdown.ClearOptions();
                dropdown.options.AddRange(items);
            }
        }

        private void PrepareEditMode()
        {
            nameFld.value = contact.Name;
            avatarImg.image = contact.Avatar;
            phoneNumberFld.value = contact.PhoneNumber;
            var coms = contact.Communicators;
            
            foreach (ComTypes comType in Enum.GetValues(typeof(ComTypes)))
            {
                _comFldObjsDict[comType].value = "Every" + coms[comType].Interval + "Days";
            }
        }

        public void SaveContact()
        {
            var contactName = nameFld.value;

            if (NameIsTaken(contactName))
            {
                nameFld.value = "Name already in use";
                return;
            }
            
            var number = phoneNumberFld.value;
            var newContact = new Contact(contactName, number, avatarImg.image);
            
            foreach (ComTypes comType in Enum.GetValues(typeof(ComTypes)))
            {
                newContact.AddCommunicator(comType, _intervalValues[_comFldObjsDict[comType].value], DateTime.Now);
            }
            
            contactsCollection.Add(name, newContact);
            
            ContactsDataManager.SaveContact(newContact);
        }

        private bool NameIsTaken(string contactName)
        {
            return string.IsNullOrEmpty(contactName) || contactsCollection.ContainsKey(contactName);
        }
    }
}
