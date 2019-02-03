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
    /// Logika interakcji dla klasy PageCheckClient2.xaml
    /// </summary>
    public partial class PageCheckClient2 : Page
    {
        public static CClient client;
        public PageCheckClient2()
        {
            InitializeComponent();
            client = new CClient();
        }

        private void Button_Wroc(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("PageChooseMovie.xaml", UriKind.Relative));
        }

        private void Button_Zaakceptuj(object sender, RoutedEventArgs e)
        {
            
            if (CDBService.CheckClient(TxtBox.Text))
            {
                client.id = Int32.Parse(TxtBox.Text);
                NavigationService.Navigate(new Uri("PageAcceptStudentDiscount.xaml", UriKind.Relative));
            }
            else
                MessageBox.Show("Nie ma takiego klienta!");
        }
        private void TextBox_Focused(object sender, RoutedEventArgs e)
        {
            TxtBox.Text = "";
        }

    }

    public class CClient
    {
        public int id { get; set; }
        public int id_znizka { get; set; }
        public DateTime exp_date { get; set; }
        
    }

}
