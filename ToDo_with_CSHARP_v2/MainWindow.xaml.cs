using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDo_with_CSHARP_v2.Data;
using ToDo_with_CSHARP_v2.Models;

namespace ToDo_with_CSHARP_v2
{
    public partial class MainWindow : Window
    {
        private readonly TaskRepository _repo;
        private List<TaskItem> _allTasks;

        public MainWindow()
        {
            InitializeComponent();
            _repo = new TaskRepository();
            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                _allTasks = _repo.GetAllTasks();
                dgTasks.ItemsSource = _allTasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка БД: {ex.Message}");
            }
        }

        private void FilterTasks()
        {
            string key = txtSearch.Text.Trim();
            dgTasks.ItemsSource = string.IsNullOrEmpty(key) ? _allTasks : _repo.SearchTasks(key);
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) => FilterTasks();

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            LoadTasks();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is TaskItem task)
            {
                if (MessageBox.Show($"Удалить задачу \"{task.Title}\"?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _repo.DeleteTask(task.Id);
                    LoadTasks();
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenTaskDialog(null);
        }

        private void DgTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgTasks.SelectedItem is TaskItem task)
            {
                OpenTaskDialog(task);
            }
        }

        private void OpenTaskDialog(TaskItem taskToEdit)
        {
            var dialog = new TaskDialogWindow(taskToEdit, _repo);
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                LoadTasks();
            }
        }
    }
}