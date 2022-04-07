using System;
using System.Collections.Generic;
using Team.Ethan;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;
using static Team.Ethan.Communicator;

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
    
    
    
    private Dictionary<ComTypes,TextField> _comFldObjsDict;
    private Dictionary<string,int> _intervalValues = new Dictionary<string, int>() 
        {{"Never", 0},{"Every Day",1},{"Every Two Days",2},{"Every Three Days",3},{"Every Four Days",4}
            ,{"Every Five Days",5},{"Every Six Days",6},{"Every Week",7},{"Every Two Weeks",14},
            {"Every Three Weeks",21},{"Every Month",31},{"Every Two Months",62},{"Every Three Months",92},
            {"Every Six Months",183},{"Yearly",365}};

    // Start is called before the first frame update
    void Start()
    {
        _comFldObjsDict = new Dictionary<ComTypes,TextField>() {{ComTypes.Text, textMessageFld}, 
            {ComTypes.VoiceCall, voiceCallFld}, 
            {ComTypes.VideoCall, videoCallFld}, 
            {ComTypes.Meeting, meetFld}};
        
        if (contact != null)
            PrepareEditMode();
        else
            avatarImg.image = defaultImage;
        
        SetUpDropdownList();
    }

    private void SetUpDropdownList()
    {
        // textMessageFld
        // //Fetch the Dropdown GameObject the script is attached to
        // m_Dropdown = GetComponent<Dropdown>();
        // //Clear the old options of the Dropdown menu
        // m_Dropdown.ClearOptions();
        //
        // //Create a new option for the Dropdown menu which reads "Option 1" and add to messages List
        // m_NewData = new Dropdown.OptionData();
        // m_NewData.text = "Option 1";
        // m_Messages.Add(m_NewData);
        //
        // //Create a new option for the Dropdown menu which reads "Option 2" and add to messages List
        // m_NewData2 = new Dropdown.OptionData();
        // m_NewData2.text = "Option 2";
        // m_Messages.Add(m_NewData2);
        //
        // //Take each entry in the message List
        // foreach (Dropdown.OptionData message in m_Messages)
        // {
        //     //Add each entry to the Dropdown
        //     m_Dropdown.options.Add(message);
        //     //Make the index equal to the total number of entries
        //     m_Index = m_Messages.Count - 1;
        // }
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
        var name = nameFld.value;

        if (NameIsTaken(name))
        {
            nameFld.value = "Name already in use";
            return;
        }
        
        var number = phoneNumberFld.value;
        var newContact = new Contact(name, number, avatarImg.image);
        
        foreach (ComTypes comType in Enum.GetValues(typeof(ComTypes)))
        {
            newContact.AddCommunicator(comType, _intervalValues[_comFldObjsDict[comType].value]);
        }
        
        contactsCollection.Add(name, newContact);
    }

    private bool NameIsTaken(string name)
    {
        return string.IsNullOrEmpty(name) || contactsCollection.ContainsKey(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
