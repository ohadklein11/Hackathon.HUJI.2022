using System;
using System.Collections.Generic;
using System.Linq;
using Team.Ethan;
using UnityEditor.UI;
using UnityEngine;
using static Team.Ethan.Communicator;

class Foo : MonoBehaviour
{
    public List<Contact> CallNext(Contact[] contacts)
    {
        List<Contact> contactsList = new List<Contact>();
        List<Contact> onlineContactsList = new List<Contact>();
        List<Contact> onlineNeverContactsList = new List<Contact>();
        foreach (Contact contact in contacts)
        {
            var voiceCall = contact.Communicators[ComTypes.VoiceCall];
            if (voiceCall.Interval > 0 & contact.IsOnline == false)
            {
                contactsList.Add(contact);
            }
            else
            {
                if (voiceCall.Interval > 0 & contact.IsOnline == true)
                {
                    onlineContactsList.Add(contact);
                }
                else
                {
                    onlineNeverContactsList.Add(contact);
                }
            }
        }
        List<Contact> sortedOnlineContacts =
            onlineContactsList.OrderBy(o => o.Communicators[ComTypes.VoiceCall].DaysToNextInterval()).ToList();
        List<Contact> sortedContacts =
            contactsList.OrderBy(o => o.Communicators[ComTypes.VoiceCall].DaysToNextInterval()).ToList();
        onlineContactsList.AddRange(onlineNeverContactsList);
        sortedContacts.AddRange(onlineContactsList);
        return sortedContacts;
    }
}