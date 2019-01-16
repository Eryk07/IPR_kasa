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
        public static COrder order { get; set; }
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
                using (var db = new MultikinoLINQDataContext(
                Properties.Settings.Default.MultikinoConnectionString))
                {
                    var movie_id = db
                                 .Movie
                                 //.Where(x => System.Data.Linq.SqlClient.SqlMethods.Like(x.title, order.title))
                                 .Where(x => x.title.Equals(order.title))
                                 .Select(x => x.id)
                                 .ToList()
                                 .FirstOrDefault();
                    var seance_id = db
                                 .Seance
                                 .Where(x => x.id_movie == movie_id && x.time == TimeSpan.Parse(order.seance_time) && x.date == DateTime.Today)
                                 .Select(x => x.id)
                                 .ToList()
                                 .FirstOrDefault();
                    order.id_seance = seance_id;

                }
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
                var movies = db.Movie
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

               var title = db.Movie.Where(o => o.id == id).Select(o => o.title).ToList();
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
        public int id_seance { get; set; }
        public List<int> seat_id { get; set; }


        public COrder()
        {
            this.title = "";
            this.seance_time = "";
            this.seat_id = new List<int>();

        }
    }
    
    }
