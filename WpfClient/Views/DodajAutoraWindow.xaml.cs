using System;
using System.Windows;
using System.Windows.Controls;
using Core.Models;

namespace WpfClient.Views;

public partial class DodajAutoraWindow : Window
{
    public Autor? KreiraniAutor { get; private set; }
    public Adresa? KreiranaAdresa { get; private set; }

    public DodajAutoraWindow()
    {
        InitializeComponent();
        dpDatumRodjenja.SelectedDate = DateTime.Today.AddYears(-25);
        ValidateForm(null, null!);
    }

    private void DpDatumRodjenja_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        ValidateForm(null, null!);
    }

    private void ValidateForm(object? sender, TextChangedEventArgs e)
    {
        bool validIme = !string.IsNullOrWhiteSpace(txtIme.Text);
        bool validPrezime = !string.IsNullOrWhiteSpace(txtPrezime.Text);
        bool validDatum = dpDatumRodjenja.SelectedDate.HasValue && dpDatumRodjenja.SelectedDate.Value <= DateTime.Today;
        bool validLicna = !string.IsNullOrWhiteSpace(txtBrojLicneKarte.Text);
        bool validEmail = !string.IsNullOrWhiteSpace(txtEmail.Text) && txtEmail.Text.Contains('@');
        bool validIskustvo = int.TryParse(txtGodineIskustva.Text.Trim(), out int iskustvo) && iskustvo >= 0;

        if (btnPotvrdi != null)
        {
            btnPotvrdi.IsEnabled = validIme && validPrezime && validDatum && validLicna && validEmail && validIskustvo;
        }
    }

    private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
    {
        int.TryParse(txtGodineIskustva.Text.Trim(), out int godineIskustva);

        bool imaAdresu = !string.IsNullOrWhiteSpace(txtUlica.Text) ||
                         !string.IsNullOrWhiteSpace(txtBroj.Text) ||
                         !string.IsNullOrWhiteSpace(txtGrad.Text) ||
                         !string.IsNullOrWhiteSpace(txtDrzava.Text);

        Adresa? adresa = null;
        if (imaAdresu)
        {
            adresa = new Adresa
            {
                Ulica = txtUlica.Text.Trim(),
                Broj = txtBroj.Text.Trim(),
                Grad = txtGrad.Text.Trim(),
                Drzava = txtDrzava.Text.Trim()
            };
        }

        KreiranaAdresa = adresa;
        KreiraniAutor = new Autor
        {
            Ime = txtIme.Text.Trim(),
            Prezime = txtPrezime.Text.Trim(),
            DatumRodjenja = dpDatumRodjenja.SelectedDate!.Value,
            BrojLicneKarte = txtBrojLicneKarte.Text.Trim(),
            GodineIskustva = godineIskustva,
            Telefon = txtTelefon.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            Adresa = adresa
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