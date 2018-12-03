﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CsvCompare.Library;
using Microsoft.Win32;

namespace CsvCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        public bool IsClosed { get; set; }
        private ComparisonResultsWindow ComparisonWindow { get; set; }
        private DataTable File1Data { get; set; }
        private DataTable File2Data { get; set; }

        private void BrowseFile1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var fileName = GetFileName();
                if (fileName == null)
                    SetError("Error when choosing File 1. Please try again.");
                else
                    File1TextBox.Text = fileName;
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private void BrowseFile2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var fileName = GetFileName();
                if (fileName == null)
                    SetError("Error when choosing File 2. Please try again.");
                else
                    File2TextBox.Text = fileName;
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private async void Compare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                if (RowIdentifierSelectedList.Items.Count == 0)
                {
                    ErrorLabel.Content = "Please selected at least one column to use as an identifier.";
                    return;
                }

                DisableButtons();

                var rowIdentifierColumns = RowIdentifierSelectedList.Items.Cast<string>();
                var inclusionColumns = ExtraOutputSelectedList.Items.Cast<string>();
                var exclusionColumns = CompareExcludeSelectedList.Items.Cast<string>();

                var results = await GetComparisonResultsAsync(rowIdentifierColumns, inclusionColumns, exclusionColumns);

                if (ComparisonWindow == null)
                    ComparisonWindow = new ComparisonResultsWindow(results, this);
                else
                    ComparisonWindow.SetResults(results);

                ComparisonWindow.Show();

                EnableButtons();
                Hide();
            }
            catch (ArgumentException ex) when (ex.Message == "An item with the same key has already been added.")
            {
                ErrorLabel.Content = $"The identifier columns are not unique enough. Repeats founds between rows.";
                EnableButtons();
            }
            catch (Exception ex)
            {
                ErrorLabel.Content = $"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}";
                EnableButtons();
            }
        }

        private void AddRowIdentifierButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var items = RowIdentifiertOptionsList.SelectedItems.Cast<object>().ToList();
                foreach (var item in items)
                {
                    RowIdentifierSelectedList.Items.Add(item);
                    RowIdentifiertOptionsList.Items.Remove(item);
                    ExtraOutputOptionsList.Items.Remove(item);
                    CompareExcludeOptionsList.Items.Remove(item);
                }

                RowIdentifierSelectedList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private void RemoveRowIdentifierButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var items = RowIdentifierSelectedList.SelectedItems.Cast<object>().ToList();
                foreach (var item in items)
                {
                    RowIdentifiertOptionsList.Items.Add(item);
                    CompareExcludeOptionsList.Items.Add(item);
                    ExtraOutputOptionsList.Items.Add(item);
                    RowIdentifierSelectedList.Items.Remove(item);
                }

                RowIdentifiertOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
                ExtraOutputOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
                CompareExcludeOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private void AddExtraOutputButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var items = ExtraOutputOptionsList.SelectedItems.Cast<object>().ToList();
                foreach (var item in items)
                {
                    ExtraOutputSelectedList.Items.Add(item);
                    ExtraOutputOptionsList.Items.Remove(item);
                    CompareExcludeOptionsList.Items.Remove(item);
                }

                ExtraOutputSelectedList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private void RemoveExtraOutputButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var items = ExtraOutputSelectedList.SelectedItems.Cast<object>().ToList();
                foreach (var item in items)
                {
                    CompareExcludeOptionsList.Items.Add(item);
                    ExtraOutputOptionsList.Items.Add(item);
                    ExtraOutputSelectedList.Items.Remove(item);
                }

                ExtraOutputOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
                CompareExcludeOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private void AddCompareExcludeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var items = CompareExcludeOptionsList.SelectedItems.Cast<object>().ToList();
                foreach (var item in items)
                {
                    CompareExcludeSelectedList.Items.Add(item);
                    CompareExcludeOptionsList.Items.Remove(item);
                    ExtraOutputOptionsList.Items.Remove(item);
                }

                CompareExcludeSelectedList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private void RemoveCompareExcludeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearErrors();

                var items = CompareExcludeSelectedList.SelectedItems.Cast<object>().ToList();
                foreach (var item in items)
                {
                    ExtraOutputOptionsList.Items.Add(item);
                    CompareExcludeOptionsList.Items.Add(item);
                    CompareExcludeSelectedList.Items.Remove(item);
                }

                ExtraOutputOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
                CompareExcludeOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private async void File1TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                ClearErrors();
                DisableButtons();

                File1Data = await CsvReader.ReadFileToDataTableAsync(File1TextBox.Text);

                if (File2Data != null)
                    SetOptions();

                EnableButtons();
            }
            catch (Exception ex)
            {
                File1Data = null;
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private async void File2TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                ClearErrors();
                DisableButtons();

                File2Data = await CsvReader.ReadFileToDataTableAsync(File2TextBox.Text);

                if (File1Data != null)
                    SetOptions();

                EnableButtons();
            }
            catch (Exception ex)
            {
                File2Data = null;
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                ClearErrors();

                IsClosed = true;
                if (!ComparisonWindow?.IsClosed ?? false)
                    ComparisonWindow.Close();
            }
            catch (Exception ex)
            {
                SetError($"Error: {ex.Message}{Environment.NewLine} Stack Trace: {ex.StackTrace}");
            }
        }

        private string GetFileName()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "CSV Files |*.csv"
            };

            return dialog.ShowDialog(this) == true
                ? dialog.FileName
                : null;
        }

        private void SetOptions()
        {
            var columns1 = new List<string>();
            var columns2 = new List<string>();
            var commonColumns = new List<string>();

            foreach (DataColumn column in File1Data.Columns)
                columns1.Add(column.ColumnName);
            foreach (DataColumn column in File2Data.Columns)
                columns2.Add(column.ColumnName);

            commonColumns = columns1.Intersect(columns2, StringComparer.OrdinalIgnoreCase).ToList();

            if (commonColumns.Count == 0)
            {
                SetError($"There were no common columns found between the two files. There is nothing to compare.");
                return;
            }

            //Preset first column as ID
            var firstColumnName = commonColumns[0];
            commonColumns.RemoveAt(0);

            RowIdentifiertOptionsList.Items.Clear();
            RowIdentifierSelectedList.Items.Clear();
            ExtraOutputOptionsList.Items.Clear();
            ExtraOutputSelectedList.Items.Clear();
            CompareExcludeOptionsList.Items.Clear();
            CompareExcludeSelectedList.Items.Clear();

            RowIdentifierSelectedList.Items.Add(firstColumnName);

            foreach (var column in commonColumns)
            {
                RowIdentifiertOptionsList.Items.Add(column);
                ExtraOutputOptionsList.Items.Add(column);
                CompareExcludeOptionsList.Items.Add(column);
            }

            RowIdentifiertOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            ExtraOutputOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            CompareExcludeOptionsList.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));

            CompareButton.Visibility = Visibility.Visible;
            OptionsGrid.Visibility = Visibility.Visible;
        }

        private Task<ComparisonResults> GetComparisonResultsAsync(IEnumerable<string> identifierColumns, IEnumerable<string> inclusionColumns, IEnumerable<string> exclusionColumns)
        {
            return new CsvComparer(File1Data, File2Data, identifierColumns.ToList(), exclusionColumns.ToList(), inclusionColumns.ToList()).CompareAsync();
        }

        private void EnableButtons()
        {
            CompareButton.IsEnabled = true;
            AddRowIdentifierButton.IsEnabled = true;
            RemoveRowIdentifierButton.IsEnabled = true;
            AddCompareExcludeButton.IsEnabled = true;
            RemoveCompareExcludeButton.IsEnabled = true;
            AddExtraOutputButton.IsEnabled = true;
            RemoveExtraOutputButton.IsEnabled = true;
            BrowseFile1Button.IsEnabled = true;
            BrowseFile2Button.IsEnabled = true;
        }

        private void DisableButtons()
        {
            CompareButton.IsEnabled = false;
            AddRowIdentifierButton.IsEnabled = false;
            RemoveRowIdentifierButton.IsEnabled = false;
            AddCompareExcludeButton.IsEnabled = false;
            RemoveCompareExcludeButton.IsEnabled = false;
            AddExtraOutputButton.IsEnabled = false;
            RemoveExtraOutputButton.IsEnabled = false;
            BrowseFile1Button.IsEnabled = false;
            BrowseFile2Button.IsEnabled = false;
        }

        private void ClearErrors()
        {
            ErrorLabel.Content = "";
        }

        private void SetError(string error)
        {
            EnableButtons();
            OptionsGrid.Visibility = Visibility.Collapsed;
            CompareButton.Visibility = Visibility.Collapsed;
            ErrorLabel.Content = error;
        }
    }
}
