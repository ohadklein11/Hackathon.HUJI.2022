using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using static Team.Ethan.Communicator;
using Button = UnityEngine.UIElements.Button;

namespace Team.Ethan
{
    public class ContactView : MonoBehaviour
    {
        private Contact contact;
        
        public Text nameTxt;
        public Image avatarImg;
        public Button toggleNotifyBtn;
        public Text isOnlineText;
        public ProgressBar healthBar;

        public Button textMessageBtn;
        public Button voiceCallBtn;
        public Button videoCallBtn;
        public Button meetBtn;
        
        public Text textMessageTxt;
        public Text voiceCallTxt;
        public Text videoCallTxt;
        public Text meetTxt;

        private IDictionary<ComTypes,Text> _comTxtObjsDict;

        // Start is called before the first frame update
        void Start()
        {
            nameTxt.text = contact.Name;
            avatarImg.GetComponent<Image>().sprite = contact.Avatar;
            toggleNotifyBtn.text = contact.ShouldNotify ? "Notify" : "Don't Notify";
            UpdateHealthBar();
            UpdateOnlineStatus();
            
            _comTxtObjsDict = new Dictionary<ComTypes,Text>() {{ComTypes.Text, textMessageTxt}, 
                {ComTypes.VoiceCall, voiceCallTxt}, 
                {ComTypes.VideoCall, videoCallTxt}, 
                {ComTypes.Meeting, meetTxt}};
            
            SetUpComTxt();
        }

        private void SetUpComTxt()
        {
            foreach (ComTypes comType in Enum.GetValues(typeof(ComTypes)))
            {
                _comTxtObjsDict[comType].text = GetDaysToGoText(contact.Communicators[comType].DaysToNextInterval());
            }
        }

        private string GetDaysToGoText(int daysToGo)
        {
            if (daysToGo == -1)
                return "Never";
            if (daysToGo < 1)
                return "Today";
            if (daysToGo < 7)
                return daysToGo + "Days";
            if (daysToGo < 31)
                return daysToGo/7 + "Week" + (daysToGo < 14 ? "" : "s");
            if (daysToGo < 365)
                return daysToGo/31 + "Month" + (daysToGo < 61 ? "" : "s");
            
            return "1 Year";
        }
        
        // Update is called once per frame
        void Update()
        {
            UpdateOnlineStatus();
        }

        private void UpdateHealthBar()
        {
            healthBar.value = contact.UpdateHealth();
        }
        
        private void UpdateOnlineStatus()
        {
            isOnlineText.text = contact.IsOnline ? "Online" : "Offline";
        }
    }
}