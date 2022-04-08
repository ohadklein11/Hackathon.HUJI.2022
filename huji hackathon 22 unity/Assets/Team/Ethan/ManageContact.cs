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
        private Dictionary<string, Contact> contactsCollection = new Dictionary<string, Contact>();
        private Contact contact;
        private Texture defaultImage;
        
        public Text nameFld;
        public Text phoneNumberFld;

        public Dropdown textDropdown;
        public Dropdown voiceCallDropdown;
        public Dropdown videoCallDropdown;
        public Dropdown meetDropdown;

        private Dropdown[] _dropdowns;
        private Dictionary<ComTypes,Dropdown> _comFldObjsDict;
        private Dictionary<string,int> _intervalValues = new Dictionary<string, int>() 
            {{"None",-1},{"Never", 0},{"Every Day",1},{"Every Two Days",2},{"Every Three Days",3},{"Every Four Days",4}
                ,{"Every Five Days",5},{"Every Six Days",6},{"Every Week",7},{"Every Two Weeks",14},
                {"Every Three Weeks",21},{"Every Month",31},{"Every Two Months",62},{"Every Three Months",92},
                {"Every Six Months",183},{"Yearly",365}};

        private void Awake()
        {
            nameFld = GameObject.Find("Canvas").transform.Find("popupAdd").transform.Find("name").
                transform.Find("Text").gameObject.GetComponent<Text>();
            phoneNumberFld = GameObject.Find("Canvas").transform.Find("popupAdd").transform.Find("phone").
                transform.Find("Text").gameObject.GetComponent<Text>();
            voiceCallDropdown = GameObject.Find("Canvas").transform.Find("popupAdd").
                transform.Find("Dropdown voice").gameObject.GetComponent<Dropdown>();
            textDropdown = GameObject.Find("Canvas").transform.Find("popupAdd").
                transform.Find("Dropdown txt").gameObject.GetComponent<Dropdown>();
            meetDropdown = GameObject.Find("Canvas").transform.Find("popupAdd").
                transform.Find("Dropdown meet").gameObject.GetComponent<Dropdown>();
            videoCallDropdown = GameObject.Find("Canvas").transform.Find("popupAdd").
                transform.Find("Dropdown video").gameObject.GetComponent<Dropdown>();
            
        }

        // Start is called before the first frame update
        void Start()
        {
            
            _dropdowns = new Dropdown[] {textDropdown, voiceCallDropdown, videoCallDropdown, meetDropdown};
            _comFldObjsDict = new Dictionary<ComTypes,Dropdown>() {{ComTypes.Text, voiceCallDropdown}, 
                {ComTypes.VoiceCall, videoCallDropdown}, 
                {ComTypes.VideoCall, textDropdown}, 
                {ComTypes.Meeting, meetDropdown}};
            
            //TODO: get default image
            defaultImage = null;
            
            // avatarImg.image = defaultImage;

            
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

        public void SaveContact()
        {
            var contactName = nameFld.text;

            if (NameIsTaken(contactName))
            {
                nameFld.text = "Name already in use";
                return;
            }
            
            var number = phoneNumberFld.text;
            var newContact = new Contact(contactName, number, null);
            
            foreach (ComTypes comType in Enum.GetValues(typeof(ComTypes)))
            {
                var txt = _comFldObjsDict[comType].captionText.text;
                if (txt == null)
                    return;
                newContact.AddCommunicator(comType, _intervalValues[txt], DateTime.Now);
            }
            
            contactsCollection.Add(name, newContact);
            
            //ContactsDataManager.SaveContact(newContact);
        }

        private bool NameIsTaken(string contactName)
        {
            // return string.IsNullOrEmpty(contactName) || contactsCollection.ContainsKey(contactName);
            return false;
        }
    }
}
