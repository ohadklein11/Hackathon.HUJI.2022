using System;
using Team.Ethan;
using UnityEditor.UI;
using UnityEngine;
using static Team.Ethan.Communicator;

class Foo : MonoBehaviour
{
    private int sec_in_day = 86400;
    
    private int dailyLoss = 3;

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        /* Communicator com = new Communicator(ComTypes.Meeting, sec_in_day * 7);
        DateTime last = com.LastComDate; */
        foreach (Communicator com in listofcommunicators)
        {
            int daysSubtracted = com.daysSubtracted;
            DateTime com._lastComDate = new DateTime(2022, 4, 1, 14, 37, 20);
            TimeSpan interval = new TimeSpan(com._interval * 24);
            DateTime next = com._lastComDate.Add(interval);
            int daysToGo = next.Subtract(DateTime.Now).Days;

            if (daysToGo + daysSubtracted < 0)
            {
                this._health += (daysToGo + daysSubtracted) * dailyLoss;
                daysSubtracted += 1;
                if (this._health < 0)
                {
                    this._health = 0;
                }
            }
        }

    }
}