using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace TaskTracker
{
    public partial class MainWindow : Window
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                var task = new TaskItem { Id = tasks.Count + 1, Description = TaskInput.Text, IsCompleted = false };
                tasks.Add(task);
                TaskList.Items.Add(task);
                TaskInput.Clear();
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is TaskItem selectedTask)
            {
                tasks.Remove(selectedTask);
                TaskList.Items.Remove(selectedTask);
            }
        }

        private void TaskList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TaskList.SelectedItem is TaskItem selectedTask)
            {
                selectedTask.IsCompleted = !selectedTask.IsCompleted;
                TaskList.Items.Refresh();
            }
        }

        private void SaveTasks_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("tasks.txt"))
            {
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Id};{task.Description};{task.IsCompleted}");
                }
            }
            MessageBox.Show("Задачи сохранены в файл tasks.txt");
        }

        private void LoadTasks()
        {
            if (File.Exists("tasks.txt"))
            {
                var lines = File.ReadAllLines("tasks.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 3 && int.TryParse(parts[0], out int id) && bool.TryParse(parts[2], out bool isCompleted))
                    {
                        var task = new TaskItem { Id = id, Description = parts[1], IsCompleted = isCompleted };
                        tasks.Add(task);
                        TaskList.Items.Add(task);
                    }
                }

            }
        }
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{Id}. {Description} (Выполнено: {IsCompleted})";
        }
    }
}