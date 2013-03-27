using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace DLCLibrary
{
    public class Settings : DLCBase
    {
        public Settings()
            : base()
        {
            if (!File.Exists(Path.Combine(Environment.SpecialFolder.MyDocuments.ToString(), SETTINGS_Filename)))
                CreateOnFirstStart();
        }

        public void Update(Dictionary<string, string> settings)
        {
            Serialize(settings, Environment.SpecialFolder.MyDocuments.ToString(), SETTINGS_Filename);
        }

        public Dictionary<string, string> LoadSettings()
        {
            return (Dictionary<string, string>)Deserialize(Environment.SpecialFolder.MyDocuments.ToString(), SETTINGS_Filename);
        }

        private void CreateOnFirstStart()
        {
            File.Create(Path.Combine(Environment.SpecialFolder.MyDocuments.ToString(), SETTINGS_Filename));
            Dictionary<string, string> setts = new Dictionary<string, string>();
            setts.Add("CHANGE_LOG_INTERVAL", "25");
            Serialize(setts, Environment.SpecialFolder.MyDocuments.ToString(), SETTINGS_Filename);
        }
    }
}
