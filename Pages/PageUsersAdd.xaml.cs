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
using WpfOrganizer.Classes;

namespace WpfOrganizer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUsersAdd.xaml
    /// </summary>
    public partial class PageUsersAdd : Page
    {
        private Users _currentTUsers = new Users();
        public PageUsersAdd(Users selectedUsers)
        {
            InitializeComponent();
            CMBcat.ItemsSource = OrganizerEntities.GetContext().Categories.ToList();
            CMBcat.SelectedValuePath = "id_category";
            CMBcat.DisplayMemberPath = "NameCate";
            if (selectedUsers != null)
            {
                _currentTUsers = selectedUsers;
                Titletxt.Text = "Изменение пользователя";
                btnAdd.Content = "Изменить";
            }
            // Создаём контекст
            DataContext = _currentTUsers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTUsers.first_name))) error.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTUsers.last_name))) error.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTUsers.email))) error.AppendLine("Укажите почту");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTUsers.password))) error.AppendLine("Укажите пароль");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentTUsers.id_category))) error.AppendLine("Укажите категорию");
            if (OrganizerEntities.GetContext().Users.Where(x => x.first_name == Txtname.Text && x.last_name == Txtlast.Text && x.email == Txtemail.Text && x.password == Txtpass.Text && x.Categories.id_category.ToString() == CMBcat.Text).Count() > 0)
            {
                MessageBox.Show("Такой пользователь уже есть");
                return;
            }
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }
            if (_currentTUsers.id_users == 0)
            {
                OrganizerEntities.GetContext().Users.Add(_currentTUsers);
                try
                {
                    OrganizerEntities.GetContext().SaveChanges();
                    ClassFrame.frmObj.Navigate(new PageUsers());
                    MessageBox.Show("Новый пользователь успешно добавлен!");
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
                    ClassFrame.frmObj.Navigate(new PageUsers());
                    MessageBox.Show("Пользователь успешно изменена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageUsers());
        }
    }
}
