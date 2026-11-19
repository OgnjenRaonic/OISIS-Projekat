using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core.Models;

namespace WpfClient.Views;

public partial class OdaberiAutoraWindow : Window
{
    public Autor? IzabraniAutor { get; private set; }

    public OdaberiAutoraWindow(List<Autor> autori)
    {
        InitializeComponent();
        lstAutori.ItemsSource = autori;
        if (autori.Count > 0)
        {
            lstAutori.SelectedIndex = 0;
        }
    }

    private void LstAutori_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (btnPotvrdi != null)
        {
            btnPotvrdi.IsEnabled = lstAutori.SelectedItem is Autor;
        }
    }

    private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
    {
        if (lstAutori.SelectedItem is Autor autor)
        {
            IzabraniAutor = autor;
            DialogResult = true;
            Close();
        }
    }

    private void BtnOdustani_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}