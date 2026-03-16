using System;
using System.Linq;
using System.Windows;
using ToDo_with_CSHARP_v2.Data;
using ToDo_with_CSHARP_v2.Models;

namespace ToDo_with_CSHARP_v2
{
	public partial class TaskDialogWindow : Window
	{
		private readonly TaskRepository _repo;
		private readonly TaskItem _task;

		public TaskDialogWindow(TaskItem taskToEdit, TaskRepository repo)
		{
			InitializeComponent();
			_repo = repo;
			_task = taskToEdit ?? new TaskItem();

			LoadCombos();
			FillFields();

			chkNoDeadline.Checked += (s, e) => dpDeadline.IsEnabled = false;
			chkNoDeadline.Unchecked += (s, e) => dpDeadline.IsEnabled = true;
		}

		private void LoadCombos()
		{
			cmbPriority.ItemsSource = new[]
			{
				new { Id = 1, Name = "1 - Низкий" },
				new { Id = 2, Name = "2 - Средний" },
				new { Id = 3, Name = "3 - Высокий" }
			};
			cmbPriority.DisplayMemberPath = "Name";
			cmbPriority.SelectedValuePath = "Id";

			var categories = _repo.GetCategories();
			categories.Insert(0, new Category { Id = 0, Name = "-- Без категории --" });
			cmbCategory.ItemsSource = categories;
			cmbCategory.DisplayMemberPath = "Name";
			cmbCategory.SelectedValuePath = "Id";

			cmbStatus.ItemsSource = _repo.GetStatuses();
			cmbStatus.DisplayMemberPath = "Name";
			cmbStatus.SelectedValuePath = "Id";
		}

		private void FillFields()
		{
			txtTitle.Text = _task.Title;
			txtDescription.Text = _task.Description;

			cmbPriority.SelectedValue = _task.Priority;

			if (_task.CategoryId.HasValue)
				cmbCategory.SelectedValue = _task.CategoryId.Value;
			else
				cmbCategory.SelectedIndex = 0;

			cmbStatus.SelectedValue = _task.StatusId;

			if (_task.Deadline.HasValue)
			{
				dpDeadline.SelectedDate = _task.Deadline.Value;
				chkNoDeadline.IsChecked = false;
				dpDeadline.IsEnabled = true;
			}
			else
			{
				chkNoDeadline.IsChecked = true;
				dpDeadline.IsEnabled = false;
			}

			Title = _task.Id > 0 ? "Редактирование задачи" : "Новая задача";
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTitle.Text))
			{
				MessageBox.Show("Введите название задачи!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			_task.Title = txtTitle.Text.Trim();
			_task.Description = txtDescription.Text.Trim();
			_task.Priority = (int)cmbPriority.SelectedValue;
			_task.StatusId = (int)cmbStatus.SelectedValue;

			int catId = (int)cmbCategory.SelectedValue;
			_task.CategoryId = (catId == 0) ? (int?)null : catId;

			if (chkNoDeadline.IsChecked == true)
			{
				_task.Deadline = null;
			}
			else
			{
				if (dpDeadline.SelectedDate.HasValue)
					_task.Deadline = dpDeadline.SelectedDate.Value;
			}

			try
			{
				if (_task.Id > 0)
				{
					_repo.UpdateTask(_task);
					MessageBox.Show("Задача успешно обновлена!", "Успех");
				}
				else
				{
					_repo.AddTask(_task);
					MessageBox.Show("Задача создана!", "Успех");
				}

				DialogResult = true;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}