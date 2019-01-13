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
    /// Logika interakcji dla klasy PageZatwierdzZnizke2.xaml
    /// </summary>
    public partial class PageZatwierdzZnizke2 : Page
    {
        public PageZatwierdzZnizke2()
        {
            InitializeComponent();
        }

        private void Button_Wroc(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("PageWyborFilmu.xaml", UriKind.Relative));
        }

        private void Button_Zaakceptuj(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("PageZaakceptujZniżke.xaml", UriKind.Relative));
        }
    }
}
