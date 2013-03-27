using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace DLCLibrary
{
    public class DLCBase
    {
        public static string DE_IFMO_RU = "https://de.ifmo.ru/servlet";
        public static string ISU_IFMO_RU = "http://isu.ifmo.ru";
        public static string DE_IFMO_RU_UserInfo = "https://de.ifmo.ru/servlet/distributedCDE?Rule=editPersonProfile";
        public static string DE_IFMO_RU_Profile = "https://de.ifmo.ru/servlet/distributedCDE?Rule=editPersonProfile";
        public static string DE_IFMO_RU_EDiary_Page = "https://de.ifmo.ru/servlet/distributedCDE?Rule=eRegister";
        public static string DE_IFMO_DistributedCDE = "https://de.ifmo.ru/servlet/distributedCDE";
        public static string LOG_Filename = "Log.bin";
        public static string USER_Filename = "User.bin";
        public static string SETTINGS_Filename = "Settings.bin";
        private readonly object _locker = new object();

        public DLCBase()
        {

        }

        public void Serialize(Object data, string path, string fileName)
        {
            lock (_locker)
            {
                Stream writeData = File.Open(Path.Combine(path, fileName), FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(writeData, data);
                writeData.Close();
            }
        }

        public object Deserialize(string path, string fileName)
        {
            lock (_locker)
            {
                Stream readData = File.Open(Path.Combine(path, fileName), FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                object returnValue = bf.Deserialize(readData);
                readData.Close();
                return returnValue;
            }
        }

        public void DeleteFile(string path, string fileName)
        {
            File.Delete(Path.Combine(path, fileName));
        }

        public string MakeRequest(string method, string URL, ref CookieContainer cookies, string data = null)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = method;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ProtocolVersion = HttpVersion.Version10;
                request.CookieContainer = cookies;
                if (method.ToLower() == "post")
                {
                    byte[] byteData = (new ASCIIEncoding()).GetBytes(data);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(byteData, 0, byteData.Length);
                    }
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                cookies.Add(response.Cookies);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("Windows-1251")))
                {
                    string ret = reader.ReadToEnd();
                    if (ret != String.Empty)
                        return ret;
                    else
                        throw new WebException();
                }
            }
            catch (WebException)
            {
                return String.Empty;
            }
        }
    }
}
