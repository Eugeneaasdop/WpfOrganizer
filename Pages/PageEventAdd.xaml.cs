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
    /// Логика взаимодействия для PageEventAdd.xaml
    /// </summary>
    public partial class PageEventAdd : Page
    {
        private Events _currentEvents = new Events();
        public PageEventAdd(Events selectedevents)
        {
            InitializeComponent();
            CMBpol.ItemsSource = OrganizerEntities.GetContext().Users.ToList();
            CMBpol.SelectedValuePath = "id_users";
            CMBpol.DisplayMemberPath = "first_name";

            if (selectedevents != null)
            {
                _currentEvents = selectedevents;
                Titletxt.Text = "Изменение задачи";
                btnAdd.Content = "Изменить";
            }
            // Создаём контекст
            DataContext = _currentEvents;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentEvents.id_users))) error.AppendLine("Укажите пользователя");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentEvents.event_name))) error.AppendLine("Укажите название задачи");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentEvents.description))) error.AppendLine("Напишите описание");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentEvents.start_datetime))) error.AppendLine("Укажите начальную дату");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentEvents.end_datetime))) error.AppendLine("Укажите конечную дату");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentEvents.location))) error.AppendLine("Укажите локацию");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentEvents.reminder_datetime))) error.AppendLine("Укажите время напоминания");
            if (OrganizerEntities.GetContext().Events.Where(x => x.Users.first_name == CMBpol.Text && x.event_name == Txtnameev.Text && x.description == Txtdes.Text && x.start_datetime.ToString() == Txtstart.Text && x.end_datetime.ToString() == Txtend.Text && x.location == Txtlocal.Text && x.reminder_datetime.ToString() == Txtdate.Text).Count() > 0)
            {
                MessageBox.Show("Такая задача уже есть");
                return;
            }
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }
            if (_currentEvents.id_event == 0)
            {
                OrganizerEntities.GetContext().Events.Add(_currentEvents);
                try
                {
                    OrganizerEntities.GetContext().SaveChanges();
                    ClassFrame.frmObj.Navigate(new PageEvent());
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
                    ClassFrame.frmObj.Navigate(new PageEvent());
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
            ClassFrame.frmObj.Navigate(new PageEvent());
        }
    }
}
