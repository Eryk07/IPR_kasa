using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Logika interakcji dla klasy PageWyborFilmu.xaml
    /// </summary>
    public partial class PageWyborFilmu : Page
    {
        public PageWyborFilmu()
        {
            InitializeComponent();
            if (MainWindow.dc.DatabaseExists()) {
                Dictionary<string, List<string>> MoviesDict = 
                WriteSeances();

                DataContext = MoviesDict;
            }

        }

        private void Button_Realizuj(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageNumerRezerwacji.xaml", UriKind.Relative));
        }

        private void Button_Zatwierdz(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageZatwierdzZnizke2.xaml", UriKind.Relative));
        }

        private void Button_Godzina(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("PagePodsumowanie.xaml", UriKind.Relative));
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
}
