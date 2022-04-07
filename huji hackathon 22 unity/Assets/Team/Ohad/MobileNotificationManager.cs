using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class MobileNotificationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // remove notifications that have already been displayed
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
        notification.Title = "Hey! Come back!";
        notification.Text = "Call someone!";
        notification.FireTime = System.DateTime.Now.AddSeconds(15);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        // if the script is run and a message is already scheduled, cancel it and re-schedule another message
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
