using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Logika interakcji dla klasy PageWyborFilmu.xaml
    /// </summary>
    public partial class PageWyborFilmu : Page
    {
        private COrder order;
        private bool titleSelected = false;
        public PageWyborFilmu()
        {
            InitializeComponent();
            if (MainWindow.dc.DatabaseExists()) {
                Dictionary<string, List<string>> MoviesDict = 
                WriteSeances();

                DataContext = MoviesDict;
            }
            order = new COrder();
        }

        private void Button_Realizuj(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageNumerRezerwacji.xaml", UriKind.Relative));
        }

        private void Button_Zatwierdz(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageZatwierdzZnizke2.xaml", UriKind.Relative));
        }

        private void Button_Tytul(object sender, RoutedEventArgs e)
        {
            ToggleButton srcButton = e.Source as ToggleButton;
            var textblock = (TextBlock)srcButton.Content;
            order.title = textblock.Text.ToString();
            titleSelected = true;

        }

        private void Button_Godzina(object sender, RoutedEventArgs e)
        {
            Button srcButton = e.Source as Button;

            order.seance_time = srcButton.Content.ToString();

            //var dict = (KeyValuePair<string,List<String>>)itemsControl.Items.CurrentItem;
            // string tilte  = dict.Key; 
            if (titleSelected)
            {
                saveOrderToDatabase();
                this.NavigationService.Navigate(new Uri("PagePodsumowanie.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Zanim wybierzesz godzinę, najpierw wybierz tytuł który jest do niej przypisany", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private Dictionary<string, List<string>> WriteSeances()
        {
            Dictionary<string, List<string>> seances = new Dictionary<string, List<string>>(); 

            using (var db = new MultikinoLINQDataContext(
            Properties.Settings.Default.MultikinoConnectionString))
            {
                var movies = db.Movies
                .Select(o => o.id)
                .ToList();

                foreach(int i in movies)
                {
                    CSeance tmp = new CSeance(i);
                    if(tmp.hoursList.Any())
                    seances.Add(tmp.title,tmp.hoursList);
                 }

            }
            return seances;
            
        }

        void saveOrderToDatabase()
        {
            //DateTime.Now

            // Add the new object to the Orders collection.
            
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

                record.id = last_id + 1;
            }

            record.title = order.title;
            record.order_time = DateTime.Now;
            record.seance_time = TimeSpan.Parse(order.seance_time);

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

    public class CSeance
    {   
        public int id { get; set; }
        public string title { get; set; }
        public List<string> hoursList { get; set; }

        public CSeance()
        {
            this.title = " ";
            
        }
        public CSeance(int id)
        {
            

            using (var db = new MultikinoLINQDataContext(
            Properties.Settings.Default.MultikinoConnectionString))
            {
                var seances = db.Seance
                .Where(o => o.id_movie == id && o.date == DateTime.Today)
                .Select(o => o.time)
                .ToList();

               var title = db.Movies.Where(o => o.id == id).Select(o => o.title).ToList();
                this.title = title.First();

                var seancesAsString = seances.Select(s => s.ToString(@"hh\:mm")).ToList();
                this.hoursList = seancesAsString;
            }
            

        }

       
    }
    public class COrder
    {
        public int id { get; set; }
        public string title { get; set; }
        public string seance_time { get; set; }

        public COrder()
        {
            this.title = "";
            this.seance_time = "";
        }
    }
    
    }
