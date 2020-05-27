using DataLayer;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFPresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private TrainingManager tm;



        public MainWindow()
        {
            InitializeComponent();
            tm = new TrainingManager(new UnitOfWork(new TrainingContext(null)));
        }

        private void AddSess_Click(object sender, RoutedEventArgs e)
        {

            Window addSess = null;
            if (runningSessionsRadio.IsChecked == true)
            {
                addSess = new AddSession(tm, 0);
            }
            else if (cyclingSessionsRadio.IsChecked == true)
            {
                addSess = new AddSession(tm, 1);
            }

            addSess.Show();
        }

        private void showGivenMonth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (runningSessionsCheck.IsChecked == false & cyclingSessionsCheck.IsChecked == false)
                {
                    throw new Exception("Please select at least one of the sessions");
                }
                else if (runningSessionsCheck.IsChecked == true & cyclingSessionsCheck.IsChecked == true)
                {
                    Window monthSessions = new SessionsInMonth(tm, -1);
                    monthSessions.Show();
                }
                else if (runningSessionsCheck.IsChecked == true)
                {
                    Window monthSessions = new SessionsInMonth(tm, 0);
                    monthSessions.Show();
                }
                else if (cyclingSessionsCheck.IsChecked == true)
                {
                    Window monthSessions = new SessionsInMonth(tm, 1);
                    monthSessions.Show();
                }

            }
            catch (Exception ex)
            {

                ShowErrorMessage(ex.Message);
            }
        }

        private void showRecent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (runningSessionsCheck.IsChecked == false & cyclingSessionsCheck.IsChecked == false)
                {
                    throw new Exception("Please select one of the sessions");
                }
                else if (runningSessionsCheck.IsChecked == true & cyclingSessionsCheck.IsChecked == true)
                {
                    throw new Exception("Please select only one of the sessions");
                }
                else if (runningSessionsCheck.IsChecked == true)
                {
                    Window recentSessions = new LastSessions(tm, 0);
                    recentSessions.Show();
                }
                else if (cyclingSessionsCheck.IsChecked == true)
                {
                    Window testWindow = new LastSessions(tm, 1);
                    testWindow.Show();
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void deleteSession_Click(object sender, RoutedEventArgs e)
        {
            Window deleteWindow = new RemoveWindow(tm);
            deleteWindow.Show();
        }
    }
}
