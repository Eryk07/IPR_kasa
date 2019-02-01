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
    public partial class PageChooseSeat : Page
    {
        private List<CSeat> seats;
        List<string> summarySeats;
        public PageChooseSeat()
        {
            InitializeComponent();
            // _NavigationFrame.Navigate(new PageChooseSeat());
            seats = CDBService.ReadFreeSeatsFromDB(PageChooseMovie.order);
            summarySeats = new List<string>();
            summarySeats.Add("12");
            titleLbl.Content = PageChooseMovie.order.title + ", "+ DateTime.Now.ToString(@"dd.MM")+ ", " + PageChooseMovie.order.seance_time;
            DataContext = seats;

        }

        private void Zaakceptuj_Click(object sender, RoutedEventArgs e)
        {
            CDBService.SaveOrderToDatabase(PageChooseMovie.order);
            MessageBox.Show("Płatność i druk");
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton srcButton = e.Source as ToggleButton;
            var content = srcButton.Content;
            PageChooseMovie.order.seat_id.Add((int)content);
            //summarySeats = PageChooseMovie.order.seat_id.ConvertAll<string>(delegate (int i) { return i.ToString(); });
            UpdateLayout();
        }
    }
    public class CSeat
    {
        public int id { get; set; }
        public bool available { get; set; }
        public CSeat()
        {
            id = 0;
            available = true;
        }
        public CSeat(int id, bool available)
        {
            this.id = id;
            this.available = available;
        }

    }


}
