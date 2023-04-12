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
    /// Логика взаимодействия для PageUsers.xaml
    /// </summary>
    public partial class PageUsers : Page
    {
        public PageUsers()
        {
            InitializeComponent();
            DtgSQL.ItemsSource = OrganizerEntities.GetContext().Users.ToList();
            var listcategor = OrganizerEntities.GetContext().Categories.Select(x => x.NameCate).Distinct().ToList();
            CMBFilterForm.Items.Add("Все категории");
            foreach (string categor in listcategor)
            {
                CMBFilterForm.Items.Add(categor);
            }
            System.Int32 customerCount = OrganizerEntities.GetContext().Users.Count();
            Txtname.Text = customerCount.ToString();
        }

        private void PGlav_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageGalvmenu());
        }

        private void Delette_Click(object sender, RoutedEventArgs e)
        {
            var AftoForRemoving = DtgSQL.SelectedItems.Cast<Users>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {AftoForRemoving.Count()} записи?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    OrganizerEntities.GetContext().Users.RemoveRange(AftoForRemoving);
                    OrganizerEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    DtgSQL.ItemsSource = OrganizerEntities.GetContext().Users.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BTNedit_Click(object sender, RoutedEventArgs e)
        {
           ClassFrame.frmObj.Navigate(new PageUsersAdd((Users)DtgSQL.SelectedItem));
        }

        private void Btnadd_Click(object sender, RoutedEventArgs e)
        {
           ClassFrame.frmObj.Navigate(new PageUsersAdd(null));
        }

        private void CMBFilterForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cat = CMBFilterForm.SelectedValue.ToString();
            if (cat != "Все категории")
            {
                int cod = OrganizerEntities.GetContext().Categories.First(x => x.NameCate == cat).id_category;
                DtgSQL.ItemsSource = OrganizerEntities.GetContext().Users.Where(x => x.id_category == cod).ToList();
                int count = OrganizerEntities.GetContext().Users.Where(x => x.id_category == cod).Count();
                Txtname.Text = count.ToString();

            }
            else
                DtgSQL.ItemsSource = OrganizerEntities.GetContext().Users.ToList();
            int count1 = OrganizerEntities.GetContext().Users.Count();
            Txtname.Text = count1.ToString();
        }

        private void Findsurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            DtgSQL.ItemsSource = OrganizerEntities.GetContext().Users.Where(x => x.first_name.ToLower().Contains(Findsurname.Text.ToLower())).ToList();
            int count = OrganizerEntities.GetContext().Users.Where(x => x.first_name.ToLower().Contains(Findsurname.Text.ToLower())).ToList().Count();
            Txtname.Text = count.ToString();
        }
    }
}
