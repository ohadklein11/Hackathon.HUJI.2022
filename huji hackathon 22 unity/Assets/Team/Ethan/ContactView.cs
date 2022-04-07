using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using static Team.Ethan.Communicator;
using static Team.Ethan.NotificationController;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;

namespace Team.Ethan
{
    public class ContactView : MonoBehaviour
    {
        private Contact _contact;
        
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

        private Dictionary<ComTypes,Text> _comTxtObjsDict;

        // Start is called before the first frame update
        void Start()
        {
            nameTxt.text = _contact.Name;
            avatarImg.image = _contact.Avatar;
            UpdateNotifyBtn();
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
                _comTxtObjsDict[comType].text = GetDaysToGoText(_contact.Communicators[comType].FormattedDaysToGo());
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

        public void OnClickNotifyBtn()
        {
            _contact.ShouldNotify = !_contact.ShouldNotify;
            
            if (_contact.ShouldNotify)
            {
                var thread = NotifyWhenOnline(_contact);
            }
            
            UpdateNotifyBtn();
        }

        private void UpdateHealthBar()
        {
            healthBar.value = _contact.UpdateHealth();
        }
        
        private void UpdateOnlineStatus()
        {
            //TODO: get online status from server
            isOnlineText.text = _contact.IsOnline ? "Online" : "Offline";
        }
        
        private void UpdateNotifyBtn()
        {
            toggleNotifyBtn.text = _contact.ShouldNotify ? "Notify" : "Don't Notify";
        }
    }
}