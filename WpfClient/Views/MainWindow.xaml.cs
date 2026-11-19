using Core.DAO;
using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using WpfClient.Controllers;
using WpfClient.Utils;

namespace WpfClient.Views;

public partial class MainWindow : Window
{
    private const int PageSize = 16;

    private readonly MainController? _controller;
    private readonly AdresaService _adresaService;
    private readonly AutorService _autorService;
    private readonly KnjigaService _knjigaService;

    private DispatcherTimer _timer;

    private List<Autor> _filteredAutori = new();
    private List<Knjiga> _filteredKnjige = new();
    private int _autoriPage = 1;
    private int _knjigePage = 1;

    private string? _autoriSortMember;
    private ListSortDirection _autoriSortDirection = ListSortDirection.Ascending;

    private string? _knjigeSortMember;
    private ListSortDirection _knjigeSortDirection = ListSortDirection.Ascending;

    public MainWindow()
    {
        InitializeComponent();

        _adresaService = new AdresaService(new AdresaDao());
        _autorService = new AutorService(new AutorDao());
        _knjigaService = new KnjigaService(new KnjigaDao());

        _timer = new DispatcherTimer();
        SetupTimer();

        LocalizationManager.LanguageChanged += ApplyLocalization;
    }

    public MainWindow(MainController controller) : this()
    {
        _controller = controller;
        _adresaService = controller.AdresaService;
        _autorService = controller.AutorService;
        _knjigaService = controller.KnjigaService;
    }

    private void SetupTimer()
    {
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (s, e) => UpdateStatusDateTime();
        UpdateStatusDateTime();
        _timer.Start();
    }

    private void UpdateStatusDateTime()
    {
        txtStatusTime.Text = DateTime.Now.ToString("HH:mm");
        txtStatusDate.Text = DateTime.Now.ToString("dd.MM.yyyy.");
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Inicijalno zauzimanje 3/4 širine i visine ekrana i centriranje (#glavni_prozor)
        Width = SystemParameters.PrimaryScreenWidth * 0.75;
        Height = SystemParameters.PrimaryScreenHeight * 0.75;
        Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
        Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;

        ApplyLocalization();
        RefreshData();
    }

    public void RefreshData()
    {
        _autoriPage = 1;
        _knjigePage = 1;
        ApplySearchAndSort();
    }

    private void BindAutoriPage()
    {
        int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredAutori.Count / (double)PageSize));
        _autoriPage = Math.Clamp(_autoriPage, 1, totalPages);

