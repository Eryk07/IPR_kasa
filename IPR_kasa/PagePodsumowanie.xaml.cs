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
    /// Logika interakcji dla klasy PagePodsumowanie.xaml
    /// </summary>
    public partial class PagePodsumowanie : Page
    {
        public PagePodsumowanie()
        {
            InitializeComponent();
           // _NavigationFrame.Navigate(new PagePodsumowanie());
        }

        private void Zaakceptuj_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Płatność i druk");
        }


    }
}
