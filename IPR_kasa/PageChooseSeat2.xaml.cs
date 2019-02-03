using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Logika interakcji dla klasy PageChooseSeat.xaml
    /// </summary>
    public partial class PageChooseSeat2 : Page
    {
        private List<CSeat> seats;
        private List<CSeat> summarySeats;
        public PageChooseSeat2()
        {
            InitializeComponent();
            seats = CDBService.ReadFreeSeatsFromDB(PageChooseMovie.order);
            summarySeats = new List<CSeat>();
            summarySeats.Add(new CSeat(12,true));
            summarySeats.Add(new CSeat(13, false));
            titleLbl.Content = PageChooseMovie.order.title + ", "+ DateTime.Now.ToString(@"dd.MM")+ ", " + PageChooseMovie.order.seance_time;
            DataContext = seats;
            seatsList.DataContext = summarySeats;
            
        }

        private void Zaakceptuj_Click(object sender, RoutedEventArgs e)
        {
            CDBService.SaveOrderToDatabase(PageChooseMovie.order);
            MessageBox.Show("Płatność i druk");
            this.NavigationService.Navigate(new Uri("PageChooseMovie.xaml", UriKind.Relative));
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton srcButton = e.Source as ToggleButton;
            var content = srcButton.Content;
            if (srcButton.Background == Brushes.Green)
            {
                PageChooseMovie.order.seat_id.Add((int)content);
                summarySeats.Add(new CSeat((int)content, true));
                seatsList.DataContext = summarySeats;
                UpdateLayout();
            }
            else
                MessageBox.Show("To miejsce jest zajęte!", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            //summarySeats = PageChooseMovie.order.seat_id.ConvertAll<string>(delegate (int i) { return i.ToString(); });
            
        }

        private void SeatHandleCheck(object sender, RoutedEventArgs e)
        {
            ToggleButton srcButton = e.Source as ToggleButton;
            
        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageChooseMovie.xaml", UriKind.Relative));
        }
    }

}
