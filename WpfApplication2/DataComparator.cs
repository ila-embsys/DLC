using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DLCLibrary
{
    public class DataComparator : DLCBase
    {
        public DataComparator()
            : base()
        {

        }
        
        /*
         * Return value = -2 : File Created and Data serialized
         * Return value >= 0 : Position of lastSavedLog in current. Number of new records
         * Return value = -1: All records are new
         */
        public int CheckChangeLog(List<List<string>> current)
        {
            List<string> lastSavedLog;
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), LOG_Filename)))
            {
                lastSavedLog = (List<string>)Deserialize(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), LOG_Filename);
                for (int i = 0; i < current.Count; i++)
                {
                    if (Enumerable.SequenceEqual(current[i].OrderBy(t => t), lastSavedLog.OrderBy(t => t)))
                        return i;
                }
                return -1;
            }
            else
            {
                File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), LOG_Filename));
                Serialize(current[0], Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), LOG_Filename);
                return -2;
            }

        }

        public void CheckScholarship()
        {

        }
    }
}
