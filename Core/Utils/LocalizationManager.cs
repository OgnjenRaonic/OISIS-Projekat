using System;
using System.Collections.Generic;

namespace WpfClient.Utils;

public enum AppLanguage
{
    Srpski,
    English
}

public static class LocalizationManager
{
    public static event Action? LanguageChanged;

    public static AppLanguage CurrentLanguage { get; private set; } = AppLanguage.Srpski;

    private static readonly Dictionary<string, (string Sr, string En)> Translations = new()
    {
        // Glavni prozor i meni
        ["AppTitle"] = ("Sajam knjiga", "Book Fair"),
        ["MenuFile"] = ("_Fajl", "_File"),
        ["MenuNew"] = ("_Novo", "_New"),
        ["MenuSave"] = ("_Sačuvaj", "_Save"),
        ["MenuOpen"] = ("_Otvori", "_Open"),
        ["MenuClose"] = ("_Zatvori", "_Close"),
        ["MenuEdit"] = ("_Izmena", "_Edit"),
        ["MenuDelete"] = ("_Brisanje", "_Delete"),
        ["MenuHelp"] = ("_Pomoć", "_Help"),
        ["MenuAbout"] = ("_O aplikaciji", "_About"),
        ["MenuLanguage"] = ("_Jezik", "_Language"),
        ["LangSerbian"] = ("Srpski", "Serbian"),
        ["LangEnglish"] = ("Engleski", "English"),

        // Toolbar i pretraga
        ["TooltipAdd"] = ("Dodaj novi entitet (Ctrl+N)", "Add new entity (Ctrl+N)"),
        ["TooltipEdit"] = ("Izmeni selektovani entitet (Ctrl+E)", "Edit selected entity (Ctrl+E)"),
        ["TooltipDelete"] = ("Obriši selektovani entitet (Ctrl+Del)", "Delete selected entity (Ctrl+Del)"),
        ["TooltipSearchInput"] = ("Unesite kriterijum pretrage", "Enter search query"),
        ["TooltipSearchBtn"] = ("Pretraži", "Search"),

        // Tabovi i paginacija
        ["TabVisitors"] = ("Posetioci", "Visitors"),
        ["TabAuthors"] = ("Autori", "Authors"),
        ["TabBooks"] = ("Knjige", "Books"),
        ["TabPublishers"] = ("Izdavači", "Publishers"),
        ["PrevPage"] = ("◀ Prethodna", "◀ Previous"),
        ["NextPage"] = ("Sledeća ▶", "Next ▶"),
        ["PageFormat"] = ("Strana {0} / {1}", "Page {0} / {1}"),
        ["StatusFormat"] = ("Sajam knjiga - {0}", "Book Fair - {0}"),

        // Kolone tabela
        ["ColFirstName"] = ("Ime", "First Name"),
        ["ColLastName"] = ("Prezime", "Last Name"),
        ["ColIdCard"] = ("Broj lične karte", "ID Card Number"),
        ["ColBirthDate"] = ("Datum rođenja", "Birth Date"),
        ["ColEmail"] = ("E-mejl", "Email"),
        ["ColIsbn"] = ("ISBN", "ISBN"),
        ["ColTitle"] = ("Naziv", "Title"),
        ["ColPrice"] = ("Cena", "Price"),
        ["ColYear"] = ("Godina izdanja", "Publication Year"),
        ["ColGenre"] = ("Žanr", "Genre"),

        // Dugmad i dijalog za brisanje
        ["BtnConfirm"] = ("Potvrdi", "Confirm"),
        ["BtnCancel"] = ("Odustani", "Cancel"),
        ["DeleteAuthorTitle"] = ("Brisanje autora", "Delete Author"),
        ["DeleteAuthorConfirm"] = ("Da li ste sigurni da želite da obrišete autora \"{0}\"?", "Are you sure you want to delete author \"{0}\"?"),
        ["DeleteBookTitle"] = ("Brisanje knjige", "Delete Book"),
        ["DeleteBookConfirm"] = ("Da li ste sigurni da želite da obrišete knjigu \"{0}\"?", "Are you sure you want to delete book \"{0}\"?"),
        ["SelectAuthorWarning"] = ("Prvo označite autora za brisanje.", "Please select an author to delete first."),
        ["SelectBookWarning"] = ("Prvo označite knjigu za brisanje.", "Please select a book to delete first."),
        ["SavedMessage"] = ("Stanje sačuvano.", "State saved."),
        ["AboutMessage"] = ("Sajam knjiga v1.0\nOISISI Projekat", "Book Fair v1.0\nOISISI Project")
    };

    public static void SetLanguage(AppLanguage language)
    {
        if (CurrentLanguage == language)
            return;

        CurrentLanguage = language;
        LanguageChanged?.Invoke();
    }

    public static string Get(string key, params object[] args)
    {
        if (!Translations.TryGetValue(key, out var pair))
            return key;

        string text = CurrentLanguage == AppLanguage.Srpski ? pair.Sr : pair.En;
        return args.Length > 0 ? string.Format(text, args) : text;
    }
}