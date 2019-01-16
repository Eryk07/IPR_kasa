﻿using System;
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
    /// Logika interakcji dla klasy PageZaakceptujZniżke.xaml
    /// </summary>
    public partial class PageZaakceptujZniżke : Page
    {
        public PageZaakceptujZniżke()
        {
            InitializeComponent();
        }

        private void Button_Zaakceptuj(object sender, RoutedEventArgs e)
        {
            PageZatwierdzZnizke2.client.id_znizka = 2;
            addClientToDb(PageZatwierdzZnizke2.client);
            this.NavigationService.Navigate(new Uri("PageWyborFilmu.xaml", UriKind.Relative));
        }

        private void Button_Wroc(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageZatwierdzZnizke2.xaml", UriKind.Relative));
        }

        private void Button_Odrzuc(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PageWyborFilmu.xaml", UriKind.Relative));
        }

        private void addClientToDb(CClient client)
        {
            Client record = new Client();


            record.id = client.id;
            record.id_znizka = client.id_znizka;

            MainWindow.dc.Client.InsertOnSubmit(record);
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
}
