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
    /// Logika interakcji dla klasy PageChooseMovie.xaml
    /// </summary>
    public partial class PageChooseMovie : Page
    {
        public static COrder order { get; set; }
        private bool titleSelected = false;
        public PageChooseMovie()
        {
            InitializeComponent();
            if (MainWindow.dc.DatabaseExists())
            {
                List<CSeance> tmplist= CDBService.ReadSeancesFromDatabase();
                Dictionary<string, List<string>> MoviesDict = new Dictionary<string, List<string>>();
                foreach (CSeance i in tmplist)
                {
                    MoviesDict.Add(i.title,i.hoursList);
                }
             
                DataContext = MoviesDict;
            }
            order = new COrder();
        }

        private void Button_Realizuj(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageCheckReservation.xaml", UriKind.Relative));
        }


        
        private void Button_Zatwierdz(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageCheckClient2.xaml", UriKind.Relative));
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
                CDBService.ChooseSeance(order);
                this.NavigationService.Navigate(new Uri("PageChooseSeat.xaml", UriKind.Relative));
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
         //   using (var db = MainWindow.dc)
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
        public bool paid { get; set; }

        public COrder()
        {
            this.title = "";
            this.seance_time = "";
            this.seat_id = new List<int>();
            this.paid = false;
        }
    }
    
    }
