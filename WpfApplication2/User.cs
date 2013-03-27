using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.IO;

namespace DLCLibrary
{
    public class User : DLCBase
    {
        public User(string login, string passwd, string initials, string sex, string group, string birtday, string appren, string interval, string univer = "1")
            : base()
        {
            Update(login, passwd, initials, sex, group, birtday, appren, interval, univer);
        }

        public User(string login, string password, string initials, string sex, string group, string birth, string univer = "1")
        {
            string appren = String.Format("{0}/{1}", DateTime.Now.Year - 1, DateTime.Now.Year);
            Update(login, password, initials, sex, group, birth, appren, "7", univer);
        }

        public User()
        {

        }

        public Dictionary<string, string> GetUserData()
        {
            return (Dictionary<string, string>)Deserialize(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), USER_Filename);
        }

        public void Update(string login, string passwd, string initials, string sex, string group, string birtday, string appren, string interval, string univer = "1")
        {
            userData = new Dictionary<string, string>();
            userData.Add("LOGIN", login);
            userData.Add("PASSWD", passwd);
            userData.Add("INITIALS", initials);
            userData.Add("GROUP", group);
            userData.Add("BIRTHDAY", birtday);
            userData.Add("UNIVER", univer);
            userData.Add("APPRENTICESHIP", appren);
            userData.Add("INTERVAL", interval);
            Serialize(userData, Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), USER_Filename);
        }

        public void RemoveUser()
        {
            if(File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), USER_Filename)))
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), USER_Filename));
        }

        private Dictionary<string, string> userData;
    }
}
