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
using System.Xml.Linq;
using WpfOrganizer.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfOrganizer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageTask.xaml
    /// </summary>
    public partial class PageTask : Page
    {
        public PageTask()
        {
            InitializeComponent();
            DtgSQL.ItemsSource = OrganizerEntities.GetContext().Tasks.ToList();
            var listser = OrganizerEntities.GetContext().Priority.Select(x => x.NamePi).Distinct().ToList();
            CMBFilterForm.Items.Add("Все степени важности");
            foreach (string serios in listser)
            {
                CMBFilterForm.Items.Add(serios);
            }
            System.Int32 customerCount = OrganizerEntities.GetContext().Tasks.Count();
            Txtname.Text = customerCount.ToString();
        }

        private void PGlav_Click(object sender, RoutedEventArgs e)
        {
           ClassFrame.frmObj.Navigate(new PageGalvmenu());
        }

        private void Delette_Click(object sender, RoutedEventArgs e)
        {
            var AftoForRemoving = DtgSQL.SelectedItems.Cast<Tasks>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {AftoForRemoving.Count()} записи?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    OrganizerEntities.GetContext().Tasks.RemoveRange(AftoForRemoving);
                    OrganizerEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    DtgSQL.ItemsSource = OrganizerEntities.GetContext().Tasks.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BTNedit_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageTaskAdd((Tasks)DtgSQL.SelectedItem));
        }

        private void Btnwrite_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open($"{Directory.GetCurrentDirectory()}\\Shablon.xlsx");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Cells[1, 4] = "Органайзер";
            ws.Cells[2, 4] = "Список задач";
            ws.Cells[7, 1] = "Дата";
            DateTime thisDay = DateTime.Today;
            ws.Cells[7, 2] = thisDay.ToShortDateString();
            int indexRows = 9;
            //ячейка
            ws.Cells[1][indexRows] = "Пользователь";
            ws.Cells[2][indexRows] = "Задача";
            ws.Cells[3][indexRows] = "Описание";
            ws.Cells[4][indexRows] = "Дата создания";
            ws.Cells[5][indexRows] = "Дата исполнения";
            ws.Cells[6][indexRows] = "Степень важности";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = DtgSQL.Items;
            //цикл по данным из списка для печати
            foreach (Tasks item in printItems)
            {
                ws.Cells[1][indexRows + 1] = item.Users.first_name;
                ws.Cells[2][indexRows + 1] = item.Taskname;
                ws.Cells[3][indexRows + 1] = item.description;
                ws.Cells[4][indexRows + 1] = item.creation_datetime;
                ws.Cells[5][indexRows + 1] = item.due_datetime;
                ws.Cells[6][indexRows + 1] = item.Priority.NamePi;
                indexRows++;
            }
            int count = Convert.ToInt32(Txtname.Text);
            ws.Cells[indexRows + 1, 2] = count.ToString();
            ws.Cells[indexRows + 1, 1] = "Итого:";
            excelApp.Visible = true;
        }

        private void Btnadd_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageTaskAdd(null));
        }

        private void CMBFilterForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string city = CMBFilterForm.SelectedValue.ToString();
            if (city != "Все степени важности")
            {
                int cod = OrganizerEntities.GetContext().Priority.First(x => x.NamePi == city).id_priority;
                DtgSQL.ItemsSource = OrganizerEntities.GetContext().Tasks.Where(x => x.id_priority == cod).ToList();
                int count = OrganizerEntities.GetContext().Tasks.Where(x => x.id_priority == cod).Count();
                Txtname.Text = count.ToString();

            }
            else
                DtgSQL.ItemsSource = OrganizerEntities.GetContext().Tasks.ToList();
            int count1 = OrganizerEntities.GetContext().Tasks.Count();
            Txtname.Text = count1.ToString();
        }
        private void Findzadan_TextChanged(object sender, TextChangedEventArgs e)
        {
            DtgSQL.ItemsSource = OrganizerEntities.GetContext().Tasks.Where(x => x.Taskname.ToLower().Contains(Findzadan.Text.ToLower())).ToList();
            int count = OrganizerEntities.GetContext().Tasks.Where(x => x.Taskname.ToLower().Contains(Findzadan.Text.ToLower())).ToList().Count();
            Txtname.Text = count.ToString();
        }
    }
}
