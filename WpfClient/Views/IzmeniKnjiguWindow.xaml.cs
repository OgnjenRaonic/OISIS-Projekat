using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Core.Models;

namespace WpfClient.Views;

public partial class IzmeniKnjiguWindow : Window
{
    private readonly Knjiga _knjiga;
    private readonly Func<Knjiga, Autor?> _getTrenutniAutor;
    private readonly Func<List<Autor>> _getSviAutori;
    private readonly Action<Knjiga, Autor> _dodajAutoraNaKnjigu;
    private readonly Action<Knjiga, Autor> _ukloniAutoraSaKnjige;

    private Autor? _dodeljeniAutor;
    public bool PodaciIzmenjeni { get; private set; }

    public IzmeniKnjiguWindow(
        Knjiga knjiga,
        Func<Knjiga, Autor?> getTrenutniAutor,
        Func<List<Autor>> getSviAutori,
        Action<Knjiga, Autor> dodajAutoraNaKnjigu,
        Action<Knjiga, Autor> ukloniAutoraSaKnjige)
    {
        InitializeComponent();

        _knjiga = knjiga;
        _getTrenutniAutor = getTrenutniAutor;
        _getSviAutori = getSviAutori;
        _dodajAutoraNaKnjigu = dodajAutoraNaKnjigu;
        _ukloniAutoraSaKnjige = ukloniAutoraSaKnjige;

        _dodeljeniAutor = _getTrenutniAutor(_knjiga);
        LoadKnjigaData();
        UpdateAutorField();
        ValidateForm(null, null!);
    }

    private void LoadKnjigaData()
    {
        txtIsbn.Text = _knjiga.ISBN;
        txtNaziv.Text = _knjiga.Naziv;
        txtCena.Text = _knjiga.Cena.ToString(CultureInfo.InvariantCulture);
        txtGodinaIzdanja.Text = _knjiga.GodinaIzdanja.ToString();
        txtZanr.Text = _knjiga.Zanr;
    }

    private void UpdateAutorField()
    {
        if (_dodeljeniAutor != null)
        {
            txtAutor.Text = $"{_dodeljeniAutor.Ime} {_dodeljeniAutor.Prezime}";
            btnDodajAutora.IsEnabled = false;
            btnUkloniAutora.IsEnabled = true;
        }
        else
        {
            txtAutor.Text = string.Empty;
            btnDodajAutora.IsEnabled = true;
            btnUkloniAutora.IsEnabled = false;
        }
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

        if (btnPotvrdi != null)
        {
            btnPotvrdi.IsEnabled = validIsbn && validNaziv && validZanr && validGodina && validCena;
        }
    }

    private void BtnDodajAutora_Click(object sender, RoutedEventArgs e)
    {
        var autori = _getSviAutori();
        if (autori.Count == 0)
        {
            MessageBox.Show(this, "Nema registrovanih autora u sistemu.", "Izaberi autora", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var picker = new OdaberiAutoraWindow(autori)
        {
            Owner = this
        };

        if (picker.ShowDialog() == true && picker.IzabraniAutor != null)
        {
            _dodajAutoraNaKnjigu(_knjiga, picker.IzabraniAutor);
            _dodeljeniAutor = picker.IzabraniAutor;
            PodaciIzmenjeni = true;
            UpdateAutorField();
        }
    }

    private void BtnUkloniAutora_Click(object sender, RoutedEventArgs e)
    {
        if (_dodeljeniAutor == null)
            return;

        var potvrda = MessageBox.Show(
            this,
            "Da li ste sigurni da želite da uklonite autora?",
            "Ukloni autora",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (potvrda == MessageBoxResult.Yes)
        {
            _ukloniAutoraSaKnjige(_knjiga, _dodeljeniAutor);
            _dodeljeniAutor = null;
            PodaciIzmenjeni = true;
            UpdateAutorField();
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

        _knjiga.ISBN = txtIsbn.Text.Trim();
        _knjiga.Naziv = txtNaziv.Text.Trim();
        _knjiga.Cena = cena;
        _knjiga.GodinaIzdanja = godina;
        _knjiga.Zanr = txtZanr.Text.Trim();

        PodaciIzmenjeni = true;
        DialogResult = true;
        Close();
    }

    private void BtnOdustani_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}