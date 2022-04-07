using System.Collections;
using Unity.Notifications.Android;

namespace Team.Ethan
{
    public class NotificationController
    {
        public static IEnumerator NotifyWhenOnline(Contact contact)
        {
            while (!isOnline(contact) || !contact.ShouldNotify) 
            { 
                yield return null;
            }
            
            NotifyUser(contact);
        }

        private static bool isOnline(Contact contact)
        {
            //TODO: check server/update value
            return false;
        }

        private static void NotifyUser(Contact contact)
        {
            // remove notifications that have already been displayed
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            
            var channel = new AndroidNotificationChannel()
            {
                Id = contact.Name,
                Name = "Notification Channel",
                Importance = Importance.Default,
                Description = "Reminder notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            var notification = new AndroidNotification();
            notification.Title = contact.Name + " is online!";
            notification.Text = "Tap to contact them!";
            notification.FireTime = System.DateTime.Now;

            var id = AndroidNotificationCenter.SendNotification(notification, contact.Name);

            // if the script is run and a message is already scheduled, cancel it and re-schedule another message
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
            {
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.SendNotification(notification, contact.Name);
            }
        }
    }
}