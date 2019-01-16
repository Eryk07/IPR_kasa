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
    /// Logika interakcji dla klasy PagePodsumowanie.xaml
    /// </summary>
    public partial class PagePodsumowanie : Page
    {
        private List<CSeat> seats;
        List<string> summarySeats;
        public PagePodsumowanie()
        {
            InitializeComponent();
            // _NavigationFrame.Navigate(new PagePodsumowanie());
            seats = SetSeats(PageWyborFilmu.order);
            summarySeats = new List<string>();
            summarySeats.Add("12");
            titleLbl.Content = PageWyborFilmu.order.title + ", "+ DateTime.Now.ToString(@"dd.MM")+ ", " + PageWyborFilmu.order.seance_time;
            DataContext = seats;

        }

        private void Zaakceptuj_Click(object sender, RoutedEventArgs e)
        {
            saveOrderToDatabase(PageWyborFilmu.order);
            MessageBox.Show("Płatność i druk");
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton srcButton = e.Source as ToggleButton;
            var content = srcButton.Content;
            PageWyborFilmu.order.seat_id.Add((int)content);
            //summarySeats = PageWyborFilmu.order.seat_id.ConvertAll<string>(delegate (int i) { return i.ToString(); });
            UpdateLayout();
        }

        void saveOrderToDatabase(COrder order)
        {
            //DateTime.Now

            // Add the new object to the Orders collection.
            foreach (int i in order.seat_id)
            {
                Order record = new Order();
                using (var db = new MultikinoLINQDataContext(
                Properties.Settings.Default.MultikinoConnectionString))
                {
                    var last_id = db.Order
                                 .OrderByDescending(x => x.id)
                                 .Take(1)
                                 .Select(x => x.id)
                                 .ToList()
                                 .FirstOrDefault();

                    record.id = last_id + 100;
                }

                record.title = order.title;
                record.order_time = DateTime.Now;
                record.seance_time = TimeSpan.Parse(order.seance_time);
                record.id_seance = order.id_seance;
                record.seat_number = i;

                MainWindow.dc.Order.InsertOnSubmit(record);
                // Submit the change to the database.
                try
                {
                    MainWindow.dc.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Make some adjustments.
                    // ...
                    // Try again.
                    MainWindow.dc.SubmitChanges();
                }
            }
        }

        public List<CSeat> SetSeats(COrder order)
        {
            List<CSeat> seats = new List<CSeat>();
            for (int i = 1; i <= 24; i++)
            {
                seats.Add(new CSeat(i, true));
            }

            using (var db = new MultikinoLINQDataContext(
            Properties.Settings.Default.MultikinoConnectionString))
            {
                var last_id = db.Order
                             .Where(x=>x.id_seance==order.id_seance)
                             .Select(x => x.seat_number)
                             .ToList();



                foreach (CSeat s in seats)
                {
                    foreach (int i in last_id) {
                        if (s.id == i)
                            s.available = false;
                    }
                }

            }

            
            return seats;
        }

        private void SeatBtn_Click(object sender, RoutedEventArgs e)
        {

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
