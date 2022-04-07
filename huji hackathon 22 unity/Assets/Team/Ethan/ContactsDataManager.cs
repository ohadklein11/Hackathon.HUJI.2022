using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Team.Ethan
{
    public class ContactsDataManager
    {
        private const string FilePath = @"test.csv";
        
        public ContactsDataManager()
        {
            if(!File.Exists(FilePath))
                CreateNewSaveFile();
        }

        private static void CreateNewSaveFile()
        {
            var myHeaders = new string[]
            {
                "Name", "Number", "Avatar", "Health", "Should Notify",
                "Text Date","Text Interval","Text Days Subtracted",
                "Voice Date","Voice Interval","Voice Days Subtracted",
                "Video Date","Video Interval","Video Days Subtracted",
                "Meet Date","Meet Interval","Meet Days Subtracted"
            };
            
            File.WriteAllText(FilePath, string.Join(Environment.NewLine, 
                new[]{myHeaders}.Select(line => string.Join(",", line))));
        }

        public static void SaveContact(Contact contact)
        {
            var dataList = new List<string>() {contact.Name, contact.PhoneNumber, contact.Avatar.ToString(), 
                contact.Health.ToString(), contact.ShouldNotify.ToString()};

            foreach (var com in contact.Communicators.Values)
            {
                dataList.Add(com.LastComDate.ToString());
                dataList.Add(com.Interval.ToString());
                dataList.Add(com.DaysSubtracted.ToString());
            }
            
            dataList.Add(Environment.NewLine);
            
            string csvLine = String.Join(",", dataList);;
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
                }
            }
        }
    }
}