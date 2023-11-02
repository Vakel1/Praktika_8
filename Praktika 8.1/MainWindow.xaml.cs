using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Task> tasks = new ObservableCollection<Task>();

        public MainWindow()
        {
            InitializeComponent();
            taskListView.ItemsSource = tasks;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string taskText = taskTextBox.Text.Trim();
                if (!string.IsNullOrWhiteSpace(taskText))
                {
                    Task task = new Task { Title = taskText, Status = "Incomplete" };
                    tasks.Add(task);
                    taskTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Task text cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FilterComplete_Click(object sender, RoutedEventArgs e)
        {
            FilterByStatus("Complete");
        }

        private void FilterIncomplete_Click(object sender, RoutedEventArgs e)
        {
            FilterByStatus("Incomplete");
        }

        private void FilterByStatus(string status)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(taskListView.ItemsSource);

            if (view != null)
            {
                view.Filter = item =>
                {
                    if (item is Task task)
                    {
                        return task.Status.ToLower() == status.ToLower();
                    }
                    return false;
                };
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskListView.SelectedItem is Task selectedTask)
            {
                tasks.Remove(selectedTask);
            }
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskListView.SelectedItem is Task selectedTask)
            {
                selectedTask.Status = "Complete";
                ICollectionView view = CollectionViewSource.GetDefaultView(taskListView.ItemsSource);
                view.Refresh();
            }
        }

       

        private void FilterTasks(object sender, RoutedEventArgs e)
        {
            string filterText = filterTextBox.Text.ToLower();
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(taskListView.ItemsSource);

            if (view != null)
            {
                view.Filter = item =>
                {
                    if (item is Task task)
                    {
                        return task.Title.ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }


        private void ResetFilter(object sender, RoutedEventArgs e)
        {
            filterTextBox.Clear();
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(taskListView.ItemsSource);

            if (view != null)
            {
                view.Filter = null;
            }
        }

      

            
        }

        public class TaskStatusComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x is Task task1 && y is Task task2)
                {
                    return string.Compare(task1.Status, task2.Status);
                }
                return 0;
            }
        }

    }

    public class Task
    {
        public string Title { get; set; }
        public string Status { get; set; }
    }

    // Дополнительные классы для сортировки
    public class TaskTitleComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x is Task task1 && y is Task task2)
            {
                return string.Compare(task1.Title, task2.Title, StringComparison.Ordinal);
            }
            return 0;
        }
    }

    public class TaskStatusComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x is Task task1 && y is Task task2)
            {
                if (task1.Status == "Complete" && task2.Status != "Complete")
                    return 1;
                if (task1.Status != "Complete" && task2.Status == "Complete")
                    return -1;
                return 0;
            }
            return 0;
        }
    }


