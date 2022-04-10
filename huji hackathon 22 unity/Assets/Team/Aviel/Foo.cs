using System;
using System.Collections.Generic;
using System.Linq;
using Team.Ethan;
//using UnityEditor.UI;
using UnityEngine;
using static Team.Ethan.Communicator;

class Foo : MonoBehaviour
{
    private int _ind;
    private List<Contact> _sortedContactsList = new List<Contact>();
    private int _listLength;
    public Dictionary<string, Contact> _contactDict;

    private List<Contact> SortCallNext(List<Contact> contacts)
    {
        List<Contact> contactsList = new List<Contact>();
        List<Contact> onlineContactsList = new List<Contact>();
        List<Contact> onlineNeverContactsList = new List<Contact>();
        foreach (Contact contact in contacts)
        {
            var voiceCall = contact.Communicators[ComTypes.VoiceCall];
            if (voiceCall.Interval > 0 & contact.IsOnline == false)
                contactsList.Add(contact);
            else
            {
                if (voiceCall.Interval > 0 & contact.IsOnline == true)
                    onlineContactsList.Add(contact);
                else
                    onlineNeverContactsList.Add(contact);
            }
        }
        List<Contact> sortedOnlineContacts =
            onlineContactsList.OrderBy(o => o.Communicators[ComTypes.VoiceCall].DaysToNextInterval()).ToList();
        List<Contact> sortedContacts =
            contactsList.OrderBy(o => o.Communicators[ComTypes.VoiceCall].DaysToNextInterval()).ToList();
        sortedOnlineContacts.AddRange(onlineNeverContactsList);
        sortedContacts.AddRange(sortedOnlineContacts);
        return sortedContacts;
    }

    private void GoToRecommended(string contactName)
    {
        // GameObject.Find("Camera").GetComponent(CameraScript).MoveToContact(contactName);
    }

    private void RecommendedCall()
    {
        _sortedContactsList = SortCallNext(_contactDict.Values.ToList());
        _listLength = _sortedContactsList.Count;
        _ind = 0;
        GoToRecommended(_sortedContactsList[_ind].Name);
    }

    private void Next()
    {
        _ind += 1;
        if (IsInBounds(_ind))
            _ind -= 1;
        GoToRecommended(_sortedContactsList[_ind].Name);
    }
    
    private void Prev()
    {
        _ind -= 1;
        if (IsInBounds(_ind))
            _ind += 1;
        GoToRecommended(_sortedContactsList[_ind].Name);
    }

    private bool IsInBounds(int ind)
    {
        return ind < _listLength & ind > 0;
    }
}