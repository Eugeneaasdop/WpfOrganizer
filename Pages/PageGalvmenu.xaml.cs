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

namespace WpfOrganizer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageGalvmenu.xaml
    /// </summary>
    public partial class PageGalvmenu : Page
    {
        public PageGalvmenu()
        {
            InitializeComponent();
        }
        private void Use_Click(object sender, RoutedEventArgs e)
        {
            Classes.ClassFrame.frmObj.Navigate(new PageUsers());
        }

        private void Tas_Click(object sender, RoutedEventArgs e)
        {
            Classes.ClassFrame.frmObj.Navigate(new PageTask());
        }

        private void Eve_Click(object sender, RoutedEventArgs e)
        {
            Classes.ClassFrame.frmObj.Navigate(new PageEvent());
        }
    }
}
