using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDo_with_CSHARP_v2
{
    public partial class Form1 : Form
    {
        private TaskRepository _repo;
        private List<TaskItem> _allTasks;

        public Form1()
        {
            InitializeComponent();

            _repo = new TaskRepository();

            txtSearch.TextChanged += (s, e) => FilterTasks();

            LoadTasks();
        }
            private void btnRefresh_Click(object sender, EventArgs e)
            {
                LoadTasks();
            }

        private void LoadTasks()
        {
            try
            {
                _allTasks = _repo.GetAllTasks();
                BindData(_allTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
            }
        }

        private void FilterTasks()
        {
            string keyword = txtSearch.Text;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                BindData(_allTasks);
            }
            else
            {
                var filtered = _repo.SearchTasks(keyword);
                BindData(filtered);
            }
        }

        private void BindData(List<TaskItem> tasks)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = tasks;

            if (dataGridView1.Columns["Id"] != null) dataGridView1.Columns["Id"].Visible = false;
            if (dataGridView1.Columns["CreatedAt"] != null) dataGridView1.Columns["CreatedAt"].Visible = false;
            if (dataGridView1.Columns["UpdatedAt"] != null) dataGridView1.Columns["UpdatedAt"].Visible = false;

            ApplyColors();
        }

        private void ApplyColors()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                var task = row.DataBoundItem as TaskItem;
                if (task == null) continue;

                if (task.StatusName == "Готово")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                }

                if (task.Priority == 3)
                {
                    row.Cells["Title"].Style.ForeColor = Color.Red;
                    row.Cells["Title"].Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция добавления будет реализована через форму!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            if (MessageBox.Show("Удалить задачу?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var task = dataGridView1.CurrentRow.DataBoundItem as TaskItem;
                _repo.DeleteTask(task.Id);
                LoadTasks();
            }
        }
    }
}