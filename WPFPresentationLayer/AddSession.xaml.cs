using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
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

namespace WPFPresentationLayer
{
    public partial class AddSession : Window
    {
        private TrainingManager tm;
        private int sessionType;
        public AddSession(TrainingManager tm, int sessionType)
        {
            InitializeComponent();
            this.tm = tm;
            this.sessionType = sessionType;
            setupPanels();
        }

        private void addToDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = GetDate();
                float? distance = GetDistance();
                TimeSpan time = GetTime();
                int? averageWatt = null;
                if (sessionType == 1)
                    averageWatt = GetWatt();
                TrainingType trainType = GetTrainingType();
                float? averageSpeed = GetAverageSpeed();
                string comments = GetComments();
                BikeType bikeType = GetBikeType();

                if (sessionType == 0)
                {
                    Task.Run(() => tm.AddRunningTraining(date, (int)distance, time, averageSpeed, trainType, comments));
                    MessageBox.Show("Added a running session to the database");
                }
                else if (sessionType == 1)
                {
                    Task.Run(() => tm.AddCyclingTraining(date, distance, time, averageSpeed, averageWatt, trainType, comments, bikeType));
                    MessageBox.Show("Added a cycling session to the database");
                }

                if (finishedInput.IsChecked == true)
                    this.Close();

            }
            catch (DomainException dex)
            {
                ShowErrorMessage(dex.Message);
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

        private void setupPanels()
        {
            if (sessionType == 0)
            {
                bikeTypePanel.Visibility = Visibility.Collapsed;
                aveWattPanel.Visibility = Visibility.Collapsed;
            }
            if (sessionType == 1)
            {
                distanceLabel.Content = distanceLabel.Content.ToString().Replace("*", "");
            }
        }


        private DateTime GetDate()
        {
            int day = 0, month = 0, year = 0;
            int hours = 0, minutes = 0, seconds = 0;

            string[] dateAndTime = dateInput.Text.Split(' ');
            if (dateAndTime.Length != 2)
                throw new ArgumentException("Please use the format day/month/year hours:minutes:seconds\n when filling in the date");
            string[] date = dateAndTime[0].Split('/');
            string[] time = dateAndTime[1].Split(':');

            if (!int.TryParse(date[0], out day))
                throw new ArgumentException("Please use the format day/month/year hours:minutes:seconds\n when filling in the date");
            if (!int.TryParse(date[1], out month))
                throw new ArgumentException("Please use the format day/month/year hours:minutes:seconds\n when filling in the date");
            if (!int.TryParse(date[2], out year))
                throw new ArgumentException("Please use the format day/month/year hours:minutes:seconds\n when filling in the date");


            if (!int.TryParse(time[0], out hours))
                throw new ArgumentException("Please use the format day/month/year hours:minutes:seconds\n when filling in the date");
            if (!int.TryParse(time[1], out minutes))
                throw new ArgumentException("Please use the format day/month/year hours:minutes:seconds\n when filling in the date");
            if (!int.TryParse(time[2], out seconds))
                throw new ArgumentException("Please use the format day/month/year hours:minutes:seconds\n when filling in the date");

            return new DateTime(year, month, day, hours, minutes, seconds);

        }
        private float? GetDistance()
        {
            float? distance = null;
            if (sessionType == 0)
            {
                if (string.IsNullOrWhiteSpace(distanceInput.Text))
                {
                    throw new ArgumentException("Distance is not a whole number.");
                }
                if (!int.TryParse(distanceInput.Text, out int dist))
                {
                    throw new ArgumentException("Distance is not a whole number." +
                        " (the distance for a running session is in meters)");
                }
                else
                {
                    distance = dist;
                }
            }
            else if (sessionType == 1)
            {
                if (!string.IsNullOrWhiteSpace(distanceInput.Text))
                {
                    if (!float.TryParse(distanceInput.Text, out float dist))
                    {

                        throw (new ArgumentException("Distance is not a number or empty"));

                    }
                    else
                    {
                        distance = dist;
                    }

                }

            }
            return distance;
        }
        private TimeSpan GetTime()
        {
            int hours = 0, minutes = 0, seconds = 0;
            string[] time = timeInput.Text.Split(':');
            if (!int.TryParse(time[0], out hours))
                throw new ArgumentException("Please use the format hours:minutes:seconds\n when filling in the time");
            if (!int.TryParse(time[1], out minutes))
                throw new ArgumentException("Please use the format hours:minutes:seconds\n when filling in the time");
            if (!int.TryParse(time[2], out seconds))
                throw new ArgumentException("Please use the format hours:minutes:seconds\n when filling in the time");
            return new TimeSpan(hours, minutes, seconds);
        }
        public float? GetAverageSpeed()
        {
            float? averageSpeed = null;
            if (!string.IsNullOrWhiteSpace(aveSpeedInput.Text))
            {
                if (!float.TryParse(aveSpeedInput.Text, out float aveSpeed))
                {
                    throw (new ArgumentException("Speed is not a number or empty"));

                }
                else
                {
                    averageSpeed = aveSpeed;
                }

            }
            return averageSpeed;
        }
        private int? GetWatt()
        {
            int? averageWatt = null;
            if (!string.IsNullOrWhiteSpace(aveWattInput.Text))
            {

                if (!int.TryParse(aveWattInput.Text, out int aveWatt))
                {
                    throw (new ArgumentException("Watt is not a whole number or empty"));

                }
                else
                {
                    averageWatt = aveWatt;
                }
            }
            return averageWatt;
        }
        private TrainingType GetTrainingType()
        {
            TrainingType trainingType = TrainingType.Endurance;
            switch (trainTypeInput.SelectedItem.ToString())
            {
                case "Endurance":
                    trainingType = TrainingType.Endurance;
                    break;
                case "Interval":
                    trainingType = TrainingType.Interval;
                    break;
                case "Recuperation":
                    trainingType = TrainingType.Recuperation;
                    break;
            }
            return (TrainingType)trainingType;
        }
        private string GetComments()
        {
            string comments = null;
            if (!commentInput.Text.ToString().Equals("Comments"))
                comments = commentInput.Text.ToString();
            return comments;
        }
        private BikeType GetBikeType()
        {
            BikeType trainingType = BikeType.CityBike;
            switch (bikeTypeInput.SelectedItem.ToString())
            {
                case "Indoor Bike":
                    trainingType = BikeType.IndoorBike;
                    break;
                case "Racing Bike":
                    trainingType = BikeType.RacingBike;
                    break;
                case "City Bike":
                    trainingType = BikeType.CityBike;
                    break;
                case "Mountain Bike":
                    trainingType = BikeType.MountainBike;
                    break;

            }
            return (BikeType)trainingType;
        }


        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
