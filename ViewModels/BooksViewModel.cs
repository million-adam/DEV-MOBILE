using System.Collections.ObjectModel;
using BiblioCampus.Models;
using BiblioCampus.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BiblioCampus.ViewModels;

public partial class BooksViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public BooksViewModel(DatabaseService db) => _db = db;

    public ObservableCollection<Book> Books { get; } = new();

    [ObservableProperty] private string searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => _ = LoadAsync();

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _db.GetBooksAsync(SearchText);
        Books.Clear();
        foreach (var b in items) Books.Add(b);
    }

    [RelayCommand]
    private async Task BorrowAsync(Book? book)
    {
        if (book is null) return;
        var page = Shell.Current.CurrentPage;

        if (!book.IsAvailable)
        {
            await page.DisplayAlertAsync("Indisponible", "Tous les exemplaires sont empruntés.", "OK");
            return;
        }

        var name = await page.DisplayPromptAsync("Emprunter", $"« {book.Title} »\nNom de l'étudiant :", "Valider", "Annuler");
        if (string.IsNullOrWhiteSpace(name)) return;

        var (ok, message) = await _db.BorrowAsync(book.Id, name);
        await page.DisplayAlertAsync(ok ? "Succès" : "Erreur", message, "OK");
        await LoadAsync();
    }
}
