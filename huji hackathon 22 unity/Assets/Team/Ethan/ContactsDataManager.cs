using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using static Team.Ethan.Communicator;

namespace Team.Ethan
{
    public class ContactsDataManager
    {
        private const string FilePath = @"test.csv";
        private const string ImagePath = @"avatar_images/";

        public ContactsDataManager()
        {
            if(!File.Exists(FilePath))
                CreateNewSaveFile();
        }

        private static void CreateNewSaveFile()
        {
            var myHeaders = new string[]
            {
                "Name", "Number", "Health", "Should Notify",
                "Text Interval","Text Date","Text Days Subtracted",
                "Voice Interval","Voice Date","Voice Days Subtracted",
                "Video Interval","Video Date","Video Days Subtracted",
                "Meet Interval","Meet Date","Meet Days Subtracted"
            };
            
            File.WriteAllText(FilePath, string.Join(Environment.NewLine, 
                new[]{myHeaders}.Select(line => string.Join(",", line))));
        }

        public static Dictionary<string,Contact> LoadContacts()
        {
            using(var reader = new StreamReader(FilePath))
            {
                Dictionary<string,Contact> contactsDict = new Dictionary<string,Contact>();
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    var rawData = File.ReadAllBytes(ImagePath + values[0] + ".jpg");
                    Texture2D avatar = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (that's what she said)
                    avatar.LoadImage(rawData);
                    
                    var index = 0;
                    var contact = new Contact(values[index++], values[index++], avatar);
                    contact.Health = Convert.ToInt32(values[index++]);
                    contact.ShouldNotify = Convert.ToBoolean(values[index++]);

                    foreach (ComTypes comType in Enum.GetValues(typeof(ComTypes)))
                    {
                        contact.AddCommunicator(comType, Convert.ToInt32(values[index++]), 
                            Convert.ToDateTime(values[index++]), Convert.ToInt32(values[index++]));
                    }
                    
                    contactsDict.Add(contact.Name, contact);
                }

                return contactsDict;
            }
        }

        public static void SaveContact(Contact contact)
        {
            var dataList = new List<string>() {contact.Name, contact.PhoneNumber, 
                contact.Health.ToString(), contact.ShouldNotify.ToString()};

            foreach (var com in contact.Communicators.Values)
            {
                dataList.Add(com.Interval.ToString());
                dataList.Add(com.LastComDate.ToString());
                dataList.Add(com.DaysSubtracted.ToString());
            }
            
            dataList.Add(Environment.NewLine);
            
            string csvLine = String.Join(";", dataList);;
            byte[] csvLineBytes = Encoding.Default.GetBytes(csvLine);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(csvLineBytes , 0, csvLineBytes.Length);
                using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {                        
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);                    
                }

                using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Write))
                {
                    ms.WriteTo(file);
                    //TODO: save image to image path
                }
            }
        }
    }
}