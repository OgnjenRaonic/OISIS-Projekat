using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Core.Models;

namespace WpfClient.Views;

public partial class DodajKnjiguWindow : Window
{
    public Knjiga? KreiranaKnjiga { get; private set; }

    public DodajKnjiguWindow()
    {
        InitializeComponent();
        ValidateForm(null, null!);
    }

    private void ValidateForm(object? sender, TextChangedEventArgs e)
    {
        bool validIsbn = !string.IsNullOrWhiteSpace(txtIsbn.Text);
        bool validNaziv = !string.IsNullOrWhiteSpace(txtNaziv.Text);
        bool validZanr = !string.IsNullOrWhiteSpace(txtZanr.Text);
        bool validGodina = int.TryParse(txtGodinaIzdanja.Text.Trim(), out int godina) && godina > 0 && godina <= DateTime.Now.Year + 1;
        bool validCena = decimal.TryParse(
            txtCena.Text.Trim().Replace(',', '.'),
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out decimal cena) && cena >= 0;
        bool validStrane = int.TryParse(txtBrojStrana.Text.Trim(), out int strane) && strane > 0;

        if (btnPotvrdi != null)
        {
            btnPotvrdi.IsEnabled = validIsbn && validNaziv && validZanr && validGodina && validCena && validStrane;
        }
    }

    private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
    {
        int.TryParse(txtGodinaIzdanja.Text.Trim(), out int godina);
        decimal.TryParse(
            txtCena.Text.Trim().Replace(',', '.'),
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out decimal cena);
        int.TryParse(txtBrojStrana.Text.Trim(), out int strane);

        KreiranaKnjiga = new Knjiga
        {
            ISBN = txtIsbn.Text.Trim(),
            Naziv = txtNaziv.Text.Trim(),
            Zanr = txtZanr.Text.Trim(),
            GodinaIzdanja = godina,
            Cena = cena,
            BrojStrana = strane
        };

        DialogResult = true;
        Close();
    }

    private void BtnOdustani_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}