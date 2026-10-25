using BiblioCampus.Models;
using BiblioCampus.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BiblioCampus.ViewModels;

public partial class AddBookViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public AddBookViewModel(DatabaseService db) => _db = db;

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string author = string.Empty;
    [ObservableProperty] private string isbn = string.Empty;
    [ObservableProperty] private string category = string.Empty;
    [ObservableProperty] private string copies = "1";

    [RelayCommand]
    private async Task SaveAsync()
    {
        var page = Shell.Current.CurrentPage;

        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Author))
        {
            await page.DisplayAlertAsync("Champs requis", "Le titre et l'auteur sont obligatoires.", "OK");
            return;
        }
        if (!int.TryParse(Copies, out var n) || n < 1)
        {
            await page.DisplayAlertAsync("Erreur", "Le nombre d'exemplaires doit être un entier ≥ 1.", "OK");
            return;
        }

        await _db.AddBookAsync(new Book
        {
            Title = Title.Trim(),
            Author = Author.Trim(),
            Isbn = Isbn.Trim(),
            Category = Category.Trim(),
            TotalCopies = n
        });

        Title = Author = Isbn = Category = string.Empty;
        Copies = "1";
        await page.DisplayAlertAsync("Succès", "Livre ajouté au catalogue.", "OK");
    }
}
