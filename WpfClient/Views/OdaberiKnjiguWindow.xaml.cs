using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core.Models;

namespace WpfClient.Views;

public partial class OdaberiKnjiguWindow : Window
{
    public Knjiga? IzabranaKnjiga { get; private set; }

    public OdaberiKnjiguWindow(List<Knjiga> knjige)
    {
        InitializeComponent();
        lstKnjige.ItemsSource = knjige;
        if (knjige.Count > 0)
        {
            lstKnjige.SelectedIndex = 0;
        }
    }

    private void LstKnjige_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (btnPotvrdi != null)
        {
            btnPotvrdi.IsEnabled = lstKnjige.SelectedItem is Knjiga;
        }
    }

    private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
    {
        if (lstKnjige.SelectedItem is Knjiga knjiga)
        {
            IzabranaKnjiga = knjiga;
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