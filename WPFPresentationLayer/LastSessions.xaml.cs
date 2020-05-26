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
    /// Interaction logic for LastSessions.xaml
    /// </summary>
    public partial class LastSessions : Window
    {
        TrainingManager tm;
        int selectedSession;
        public LastSessions(TrainingManager tm, int selectedSession)
        {
            InitializeComponent();
            this.tm = tm;
            this.selectedSession = selectedSession;
        }

        private void inputButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nrOfSessions = 0;
                if (string.IsNullOrWhiteSpace(prevSessInput.Text))
                {
                    throw new ArgumentException("Nr of sessions can not be empty.");
                }
                if (!int.TryParse(prevSessInput.Text, out int nrofSessionsParsed))
                {
                    throw new ArgumentException("Nr of sessions is not a whole number.");
                }
                else
                {
                    if (nrofSessionsParsed > 0)
                        nrOfSessions = nrofSessionsParsed;
                    else
                        throw new ArgumentException("Nr of sessions must be bigger than 0.");
                }

                if (selectedSession == 0)
                {

                    List<RunningSession> sessions = tm.GetPreviousRunningSessions(nrOfSessions);
                    inputPanel.Visibility = Visibility.Collapsed;
                    sessionsPanel.Visibility = Visibility.Visible;
                    MaxHeight = 370;
                    MaxWidth = 400;
                    Height = 370;
                    Width = 400;
                    MinHeight = 370;
                    MinWidth = 400;
                    SetSessions(sessions);

                }
                else if (selectedSession == 1)
                {
                    List<RunningSession> sessions = tm.GetPreviousRunningSessions(nrOfSessions);
                    inputPanel.Visibility = Visibility.Collapsed;
                    sessionsPanel.Visibility = Visibility.Visible;
                    MaxHeight = 370;
                    MaxWidth = 400;
                    Height = 370;
                    Width = 400;
                    MinHeight = 370;
                    MinWidth = 400;
                    SetSessions(tm.GetPreviousCyclingSessions(nrOfSessions));
                }

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

        private void SetSessions(List<RunningSession> runSess)
        {
            sessionsHeader.Text.Replace("x", runSess.Count.ToString());


            foreach (var session in runSess)
            {
                sessions.Text += session.ToString() + "\n";
            }
        }
        private void SetSessions(List<CyclingSession> cycleSess)
        {
            sessionsHeader.Text.Replace("x", cycleSess.Count.ToString());

            foreach (var session in cycleSess)
            {
                sessions.Text += session.ToString() + "\n";
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
