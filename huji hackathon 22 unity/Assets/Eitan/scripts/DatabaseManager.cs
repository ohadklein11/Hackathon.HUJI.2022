using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;
using UnityEngine.AI;

namespace Team.Ohad
{
    public class DatabaseManager : MonoBehaviour
    {
        public InputField PhoneNumber;
        public Toggle IsOnline;
        public GameObject SaveButton;
        public GameObject PhoneNumberObject;
        public GameObject IsOnlineObject;
        
        private string phoneNumber;

        private void Awake()
        {
            phoneNumber = gameObject.name;
        }


        private DatabaseReference dbReference;
        // Start is called before the first frame update
        void Start()
        {
            dbReference = FirebaseDatabase.GetInstance("https://hackathonproject2022-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
            print("started");
        }

        public void CreateUser()
        {
            FirebaseUser newUser = new FirebaseUser( false);
            string json = JsonUtility.ToJson(newUser);
            phoneNumber = PhoneNumber.text;
            dbReference.Child("users").Child(phoneNumber).SetRawJsonValueAsync(json);

            IsOnlineObject.SetActive(true);
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

        public void UpdateIsOnline()
        {
            dbReference.Child("users").Child(phoneNumber).SetValueAsync(IsOnline.isOn);
        }
    }
}
