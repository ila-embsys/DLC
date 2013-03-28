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
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        List<List<string>> protocol;
        public NotificationWindow(List<List<string>> protocol)
        {
            this.protocol = protocol;
            InitializeComponent();
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - 500;
            this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - (System.Windows.SystemParameters.PrimaryScreenHeight - System.Windows.SystemParameters.WorkArea.Bottom) - 300;
        }
        /*
         * <Border CornerRadius="3" BorderThickness="3" Margin="3" BorderBrush="#F0007ACC">
                <StackPanel >
                    <TextBlock Margin="1" Background="#33AA33" Foreground="White" FontSize="18"  />
                    <TextBlock Margin="1" Background="#33AA33" Foreground="White" FontSize="18"  />
                    <TextBlock Margin="1" Background="#33AA33" Foreground="White" FontSize="18"  />
                </StackPanel>
            </Border>
         * */
        public void ShowNotification(string subject, string ct, string delta)
        {
            Border border = new Border();
            StackPanel panel = new StackPanel();
            panel.Background = new SolidColorBrush(Color.FromArgb(0xEE, 0x40, 0x40, 0x40));
            TextBlock s = new TextBlock();
            s.Text = subject;
          //  s.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0xAA, 0x33));
            s.FontSize = 16;
            s.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            s.Margin = new Thickness(1);
            s.FontWeight = FontWeights.Bold;
            s.Padding = new Thickness(10, 0, 0, 0);
          
            TextBlock control = new TextBlock();
            control.Text = ct;
          //  control.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0xAA, 0x33));
            control.FontSize = 16;
            control.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            control.Margin = new Thickness(1);
            control.FontWeight = FontWeights.Bold;
            control.Padding = new Thickness(10, 0, 0, 0);

            TextBlock mark = new TextBlock();
            mark.Text = delta;
          //  mark.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0xAA, 0x33));
            mark.FontSize = 16;
            mark.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            mark.Margin = new Thickness(1);
            mark.FontWeight = FontWeights.Bold;
            mark.Padding = new Thickness(10, 0, 0, 0);

            panel.Children.Add(s);
            panel.Children.Add(control);
            panel.Children.Add(mark);
            border.Child = panel;
            border.CornerRadius = new CornerRadius(3);
            border.BorderThickness = new Thickness(3);
            border.Margin = new Thickness(3);
            border.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0xcc, 0x33));
            Changes.Children.Add(border);
        }

        public void ShowWindow(NotificationWindow ex)
        {
            ex.Show();
        }
    }
}
