using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace TaskTracker
{
    public partial class MainWindow : Window
    {
        private List<TaskItem> tasks = new List<TaskItem>();
        private int taskCounter = 1;

        public MainWindow()
        {
            InitializeComponent();
            LoadTasks(); // Загружаем задачи при запуске приложения
        }

        private void LoadTasks()
        {
            if (File.Exists("tasks.txt"))
            {
                using (StreamReader reader = new StreamReader("tasks.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 3 && int.TryParse(parts[0], out int number) && bool.TryParse(parts[2], out bool isCompleted))
                        {
                            tasks.Add(new TaskItem { Number = number, Description = parts[1], IsCompleted = isCompleted });
                            if (number >= taskCounter) // Обновляем счетчик задач
                            {
                                taskCounter = number + 1;
                            }
                        }
                    }
                }
                UpdateTaskList(); // Обновляем список задач после загрузки
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                tasks.Add(new TaskItem { Description = TaskInput.Text, IsCompleted = false, Number = taskCounter++ });
                TaskInput.Clear();
                UpdateTaskList();
            }
        }

        private void UpdateTaskList()
        {
            TaskList.Items.Clear();
            foreach (var task in tasks)
            {
                TaskList.Items.Add(task);
            }
        }

        private void TaskList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Здесь можно оставить как есть: выделение элемента
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is TaskItem selectedTask)
            {
                tasks.Remove(selectedTask); // Удаляем выбранную задачу
                UpdateTaskList(); // Обновляем список задач
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для удаления."); // Сообщение, если ничего не выбрано
            }
        }

        private void SaveTasks_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("tasks.txt"))
            {
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Number}|{task.Description}|{task.IsCompleted}");
                }
            }
            MessageBox.Show("Задачи сохранены в файл tasks.txt");
        }
    }

    public class TaskItem
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{Number}. {Description} - {(IsCompleted ? "Выполнено" : "Не выполнено")}";
        }
    }
}