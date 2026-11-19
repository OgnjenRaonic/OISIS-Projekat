using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Models;

namespace WpfClient.Views;

public partial class IzmeniAutoraWindow : Window
{
    private readonly Autor _autor;
    private readonly Func<Autor, List<Knjiga>> _getAutorKnjige;
    private readonly Func<Autor, List<Knjiga>> _getDostupneKnjige;
    private readonly Action<Autor, Knjiga> _dodajKnjiguAutoru;
    private readonly Action<Autor, Knjiga> _ukloniKnjiguSaAutora;

    public bool PodaciIzmenjeni { get; private set; }

    public IzmeniAutoraWindow(
        Autor autor,
        Func<Autor, List<Knjiga>> getAutorKnjige,
        Func<Autor, List<Knjiga>> getDostupneKnjige,
        Action<Autor, Knjiga> dodajKnjiguAutoru,
        Action<Autor, Knjiga> ukloniKnjiguSaAutora)
    {
        InitializeComponent();

        _autor = autor;
        _getAutorKnjige = getAutorKnjige;
        _getDostupneKnjige = getDostupneKnjige;
        _dodajKnjiguAutoru = dodajKnjiguAutoru;
        _ukloniKnjiguSaAutora = ukloniKnjiguSaAutora;

        LoadAutorData();
        RefreshKnjigeGrid();
        ValidateForm(null, null!);
    }

    private void LoadAutorData()
    {
        txtIme.Text = _autor.Ime;
        txtPrezime.Text = _autor.Prezime;
        dpDatumRodjenja.SelectedDate = _autor.DatumRodjenja == default
            ? DateTime.Today.AddYears(-25)
            : _autor.DatumRodjenja;
        txtBrojLicneKarte.Text = _autor.BrojLicneKarte;
        txtGodineIskustva.Text = _autor.GodineIskustva.ToString();
        txtTelefon.Text = _autor.Telefon;
        txtEmail.Text = _autor.Email;

        if (_autor.Adresa != null)
        {
            txtUlica.Text = _autor.Adresa.Ulica;
            txtBroj.Text = _autor.Adresa.Broj;
            txtGrad.Text = _autor.Adresa.Grad;
            txtDrzava.Text = _autor.Adresa.Drzava;
        }
    }

    private void RefreshKnjigeGrid()
    {
        dgAutorKnjige.ItemsSource = _getAutorKnjige(_autor);
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
            adresa = _autor.Adresa ?? new Adresa();
            adresa.Ulica = txtUlica.Text.Trim();
            adresa.Broj = txtBroj.Text.Trim();
            adresa.Grad = txtGrad.Text.Trim();
            adresa.Drzava = txtDrzava.Text.Trim();
        }

        _autor.Ime = txtIme.Text.Trim();
        _autor.Prezime = txtPrezime.Text.Trim();
        _autor.DatumRodjenja = dpDatumRodjenja.SelectedDate!.Value;
        _autor.BrojLicneKarte = txtBrojLicneKarte.Text.Trim();
        _autor.GodineIskustva = godineIskustva;
        _autor.Telefon = txtTelefon.Text.Trim();
        _autor.Email = txtEmail.Text.Trim();
        _autor.Adresa = adresa;

        PodaciIzmenjeni = true;
        DialogResult = true;
        Close();
    }

    private void BtnOdustani_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void BtnDodajKnjigu_Click(object sender, RoutedEventArgs e)
    {
        var dostupne = _getDostupneKnjige(_autor);
        if (dostupne.Count == 0)
        {
            MessageBox.Show(this, "Nema dostupnih knjiga za dodavanje.", "Dodaj knjigu", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var picker = new OdaberiKnjiguWindow(dostupne)
        {
            Owner = this
        };

        if (picker.ShowDialog() == true && picker.IzabranaKnjiga != null)
        {
            _dodajKnjiguAutoru(_autor, picker.IzabranaKnjiga);
            PodaciIzmenjeni = true;
            RefreshKnjigeGrid();
        }
    }

    private void BtnUkloniKnjigu_Click(object sender, RoutedEventArgs e)
    {
        if (dgAutorKnjige.SelectedItem is not Knjiga izabranaKnjiga)
        {
            MessageBox.Show(this, "Prvo označite knjigu za uklanjanje.", "Ukloni knjigu", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var potvrda = MessageBox.Show(
            this,
            "Da li ste sigurni da želite da uklonite knjigu?",
            "Ukloni knjigu",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (potvrda == MessageBoxResult.Yes)
        {
            _ukloniKnjiguSaAutora(_autor, izabranaKnjiga);
            PodaciIzmenjeni = true;
            RefreshKnjigeGrid();
        }
    }
}