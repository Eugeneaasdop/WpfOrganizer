using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfOrganizer.Classes;

namespace WpfOrganizer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageTaskAdd.xaml
    /// </summary>
    public partial class PageTaskAdd : Page
    {
        private Classes.Tasks _currentTasks = new Classes.Tasks();
        public PageTaskAdd(Classes.Tasks selectedTasks)
        {
            InitializeComponent();
            CMBpol.ItemsSource = OrganizerEntities.GetContext().Users.ToList();
            CMBpol.SelectedValuePath = "id_users"; 
            CMBpol.DisplayMemberPath = "first_name";
            CMBdate.ItemsSource = OrganizerEntities.GetContext().Priority.ToList();
            CMBdate.SelectedValuePath = "id_priority";
            CMBdate.DisplayMemberPath = "NamePi";
            if (selectedTasks != null)
            {
                _currentTasks = selectedTasks;
                Titletxt.Text = "Изменение задачи";
                btnAdd.Content = "Изменить";
            }
            // Создаём контекст
            DataContext = _currentTasks;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTasks.id_users))) error.AppendLine("Укажите пользователя");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTasks.Taskname))) error.AppendLine("Укажите задачу");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTasks.description))) error.AppendLine("Укажите описание");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTasks.creation_datetime))) error.AppendLine("Укажите дату создания");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTasks.due_datetime))) error.AppendLine("Укажите дату исполнения");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTasks.id_priority))) error.AppendLine("Укажите степень важности");
            if (OrganizerEntities.GetContext().Tasks.Where(x => x.Users.first_name == CMBpol.Text && x.Taskname == Txtnameev.Text && x.description == Txtdes.Text && x.creation_datetime.ToString() == Txtstart.Text && x.due_datetime.ToString() == Txtend.Text && x.Priority.NamePi == CMBdate.Text).Count() > 0)
            {
                MessageBox.Show("Такая задача уже есть");
                return;
            }
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }
            if (_currentTasks.id_task == 0)
            {
                OrganizerEntities.GetContext().Tasks.Add(_currentTasks);
                try
                {
                    OrganizerEntities.GetContext().SaveChanges();
                    ClassFrame.frmObj.Navigate(new PageTask());
                    MessageBox.Show("Новая задача успешно добавлена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                try
                {
                    OrganizerEntities.GetContext().SaveChanges();
                    ClassFrame.frmObj.Navigate(new PageTask());
                    MessageBox.Show("Задача успешно изменена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageTask());
        }
    }
}
