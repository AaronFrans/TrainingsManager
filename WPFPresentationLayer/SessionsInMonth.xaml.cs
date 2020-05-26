using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFPresentationLayer
{
    /// <summary>
    /// Interaction logic for SessionsInMonth.xaml
    /// </summary>
    public partial class SessionsInMonth : Window
    {
        TrainingManager tm;
        int selectedBoxes;
        public SessionsInMonth(TrainingManager tm, int selectedBoxes)
        {
            InitializeComponent();
            this.tm = tm;
            bestRunningSessions.Visibility = Visibility.Collapsed;
            bestCyclingSessions.Visibility = Visibility.Collapsed;
            sessionsPanel.Visibility = Visibility.Collapsed;
            this.selectedBoxes = selectedBoxes;
        }

        private void inputButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                int year = 0;
                if (string.IsNullOrWhiteSpace(yearInput.Text))
                {
                    throw new ArgumentException("Year is not a whole number.");
                }
                if (!int.TryParse(yearInput.Text, out int yearParsed))
                {
                    throw new ArgumentException("Year is not a whole number.");
                }
                else
                {
                    year = yearParsed;
                }
                int month = 0;
                if (string.IsNullOrWhiteSpace(monthInput.Text))
                {
                    throw new ArgumentException("Month is not a whole number.");
                }
                if (!int.TryParse(monthInput.Text, out int monthParsed))
                {
                    throw new ArgumentException("Month is not a whole number.");
                }
                else
                {
                    if (!(monthParsed <= 12))
                        throw new ArgumentException("Month is bigger than 12.");
                    month = monthParsed;
                }
                Report r = null;
                if (selectedBoxes == -1)
                {
                    r = tm.GenerateMonthlyTrainingsReport(year, month);
                    if (r.Runs.Count == 0)
                    {
                        throw new Exception("There are no run session for the given month and year.");
                    }
                    if (r.Rides.Count == 0)
                    {
                        throw new Exception("There are no cycle session for the given month and year.");
                    }
                }
                else if (selectedBoxes == 0)
                {
                    r = tm.GenerateMonthlyRunningReport(year, month);
                    if (r.Runs.Count == 0)
                    {
                        throw new Exception("There are no run session for the given month and year.");
                    }
                }
                else if (selectedBoxes == 1)
                {
                    r = tm.GenerateMonthlyCyclingReport(year, month);
                    if (r.Rides.Count == 0)
                    {
                        throw new Exception("There are no cycle session for the given month and year.");
                    }
                }

                sessionsHeader.Text += $@" {month}/{year}";
                inputPanel.Visibility = Visibility.Collapsed;
                MaxHeight = 450;
                MaxWidth = 550;
                this.Height = 450;
                this.Width = 600;
                if (selectedBoxes == -1 || selectedBoxes == 0)
                    bestRunningSessions.Visibility = Visibility.Visible;
                if (selectedBoxes == -1 || selectedBoxes == 1)
                    bestCyclingSessions.Visibility = Visibility.Visible;
                sessionsPanel.Visibility = Visibility.Visible;
                MinHeight = 450;
                MinWidth = 600;

                SetSessions(r);

            }
            catch (ArgumentException aex)
            {
                ShowErrorMessage(aex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void SetSessions(Report r)
        {
            if (selectedBoxes == -1 || selectedBoxes == 0)
            {
                besRunDist.Text = r.MaxDistanceSessionRunning.ToString();
                bestRunSpeed.Text = r.MaxSpeedSessionRunning.ToString();
            }
            if (selectedBoxes == -1 || selectedBoxes == 1)
            {
                besCycleDist.Text = r.MaxDistanceSessionCycling.ToString();
                bestCycleSpeed.Text = r.MaxSpeedSessionCycling.ToString();
                bestCycleWatt.Text = r.MaxWattSessionCycling.ToString();
            }

            

            foreach (var pair in r.TimeLine)
            {
                sessions.Text += pair.Item2.ToString() + "\n";
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

    }
}