        dgAutori.ItemsSource = _filteredAutori
            .Skip((_autoriPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    private void BindKnjigePage()
    {
        int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredKnjige.Count / (double)PageSize));
        _knjigePage = Math.Clamp(_knjigePage, 1, totalPages);

        dgKnjige.ItemsSource = _filteredKnjige
            .Skip((_knjigePage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    private void UpdateStatusTabName()
    {
        if (mainTabControl.SelectedItem is TabItem selectedTab)
        {
            string tabName = selectedTab.Header?.ToString() ?? string.Empty;
            txtStatusApp.Text = LocalizationManager.Get("StatusFormat", tabName);
        }
        else
        {
            txtStatusApp.Text = LocalizationManager.Get("AppTitle");
        }
    }

    private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.Source == mainTabControl)
        {
            UpdateStatusTabName();
            UpdatePaginationControls();
        }
    }

    // --- LOKALIZACIJA (#lokalizacija) ---
    private void MenuLangSr_Click(object sender, RoutedEventArgs e)
    {
        LocalizationManager.SetLanguage(AppLanguage.Srpski);
        menuLangSr.IsChecked = true;
        menuLangEn.IsChecked = false;
    }

    private void MenuLangEn_Click(object sender, RoutedEventArgs e)
    {
        LocalizationManager.SetLanguage(AppLanguage.English);
        menuLangSr.IsChecked = false;
        menuLangEn.IsChecked = true;
    }

    public void ApplyLocalization()
    {
        Title = LocalizationManager.Get("AppTitle");

        menuFile.Header = LocalizationManager.Get("MenuFile");
        menuNew.Header = LocalizationManager.Get("MenuNew");
        menuSave.Header = LocalizationManager.Get("MenuSave");
        menuOpen.Header = LocalizationManager.Get("MenuOpen");
        menuClose.Header = LocalizationManager.Get("MenuClose");
        menuEdit.Header = LocalizationManager.Get("MenuEdit");
        menuDelete.Header = LocalizationManager.Get("MenuDelete");
        menuHelp.Header = LocalizationManager.Get("MenuHelp");
        menuAbout.Header = LocalizationManager.Get("MenuAbout");
        menuLanguage.Header = LocalizationManager.Get("MenuLanguage");
        menuLangSr.Header = LocalizationManager.Get("LangSerbian");
        menuLangEn.Header = LocalizationManager.Get("LangEnglish");

        btnAdd.ToolTip = LocalizationManager.Get("TooltipAdd");
        btnEdit.ToolTip = LocalizationManager.Get("TooltipEdit");
        btnDelete.ToolTip = LocalizationManager.Get("TooltipDelete");
        txtSearch.ToolTip = LocalizationManager.Get("TooltipSearchInput");
        btnSearch.ToolTip = LocalizationManager.Get("TooltipSearchBtn");

        tabPosetioci.Header = LocalizationManager.Get("TabVisitors");
        tabAutori.Header = LocalizationManager.Get("TabAuthors");
        tabKnjige.Header = LocalizationManager.Get("TabBooks");
        menuIzdavaci.Header = LocalizationManager.Get("TabPublishers");

        if (dgAutori.Columns.Count >= 5)
        {
            dgAutori.Columns[0].Header = LocalizationManager.Get("ColFirstName");
            dgAutori.Columns[1].Header = LocalizationManager.Get("ColLastName");
            dgAutori.Columns[2].Header = LocalizationManager.Get("ColIdCard");
            dgAutori.Columns[3].Header = LocalizationManager.Get("ColBirthDate");
            dgAutori.Columns[4].Header = LocalizationManager.Get("ColEmail");
        }

        if (dgKnjige.Columns.Count >= 5)
        {
            dgKnjige.Columns[0].Header = LocalizationManager.Get("ColIsbn");
            dgKnjige.Columns[1].Header = LocalizationManager.Get("ColTitle");
            dgKnjige.Columns[2].Header = LocalizationManager.Get("ColPrice");
            dgKnjige.Columns[3].Header = LocalizationManager.Get("ColYear");
            dgKnjige.Columns[4].Header = LocalizationManager.Get("ColGenre");
        }

        btnPrevPage.Content = LocalizationManager.Get("PrevPage");
        btnNextPage.Content = LocalizationManager.Get("NextPage");

        UpdateStatusTabName();
        UpdatePaginationControls();
    }

    // --- DODAVANJE (#dodavanje_autora, #dodavanje_knjige) ---
    private void ToolbarAdd_Click(object sender, RoutedEventArgs e)
    {
        if (mainTabControl.SelectedItem == tabAutori)
        {
            AddAutorDialog();
        }
        else if (mainTabControl.SelectedItem == tabKnjige)
        {
            AddKnjigaDialog();
        }
    }

    private void AddAutorDialog()
    {
        var dialog = new DodajAutoraWindow
        {
            Owner = this
        };

        if (dialog.ShowDialog() == true && dialog.KreiraniAutor != null)
        {
            if (dialog.KreiranaAdresa != null)
            {
                _adresaService.Add(dialog.KreiranaAdresa);
            }

            _autorService.Add(dialog.KreiraniAutor);
            ApplySearchAndSort();
        }
    }

    private void AddKnjigaDialog()
    {
        var dialog = new DodajKnjiguWindow
        {
            Owner = this
        };

        if (dialog.ShowDialog() == true && dialog.KreiranaKnjiga != null)
        {
            _knjigaService.Add(dialog.KreiranaKnjiga);
            ApplySearchAndSort();
        }
    }

    // --- IZMENA (#izmena_autora, #izmena_knjige) ---
    private void ToolbarEdit_Click(object sender, RoutedEventArgs e)
    {
        if (mainTabControl.SelectedItem == tabAutori)
        {
            if (dgAutori.SelectedItem is not Autor selectedAutor)
            {
                MessageBox.Show(this, LocalizationManager.Get("SelectAuthorWarning"), LocalizationManager.Get("MenuEdit"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(this, $"{selectedAutor.Ime} {selectedAutor.Prezime}", LocalizationManager.Get("MenuEdit"), MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else if (mainTabControl.SelectedItem == tabKnjige)
        {
            if (dgKnjige.SelectedItem is not Knjiga selectedKnjiga)
            {
                MessageBox.Show(this, LocalizationManager.Get("SelectBookWarning"), LocalizationManager.Get("MenuEdit"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(this, selectedKnjiga.Naziv, LocalizationManager.Get("MenuEdit"), MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // --- BRISANJE (#brisanje_autora, #brisanje_knjige) ---
    private void ToolbarDelete_Click(object sender, RoutedEventArgs e)
    {
        if (mainTabControl.SelectedItem == tabAutori)
        {
            DeleteSelectedAutor();
        }
        else if (mainTabControl.SelectedItem == tabKnjige)
        {
            DeleteSelectedKnjiga();
        }
    }

    private void DeleteSelectedAutor()
    {
        if (dgAutori.SelectedItem is not Autor selectedAutor)
        {
            MessageBox.Show(this, LocalizationManager.Get("SelectAuthorWarning"), LocalizationManager.Get("DeleteAuthorTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string fullName = $"{selectedAutor.Ime} {selectedAutor.Prezime}";
        var result = MessageBox.Show(
            this,
            LocalizationManager.Get("DeleteAuthorConfirm", fullName),
            LocalizationManager.Get("DeleteAuthorTitle"),
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _autorService.Delete(selectedAutor.Id);
            ApplySearchAndSort();
        }
    }

    private void DeleteSelectedKnjiga()
    {
        if (dgKnjige.SelectedItem is not Knjiga selectedKnjiga)
        {
            MessageBox.Show(this, LocalizationManager.Get("SelectBookWarning"), LocalizationManager.Get("DeleteBookTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show(
            this,
            LocalizationManager.Get("DeleteBookConfirm", selectedKnjiga.Naziv),
            LocalizationManager.Get("DeleteBookTitle"),
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _knjigaService.Delete(selectedKnjiga.Id);
            ApplySearchAndSort();
        }
    }

    // --- PRETRAGA (#pretraga_autora, #pretraga_knjiga) ---
    private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSearch.Text))
        {
            if (mainTabControl.SelectedItem == tabAutori)
                _autoriPage = 1;
            else if (mainTabControl.SelectedItem == tabKnjige)
                _knjigePage = 1;

            ApplySearchAndSort();
        }
    }

    private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ToolbarSearch_Click(sender, e);
            e.Handled = true;
        }
    }

    private void ToolbarSearch_Click(object sender, RoutedEventArgs e)
    {
        if (mainTabControl.SelectedItem == tabAutori)
        {
            _autoriPage = 1;
        }
        else if (mainTabControl.SelectedItem == tabKnjige)
        {
            _knjigePage = 1;
        }

        ApplySearchAndSort();
    }

    private void ApplySearchAndSort()
    {
        string query = txtSearch.Text.Trim();

        var autori = FilterAutori(query, _autorService.GetAll());
        _filteredAutori = SortAutori(autori, _autoriSortMember, _autoriSortDirection);

        var knjige = FilterKnjige(query, _knjigaService.GetAll());
        _filteredKnjige = SortKnjige(knjige, _knjigeSortMember, _knjigeSortDirection);

        BindAutoriPage();
        BindKnjigePage();
        UpdatePaginationControls();
    }

    private static List<Autor> FilterAutori(string query, IEnumerable<Autor> source)
    {
        if (string.IsNullOrWhiteSpace(query))
            return source.ToList();

        var parts = query.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 0)
            return source.ToList();

        if (parts.Length == 1)
        {
            string prezime = parts[0];
            return source
                .Where(a => a.Prezime.Contains(prezime, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        string prezimeFilter = parts[0];
        string imeFilter = parts[1];

        return source
            .Where(a =>
                a.Prezime.Contains(prezimeFilter, StringComparison.OrdinalIgnoreCase) &&
                a.Ime.Contains(imeFilter, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private static List<Knjiga> FilterKnjige(string query, IEnumerable<Knjiga> source)
    {
        if (string.IsNullOrWhiteSpace(query))
            return source.ToList();

        return source
            .Where(k =>
                k.Naziv.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                k.ISBN.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    // --- SORTIRANJE (#sortiranje_autora, #sortiranje_knjiga) ---
    private static List<Autor> SortAutori(List<Autor> list, string? sortMember, ListSortDirection direction)
    {
        if (string.IsNullOrWhiteSpace(sortMember))
            return list;

        IEnumerable<Autor> ordered = sortMember switch
        {
            "Ime" => direction == ListSortDirection.Ascending
                ? list.OrderBy(a => a.Ime, StringComparer.CurrentCultureIgnoreCase)
                : list.OrderByDescending(a => a.Ime, StringComparer.CurrentCultureIgnoreCase),
            "Prezime" => direction == ListSortDirection.Ascending
                ? list.OrderBy(a => a.Prezime, StringComparer.CurrentCultureIgnoreCase)
                : list.OrderByDescending(a => a.Prezime, StringComparer.CurrentCultureIgnoreCase),
            "Email" => direction == ListSortDirection.Ascending
                ? list.OrderBy(a => a.Email, StringComparer.CurrentCultureIgnoreCase)
                : list.OrderByDescending(a => a.Email, StringComparer.CurrentCultureIgnoreCase),
            _ => list
        };

        return ordered.ToList();
    }

    private static List<Knjiga> SortKnjige(List<Knjiga> list, string? sortMember, ListSortDirection direction)
    {
        if (string.IsNullOrWhiteSpace(sortMember))
            return list;

        IEnumerable<Knjiga> ordered = sortMember switch
        {
            "ISBN" => direction == ListSortDirection.Ascending
                ? list.OrderBy(k => k.ISBN, StringComparer.OrdinalIgnoreCase)
                : list.OrderByDescending(k => k.ISBN, StringComparer.OrdinalIgnoreCase),
            "Naziv" => direction == ListSortDirection.Ascending
                ? list.OrderBy(k => k.Naziv, StringComparer.CurrentCultureIgnoreCase)
                : list.OrderByDescending(k => k.Naziv, StringComparer.CurrentCultureIgnoreCase),
            "Cena" => direction == ListSortDirection.Ascending
                ? list.OrderBy(k => k.Cena)
                : list.OrderByDescending(k => k.Cena),
            "GodinaIzdanja" => direction == ListSortDirection.Ascending
                ? list.OrderBy(k => k.GodinaIzdanja)
                : list.OrderByDescending(k => k.GodinaIzdanja),
            "Zanr" => direction == ListSortDirection.Ascending
                ? list.OrderBy(k => k.Zanr, StringComparer.CurrentCultureIgnoreCase)
                : list.OrderByDescending(k => k.Zanr, StringComparer.CurrentCultureIgnoreCase),
            _ => list
        };

        return ordered.ToList();
    }

    private void DgAutori_Sorting(object sender, DataGridSortingEventArgs e)
    {
        e.Handled = true;
        string? sortMember = e.Column.SortMemberPath;
        if (string.IsNullOrWhiteSpace(sortMember)) return;

        if (_autoriSortMember == sortMember)
        {
            _autoriSortDirection = _autoriSortDirection == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;
        }
        else
        {
            _autoriSortMember = sortMember;
            _autoriSortDirection = ListSortDirection.Ascending;
        }

        e.Column.SortDirection = _autoriSortDirection;
        _autoriPage = 1;
        ApplySearchAndSort();
    }

    private void DgKnjige_Sorting(object sender, DataGridSortingEventArgs e)
    {
        e.Handled = true;
        string? sortMember = e.Column.SortMemberPath;
        if (string.IsNullOrWhiteSpace(sortMember)) return;

        if (_knjigeSortMember == sortMember)
        {
            _knjigeSortDirection = _knjigeSortDirection == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;
        }
        else
        {
            _knjigeSortMember = sortMember;
            _knjigeSortDirection = ListSortDirection.Ascending;
        }

        e.Column.SortDirection = _knjigeSortDirection;
        _knjigePage = 1;
        ApplySearchAndSort();
    }

    // --- PAGINACIJA (do 16 entiteta po strani) ---
    private void UpdatePaginationControls()
    {
        if (mainTabControl.SelectedItem == tabAutori)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredAutori.Count / (double)PageSize));
            txtPageInfo.Text = LocalizationManager.Get("PageFormat", _autoriPage, totalPages);
            btnPrevPage.IsEnabled = _autoriPage > 1;
            btnNextPage.IsEnabled = _autoriPage < totalPages;
        }
        else if (mainTabControl.SelectedItem == tabKnjige)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredKnjige.Count / (double)PageSize));
            txtPageInfo.Text = LocalizationManager.Get("PageFormat", _knjigePage, totalPages);
            btnPrevPage.IsEnabled = _knjigePage > 1;
            btnNextPage.IsEnabled = _knjigePage < totalPages;
        }
    }

    private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
    {
        if (mainTabControl.SelectedItem == tabAutori && _autoriPage > 1)
        {
            _autoriPage--;
            BindAutoriPage();
        }
        else if (mainTabControl.SelectedItem == tabKnjige && _knjigePage > 1)
        {
            _knjigePage--;
            BindKnjigePage();
        }
        UpdatePaginationControls();
    }

    private void BtnNextPage_Click(object sender, RoutedEventArgs e)
    {
        if (mainTabControl.SelectedItem == tabAutori)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredAutori.Count / (double)PageSize));
            if (_autoriPage < totalPages)
            {
                _autoriPage++;
                BindAutoriPage();
            }
        }
        else if (mainTabControl.SelectedItem == tabKnjige)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredKnjige.Count / (double)PageSize));
            if (_knjigePage < totalPages)
            {
                _knjigePage++;
                BindKnjigePage();
            }
        }
        UpdatePaginationControls();
    }

    // --- MENU BAR ---
    private void MenuNew_Click(object sender, RoutedEventArgs e) => ToolbarAdd_Click(sender, e);
    private void MenuSave_Click(object sender, RoutedEventArgs e) => MessageBox.Show(this, LocalizationManager.Get("SavedMessage"), LocalizationManager.Get("MenuSave"), MessageBoxButton.OK, MessageBoxImage.Information);
    private void MenuClose_Click(object sender, RoutedEventArgs e) => Close();
    private void MenuEdit_Click(object sender, RoutedEventArgs e) => ToolbarEdit_Click(sender, e);
    private void MenuDelete_Click(object sender, RoutedEventArgs e) => ToolbarDelete_Click(sender, e);
    private void MenuAbout_Click(object sender, RoutedEventArgs e) => MessageBox.Show(this, LocalizationManager.Get("AboutMessage"), LocalizationManager.Get("MenuAbout"), MessageBoxButton.OK, MessageBoxImage.Information);

    private void TabPosetioci_Click(object sender, RoutedEventArgs e) => mainTabControl.SelectedItem = tabPosetioci;
    private void TabAutori_Click(object sender, RoutedEventArgs e) => mainTabControl.SelectedItem = tabAutori;
    private void TabKnjige_Click(object sender, RoutedEventArgs e) => mainTabControl.SelectedItem = tabKnjige;
    private void MenuIzdavaci_Click(object sender, RoutedEventArgs e) => MessageBox.Show(this, LocalizationManager.Get("TabPublishers"), LocalizationManager.Get("TabPublishers"), MessageBoxButton.OK, MessageBoxImage.Information);

    private void DgAutori_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (dgAutori.SelectedItem is Autor)
        {
            ToolbarEdit_Click(sender, e);
        }
    }

    private void DgKnjige_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (dgKnjige.SelectedItem is Knjiga)
        {
            ToolbarEdit_Click(sender, e);
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        LocalizationManager.LanguageChanged -= ApplyLocalization;
        _timer.Stop();
        base.OnClosed(e);
    }
}