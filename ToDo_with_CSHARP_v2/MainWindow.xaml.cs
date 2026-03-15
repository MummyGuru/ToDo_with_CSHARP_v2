using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterTasks()
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                dgTasks.ItemsSource = _allTasks;
            }
            else
            {
                var filtered = _repo.SearchTasks(keyword);
                dgTasks.ItemsSource = filtered;
            }
        }

        // Событие поиска
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterTasks();
        }

        // Кнопка Обновить
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
            txtSearch.Clear();
        }

        // Кнопка Удалить
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is TaskItem selectedTask)
            {
                var result = MessageBox.Show($"Удалить задачу \"{selectedTask.Title}\"?", "Подтверждение",
                                             MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _repo.DeleteTask(selectedTask.Id);
                    LoadTasks();
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Кнопка Добавить (Заглушка для демонстрации)
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Для полноценной реализации нужно открывать второе окно (Dialog)
            // Сейчас сделаем простую демонстрацию добавления "тестовой" задачи
            var newTask = new TaskItem
            {
                Title = "Новая задача из WPF",
                Description = "Создана автоматически для теста",
                Priority = 2,
                StatusId = 1, // Новая
                CategoryId = 1,
                Deadline = DateTime.Now.AddDays(2)
            };

            try
            {
                _repo.AddTask(newTask);
                MessageBox.Show("Задача успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
            }
        }
    }
}