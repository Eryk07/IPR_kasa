using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR_kasa
{
    class CDBService
    {
        public static List<CSeance> ReadSeancesFromDatabase()
        {
            List<CSeance> list = new List<CSeance>();
            using (var db = new MultikinoLINQDataContext(
            Properties.Settings.Default.MultikinoConnectionString))
            {
                var movies = db.Movie
                .Select(o => o.id)
                .ToList();

                foreach (int i in movies)
                {
                    CSeance tmp = new CSeance(i);
                    if (tmp.hoursList.Any())
                        list.Add(tmp);
                }
            }
            return list;
        }

        public static void ChooseSeance(COrder order)
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
        }

        public static List<CSeat> ReadFreeSeatsFromDB(COrder order)
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
                    .Where(x => x.id_seance == order.id_seance)
                    .Select(x => x.seat_number)
                    .ToList();

                foreach (CSeat s in seats)
                {
                    foreach (int i in last_id)
                    {
                        if (s.id == i)
                            s.available = false;
                    }
                }

            }


            return seats;
        }

        public static void SaveOrderToDatabase(COrder order)
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
                if (PageCheckClient2.client != null)
                    record.id_client = PageCheckClient2.client.id;
                record.paid = order.paid;
                if (PageCheckClient2.client != null)
                    record.id_discount = 1;
                
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

        public static bool CheckClient(string clientIdInput)
        {
            bool client_exist = false;
            using (var db = new MultikinoLINQDataContext(
                Properties.Settings.Default.MultikinoConnectionString))
            {
                var client_id = db
                    .Client
                    //.Where(x => System.Data.Linq.SqlClient.SqlMethods.Like(x.title, order.title))
                    .Where(x => x.id.ToString().Equals(clientIdInput))
                    .Select(x => x.id_znizka)
                    .ToList();



                if (client_id.Any())
                    client_exist = true;
            }

            return client_exist;
        }

        public static void AddDiscountToClientDB(CClient client)
        {

            using (var db = new MultikinoLINQDataContext(
                Properties.Settings.Default.MultikinoConnectionString))
            {
                var clt = db.Client
                    .Where(c => c.id == client.id)
                    .SingleOrDefault();

                if (clt != null)
                {
                    clt.id_znizka = 2;
                    clt.student_date = client.exp_date;
                }

                // Submit the change to the database.
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Make some adjustments.
                    // ...
                    // Try again.
                    db.SubmitChanges();
                }
            }

        }

        public static string ShowClientName(int id)
        {
            string output; 
            using (var db = new MultikinoLINQDataContext(
            Properties.Settings.Default.MultikinoConnectionString))
            {
                var name = db.Client
                    .Where(x=> x.id == id)
                    .Select(x => x.name)
                    .ToList()
                    .FirstOrDefault();

                var surname = db.Client
                    .Where(x => x.id == id)
                    .Select(x => x.surname)
                    .ToList()
                    .FirstOrDefault();

                output = name + " " + surname;
            }
            return output;
        }

        public static bool CheckClientDiscount(string clientIdInput)
        {
            bool client_discount = false;
            using (var db = new MultikinoLINQDataContext(
                Properties.Settings.Default.MultikinoConnectionString))
            {
                var client_id = db
                    .Client
                    //.Where(x => System.Data.Linq.SqlClient.SqlMethods.Like(x.title, order.title))
                    .Where(x => x.id.ToString().Equals(clientIdInput))
                    .Select(x => x.id_znizka)
                    .ToList();



                if (client_id.FirstOrDefault() == 3)
                    client_discount = true;
            }

            return client_discount;
        }

    }
}