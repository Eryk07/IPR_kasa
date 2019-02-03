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
    /// Logika interakcji dla klasy PageCheckClient.xaml
    /// </summary>
    public partial class PageCheckClient : Page
    {
        public PageCheckClient()
        {
            InitializeComponent();
        }

        private void Button_Zaakceptuj(object sender, RoutedEventArgs e)
        {
            if (CDBService.CheckClient(TxtBox.Text))
            {
                PageChooseSeat.currentClient.id = Int32.Parse(TxtBox.Text);
                if (CDBService.CheckClientDiscount(TxtBox.Text))
                {
                    PageChooseSeat.currentClient.id_znizka = 3;
                }
                else
                    MessageBox.Show("Podany klient nie ma zniżki!");
            }
            else
                MessageBox.Show("Nie ma takiego klienta!");

            this.NavigationService.Navigate(new Uri("PageChooseSeat.xaml", UriKind.Relative));
        }

        private void TextBox_Focused(object sender, RoutedEventArgs e)
        {
            TxtBox.Text = "";
        }


    }
}
