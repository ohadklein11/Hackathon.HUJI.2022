using System.Collections;

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
            
        }
    }
}