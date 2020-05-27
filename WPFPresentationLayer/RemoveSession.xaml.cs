using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RemoveWindow : Window
    {
        TrainingManager tm;
        List<int> runIds;
        List<int> cycleIds;
        public RemoveWindow(TrainingManager tm)
        {
            InitializeComponent();
            this.tm = tm;
            runIds = new List<int>();
            cycleIds = new List<int>();
        }

        private void removeIdButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = GetId();
                if ((bool)rusSessSelect.IsChecked == true)
                {
                    if (!runIds.Contains(id))
                        throw new ArgumentException("Run id is not added.");
                    runIds.Remove(id);
                    int startIndex = selectedIds.Text.IndexOf($"Running session (id): {id}\n");
                    int count = $"Running session(id): {id}/n".Length;
                    selectedIds.Text = selectedIds.Text.Remove(startIndex, count);
                }
                else if ((bool)cycleSessSelect.IsChecked == true)
                {
                    if (!cycleIds.Contains(id))
                        throw new ArgumentException("Cycle id is not added.");
                    cycleIds.Remove(id);
                    int startIndex = selectedIds.Text.IndexOf($"Cycling session (id): {id}\n");
                    int count = $"Cycling session(id): {id}/n".Length;
                    selectedIds.Text = selectedIds.Text.Remove(startIndex, count);
                }
            }
            catch (Exception ex)
            {

                ShowErrorMessage(ex.Message);
            }
        }
        private void addIDButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = GetId();
                if ((bool)rusSessSelect.IsChecked == true)
                {
                    if (runIds.Contains(id))
                        throw new ArgumentException("Run id is already added.");
                    runIds.Add(id);
                    selectedIds.Text += $"Running session (id): {id}\n";
                }
                else if ((bool)cycleSessSelect.IsChecked == true)
                {
                    if (cycleIds.Contains(id))
                        throw new ArgumentException("Cycle id is already added.");
                    cycleIds.Add(id);
                    selectedIds.Text += $"Cycling session (id): {id}\n";
                }
            }
            catch (Exception ex)
            {

                ShowErrorMessage(ex.Message);
            }

        }
        private int GetId()
        {
            int result = -1;

            if (string.IsNullOrWhiteSpace(idNumberInput.Text))
            {
                throw new ArgumentException("Id can not be empty");
            }
            if (!int.TryParse(idNumberInput.Text, out int id))
            {
                throw (new ArgumentException("Id must be a whole number"));

            }
            else
            {
                if (id > 0)
                    result = id;
                else
                    throw (new ArgumentException("Id can not be smaller than one"));
            }

            return result;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void deleteSessions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (confirmationCheck.IsChecked == false)
                {
                    throw new ArgumentException("Please confirm that you want to delete these sesssions");
                }

                tm.RemoveTrainings(cycleIds, runIds);
                MessageBox.Show("Removed the selected sessions");
                this.Close();
            }
            catch (Exception ex)
            {

                ShowErrorMessage(ex.Message);
            }
        }
    }
}

