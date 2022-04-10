using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;
using UnityEngine.AI;

public class FireBaseContact : MonoBehaviour
    {
        public InputField PhoneNumber;
        public Toggle IsAlerted;
        public GameObject SaveButton;
        public GameObject PhoneNumberObject;
        public GameObject IsAlertedObject;
        
        private string ohadPhone;

        private string phoneNumber;

        private DatabaseReference dbReference;
        // Start is called before the first frame update
        void Start()
        {
            dbReference = FirebaseDatabase.GetInstance("https://hackathonproject2022-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
        }

        public void CreateUser()
        {
            phoneNumber = PhoneNumber.text;
            IsAlertedObject.SetActive(true);
            SaveButton.SetActive(false);
            PhoneNumberObject.SetActive(false);
        }

        public IEnumerator GetIsOnline(Action<string> onCallback)
        {
            var userIsOnlineData = dbReference.Child("users").Child(phoneNumber).GetValueAsync();
            yield return new WaitUntil(predicate: () => userIsOnlineData.IsCompleted);
            if (userIsOnlineData != null)
            {
                DataSnapshot snapshot = userIsOnlineData.Result;
                
                onCallback.Invoke(snapshot.Value.ToString());
            }
        }

        public void AlertUser()
        {
            if (IsAlerted.isOn)
            {
                dbReference.ChildChanged += HandleChildValueChanged;
            }
            else
            {
                dbReference.ChildChanged -= HandleChildValueChanged;
            }
        }

        private void SendNotif()
        {
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        
            var channel = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Notification Channel",
                Importance = Importance.Default,
                Description = "Reminder notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            var notification = new AndroidNotification();
            notification.Title = "A friend is available!";
            notification.Text = "Ohad is available!";
            notification.FireTime = DateTime.Now;

            var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

            //if the script is run and a message is already scheduled, cancel it and re-schedule another message
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
            {
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.SendNotification(notification, "channel_id");
            }
        }
        
        void HandleChildValueChanged(object sender, ChildChangedEventArgs args) {
            if (args.DatabaseError != null) {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }

            var dictionary = (Dictionary<String, System.Object>) args.Snapshot.Value;
            var isAvailable = "false";
            foreach (KeyValuePair<String, System.Object> kvp in dictionary)
            {
                if (kvp.Key.Equals(phoneNumber))
                {
                    isAvailable = kvp.Value.ToString();
                }
            }

            if (isAvailable == "True")
            {
                SendNotif();
            }
        }
    }
