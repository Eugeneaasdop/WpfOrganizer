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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfOrganizer.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfOrganizer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageEvent.xaml
    /// </summary>
    public partial class PageEvent : Page
    {
        public PageEvent()
        {
            InitializeComponent();
            DtgSQL.ItemsSource = OrganizerEntities.GetContext().Events.ToList();
            var listname = OrganizerEntities.GetContext().Users.Select(x => x.first_name).Distinct().ToList();
            CMBFilterForm.Items.Add("Все пользователи");
            foreach (string name in listname)
            {
                CMBFilterForm.Items.Add(name);
            }
        }

        private void Btnwrite_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open($"{Directory.GetCurrentDirectory()}\\Shablon.xlsx");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Cells[1, 4] = "Органайзер";
            ws.Cells[2, 4] = "Список планируемых задач";
            ws.Cells[7, 1] = "Дата";
            DateTime thisDay = DateTime.Today;
            ws.Cells[7, 2] = thisDay.ToShortDateString();
            int indexRows = 9;
            //ячейка
            ws.Cells[1][indexRows] = "Пользователь";
            ws.Cells[2][indexRows] = "Задача";
            ws.Cells[3][indexRows] = "Описание";
            ws.Cells[4][indexRows] = "Начальная дата";
            ws.Cells[5][indexRows] = "Конечная дата";
            ws.Cells[6][indexRows] = "Локация";
            ws.Cells[6][indexRows] = "Время напоминания";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = DtgSQL.Items;
            //цикл по данным из списка для печати
            foreach (Events item in printItems)
            {
                ws.Cells[1][indexRows + 1] = item.Users.first_name;
                ws.Cells[2][indexRows + 1] = item.event_name;
                ws.Cells[3][indexRows + 1] = item.description;
                ws.Cells[4][indexRows + 1] = item.start_datetime;
                ws.Cells[5][indexRows + 1] = item.end_datetime;
                ws.Cells[6][indexRows + 1] = item.location;
                ws.Cells[6][indexRows + 1] = item.reminder_datetime;
                indexRows++;
            }
            int count = Convert.ToInt32(Txtname.Text);
            ws.Cells[indexRows + 1, 2] = count.ToString();
            ws.Cells[indexRows + 1, 1] = "Итого:";
            excelApp.Visible = true;
        }

        private void PGlav_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageGalvmenu());
        }

        private void Delette_Click(object sender, RoutedEventArgs e)
        {
            var AftoForRemoving = DtgSQL.SelectedItems.Cast<Events>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {AftoForRemoving.Count()} записи?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    OrganizerEntities.GetContext().Events.RemoveRange(AftoForRemoving);
                    OrganizerEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    DtgSQL.ItemsSource = OrganizerEntities.GetContext().Events.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            int count = OrganizerEntities.GetContext().Events.Count();
            Txtname.Text = count.ToString();
        }

        private void BTNedit_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageEventAdd((Events)DtgSQL.SelectedItem));
        }

        private void Btnadd_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageEventAdd(null));
        }

        private void CMBFilterForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string aut = CMBFilterForm.SelectedValue.ToString();
            if (aut != "Все пользователи")
            {
                int cod = OrganizerEntities.GetContext().Users.First(x => x.first_name == aut).id_users;
                DtgSQL.ItemsSource = OrganizerEntities.GetContext().Events.Where(x => x.id_users == cod).ToList();
                int count = OrganizerEntities.GetContext().Events.Where(x => x.id_users == cod).Count();
                Txtname.Text = count.ToString();
            }
            else
            {
                DtgSQL.ItemsSource = OrganizerEntities.GetContext().Events.ToList();
                int count1 = OrganizerEntities.GetContext().Events.Count();
                Txtname.Text = count1.ToString();
            }
        }

        private void Findsurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            DtgSQL.ItemsSource = OrganizerEntities.GetContext().Events.Where(x => x.event_name.ToLower().Contains(Findsurname.Text.ToLower())).ToList();
            int count = OrganizerEntities.GetContext().Events.Where(x => x.event_name.ToLower().Contains(Findsurname.Text.ToLower())).ToList().Count();
            Txtname.Text = count.ToString();
        }
    }
}
