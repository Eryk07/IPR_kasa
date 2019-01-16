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
    /// Logika interakcji dla klasy PageMenu.xaml
    /// </summary>
    public partial class PageNumerRezerwacji : Page
    {
        public PageNumerRezerwacji()
        {
            
            InitializeComponent();

        }

        private void Button_Wroc(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageWyborFilmu.xaml", UriKind.Relative));
        }

        private void Button_Zrealizuj(object sender, RoutedEventArgs e)
        {
            bool reservation_exist = false;
            using (var db = new MultikinoLINQDataContext(
                Properties.Settings.Default.MultikinoConnectionString))
            {
                var order_id = db
                                 .Order
                                 //.Where(x => System.Data.Linq.SqlClient.SqlMethods.Like(x.title, order.title))
                                 .Where(x => x.id.ToString().Equals(TxtBox.Text))
                                 .Select(x => x.order_time)
                                 .ToList();

                if (order_id.Any())
                    reservation_exist = true;
            }

            if(reservation_exist)
                MessageBox.Show("Płatność i druk");
            else
                MessageBox.Show("Nie ma takiego numeru rezerwacji");
        }
        private void TextBox_Focused(object sender, RoutedEventArgs e)
        {
            TxtBox.Text="";
        }

    }
}
