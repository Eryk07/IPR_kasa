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

namespace IPR_kasa
{
    /// <summary>
    /// Logika interakcji dla klasy PageAcceptStudentDiscount.xaml
    /// </summary>
    public partial class PageAcceptStudentDiscount : Page
    {
        public PageAcceptStudentDiscount()
        {
            InitializeComponent();
            clientLbl.Content = CDBService.ShowClientName(PageCheckClient2.client.id);
        }

        private void Button_Zaakceptuj(object sender, RoutedEventArgs e)
        {
            PageCheckClient2.client.id_znizka = 2;
            PageCheckClient2.client.exp_date = (DateTime)datePicker1.SelectedDate;
            
            CDBService.AddDiscountToClientDB(PageCheckClient2.client);
            
            this.NavigationService.Navigate(new Uri("PageChooseMovie.xaml", UriKind.Relative));
        }

        private void Button_Wroc(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageCheckClient2.xaml", UriKind.Relative));
        }

        private void Button_Odrzuc(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageChooseMovie.xaml", UriKind.Relative));
        }
    }
}
