using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace DLCLibrary
{
    public class DE_IFMO_Checker : DLCBase
    {
        private CookieContainer DE_IFMO_Cookies;
        private DataComparator checker;
        private bool success = false;

        public DE_IFMO_Checker(string login, string password)
            : base()
        {
            DE_IFMO_Cookies = new CookieContainer();
            checker = new DataComparator();
            if (!UserFileExists())
                GetUserInfo(login, password);
            else
                success = true;
        }

        public DE_IFMO_Checker()
            : base()
        {
            DE_IFMO_Cookies = new CookieContainer();
            checker = new DataComparator();
            success = true;
        }

        public bool UserFileExists()
        {
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), USER_Filename)))
                return false;
            return true;
        }

        public bool GetUserInfo(string login, string password)
        {
            string response = TryAuthorize(login, password);
            if (!response.Contains("Access is forbidden") && !response.Contains("Доступ запрещен") && response != null)
            {
                response = MakeRequest("GET", DE_IFMO_RU_UserInfo, ref DE_IFMO_Cookies);
                response = Regex.Replace(response, @"(<[^>]*>)|(\n{1,})|(\s{1,})", " ");
                string userData = response.Substring(response.IndexOf("Фамилия"), response.IndexOf("E-mail") - response.IndexOf("Фамилия"));
                string[] dataArray = userData.Trim().Split(' ');
                string initials = String.Format("{0} {1} {2}", dataArray[1], dataArray[3], dataArray[5]);
                string sex = dataArray[7];
                string birth = dataArray[11];
                string group = dataArray[dataArray.Length - 1];
                response = MakeRequest("GET", DE_IFMO_RU_EDiary_Page, ref DE_IFMO_Cookies);
                string match = Regex.Matches(response, "UNIVER&quot;\\sValue=.*([0-9]{1})&quot")[0].Value;
                string univer = match.Substring(match.LastIndexOf(';') + 1, 1);
                User currentUser = new User(login, password, initials, sex, group, birth, univer);
                success = true;
                return true;
            }
            success = false;
            return false;
        }

        private string GetMainInfo(string rule, string login, string group, string univer, string apprenticeship, string invterval)
        {
            string BACK = "<Back Rule=\"ERegister\"><Parameter Name=\"ST_GRP\" Value=\"\"/>" +
                                                    "<Parameter Name=\"PERSONID\"  Value=\"" + login + "\"/>" +
                                                    "<Parameter Name=\"UNIVER\"  Value=\"" + univer + "\"/>" +
                                                    "<Parameter Name=\"PROGRAMID\" Value=\"" + "-" + "\"/>" +
                                                    "<Parameter Name=\"APPRENTICESHIP\" Value=\"" + apprenticeship + "\"/>" +
                                                    "</Back>";
            string data = String.Format("Rule={0}&ST_GRP={1}&PERSONID={2}&PROGRAMID=-&UNIVER=1&APPRENTICESHIP={3}&BACK={4}&PERIOD={5}", rule, group, login, apprenticeship, BACK, invterval);
            string response = MakeRequest("POST", DE_IFMO_DistributedCDE, ref DE_IFMO_Cookies, data);
            return response;
        }

        private List<List<string>> GetEDiaryInternal(string login, string group, string univer, string apprenticeship, string invterval)
        {
            string response = GetMainInfo("eRegisterPageSelector", login, group, univer, apprenticeship, invterval);
            int index = response.IndexOf("<table class=\"d_table\">\n<form name=\"IntermediateExams\"");
            string cut = response.Substring(index);
            cut = cut.Substring(0, cut.IndexOf("/table>") + "/table>".Length);
            MatchCollection matches = Regex.Matches(cut, "</td>.*</td>");
            List<List<string>> diary = new List<List<string>>();
            List<string> subject;
            string value;
            bool gotString;
            string colorName = "Default";
            string[] data;
            foreach (Match match in matches)
            {
                value = match.Value;
                subject = new List<string>();
                gotString = false;

                if (value.IndexOf("color") != -1)
                {
                    colorName = value.Substring(value.IndexOf("color:") + "color:".Length);
                    colorName = colorName.Substring(0, colorName.IndexOf(";"));
                }

                data = Regex.Split(value, "</td>");

                for (int i = 0; i < data.Length; i++)
                {
                    string buf = Regex.Replace(data[i], "<a.*?>|</a>|<td.*?>|</td>|<input.*?>", String.Empty).Trim();
                    if (buf != String.Empty)
                        gotString = true;
                    if (gotString)
                        subject.Add(buf);
                }
                subject.Add(colorName);
                diary.Add(subject);
            }
            return diary;
        }

        private List<List<string>> GetChangeLogInternal(string login, string group, string univer, string apprenticeship, string invterval)
        {
            string response = GetMainInfo("eRegisterGetProtokolVariable", login, group, univer, apprenticeship, invterval);
            int startIndex = response.IndexOf("<table class=\"d_table\">\n<tr>\n<th width=\"15%\">Студент");
            string cut = response.Substring(startIndex);
            cut = cut.Substring(0, cut.IndexOf("</table>"));
            cut = Regex.Replace(cut, @"\s{2,}", "");
            cut = Regex.Replace(cut, @"\n{1,}", "");

            string buf = Regex.Replace(cut, "<tr>", String.Empty);
            string[] rows = Regex.Split(buf, @"</tr>");
            List<List<string>> rowsData = new List<List<string>>();
            for (int i = 1; i < rows.Length; i++)
            {
                string current = rows[i];
                string modified = Regex.Replace(current, "<td>", String.Empty);
                modified = Regex.Replace(modified, "<br>", " ");
                string[] splited = Regex.Split(modified, "</td>");
                rowsData.Add(splited.ToList<string>());
            }
            return rowsData;
        }

        private List<int> GetUserTopInternal()
        {
            string data = "Rule=REP_SHOWREPORTPARAMFORM&REP_ID=1441&IN_ADMIN=0&BACK=<Back Rule=\"rep_UsrShowReports\"></Back>";
            string response = MakeRequest("POST", DE_IFMO_DistributedCDE, ref DE_IFMO_Cookies, data);
            MatchCollection matches = Regex.Matches(response, "([0-9]{1,})\\s{1}из\\s{1}([0-9]{1,})"); // x из y
            string[] splitedData = matches[0].Value.Split(' ');
            int position = Int32.Parse(splitedData[0]);
            int allPositions = Int32.Parse(splitedData[2]);
            List<int> values = new List<int>();
            values.Add(position);
            values.Add(allPositions);
            return values;
        }

        public List<List<string>> GetEDiary()
        {
          string response = TryAuthorize();
          if (!response.Contains("Access is forbidden") && !response.Contains("Доступ запрещен") && response != null)
          {
              User user = new User();
              Dictionary<string, string> data = user.GetUserData();
              return GetEDiaryInternal(data["LOGIN"], data["GROUP"], data["UNIVER"], data["APPRENTICESHIP"], data["INTERVAL"]);
          }
          return null;
        }

        public List<List<string>> GetChangeLog()
        {
            string response = TryAuthorize();
            if (!response.Contains("Access is forbidden") && !response.Contains("Доступ запрещен") && response != null)
            {
                User user = new User();
                Dictionary<string, string> data = user.GetUserData();
                return GetChangeLogInternal(data["LOGIN"], data["GROUP"], data["UNIVER"], data["APPRENTICESHIP"], data["INTERVAL"]);
            }
            return null;
        }

        public List<int> GetTopPosition()
        {
            string response = TryAuthorize();
            if (!response.Contains("Access is forbidden") && !response.Contains("Доступ запрещен") && response != null)
            {
                return GetUserTopInternal();
            }
            return null;
        }

        private string TryAuthorize()
        {
            User user = new User();
            Dictionary<string, string> userData = new Dictionary<string, string>();
            userData = user.GetUserData();
            DE_IFMO_Cookies = new CookieContainer();
            string data = String.Format("Rule=LOGON&LOGIN={0}&PASSWD={1}", userData["LOGIN"], userData["PASSWD"]);
            string response = MakeRequest("POST", DE_IFMO_RU, ref DE_IFMO_Cookies, data);
            return response;
        }

        private string TryAuthorize(string login, string passwd)
        {
            DE_IFMO_Cookies = new CookieContainer();
            string data = String.Format("Rule=LOGON&LOGIN={0}&PASSWD={1}", login, passwd);
            string response = MakeRequest("POST", DE_IFMO_RU, ref DE_IFMO_Cookies, data);
            return response;
        }

        public int CheckLogForUpdates(List<List<string>> currentLog)
        {
            int position = checker.CheckChangeLog(currentLog);
            Serialize(currentLog[0], Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), LOG_Filename);
            return position;
        }

        public bool GetStatus()
        {
            return success;
        }
    }
}
