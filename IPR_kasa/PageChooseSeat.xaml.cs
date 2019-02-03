using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<CSeat> summarySeats;
        public static CClient currentClient; 
        public PageChooseSeat()
        {
            InitializeComponent();
            seats = CDBService.ReadFreeSeatsFromDB(PageChooseMovie.order);
            summarySeats = new ObservableCollection<CSeat>();
            titleLbl.Content = PageChooseMovie.order.title + ", "+ DateTime.Now.ToString(@"dd.MM")+ ", " + PageChooseMovie.order.seance_time;
            DataContext = seats;
            seatsList.DataContext = summarySeats;

            currentClient = new CClient();

        }

        private void Zaakceptuj_Click(object sender, RoutedEventArgs e)
        {
            foreach (CSeat item in summarySeats)
            {
                PageChooseMovie.order.seat_id.Add(item.id);
            }
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
                //PageChooseMovie.order.seat_id.Add((int)content);
                summarySeats.Add(new CSeat((int)content, true));
                
                UpdateLayout();
            }
            else
                MessageBox.Show("To miejsce jest zajęte!", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            //summarySeats = PageChooseMovie.order.seat_id.ConvertAll<string>(delegate (int i) { return i.ToString(); });
            
        }

        private void SeatHandleUncheck(object sender, RoutedEventArgs e)
        {
            ToggleButton srcButton = e.Source as ToggleButton;
            var content = srcButton.Content;
            var itemToRemove = summarySeats.SingleOrDefault(r => r.id == (int)content);
            if (itemToRemove != null)
                summarySeats.Remove(itemToRemove);
            //summarySeats.RemoveAt(0);
            UpdateLayout();
            
        }

        private bool handle = true;
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (handle) Handle(cmb);
            handle = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            Handle(cmb);
        }

        private void Handle(ComboBox cmb)
        {
            switch (cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Normalny":
                    break;
                case "Student":
                    break;
                case "Twoja zniżka":
                    this.NavigationService.Navigate(new Uri("PageCheckClient.xaml", UriKind.Relative));
                    break;
            }
        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageChooseMovie.xaml", UriKind.Relative));
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
