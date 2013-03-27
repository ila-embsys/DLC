using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DLCLibrary;


namespace WpfApplication2
{
    public class EDIaryStructure
    {
        public string subject { get; set; }
        public string semester { get; set; }
        public string rating { get; set; }
        public string controlType { get; set; }
        public string mark { get; set; }
        public string date { get; set; }
        public string sign { get; set; }
        public string color { get; set; }
    }

    public class ChangeLogStructure
    {
        public string Student { get; set; }
        public string Group { get; set; }
        public string Subject { get; set; }
        public string Variable { get; set; }
        public string Rating { get; set; }
        public string Date { get; set; }
        public string Sign { get; set; }
    }

    public partial class WorkspacePage : Page
    {
        private DE_IFMO_Checker checker;

        public WorkspacePage(DE_IFMO_Checker checker)
        {
            InitializeComponent();
            this.checker = checker;
            EDiaryGrid.ItemsSource = PrepareDataForDiary();
            ChangeLog.ItemsSource = PrepareDataForChangeLog();
        }

        private List<ChangeLogStructure> PrepareDataForChangeLog()
        {
            List<List<string>> changeLog = checker.GetChangeLog();
            List<ChangeLogStructure> EDiary = new List<ChangeLogStructure>();
            foreach (List<string> record in changeLog)
            {
                if (record.Count > 1)
                    EDiary.Add(new ChangeLogStructure()
                    {
                        Student = record[0],
                        Group = record[1],
                        Subject = record[2],
                        Variable = record[3],
                        Rating = record[4],
                        Date = record[5],
                        Sign = record[6],
                    });
            }
            return EDiary;
        }

        private List<EDIaryStructure> PrepareDataForDiary()
        {
            List<List<string>> diary = checker.GetEDiary();
            List<EDIaryStructure> eDiary = new List<EDIaryStructure>();
            foreach (List<string> record in diary)
            {
                eDiary.Add(new EDIaryStructure()
                {
                    subject = record[0],
                    semester = record[1],
                    rating = record[2],
                    controlType = record[3],
                    mark = record[4],
                    date = record[5],
                    sign = record[6],
                    color = record[7],
                });
            }

            return eDiary;
        }
    }
}
