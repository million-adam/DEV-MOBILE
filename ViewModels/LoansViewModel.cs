using System.Collections.ObjectModel;
using BiblioCampus.Models;
using BiblioCampus.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BiblioCampus.ViewModels;

public partial class LoansViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public LoansViewModel(DatabaseService db) => _db = db;

    public ObservableCollection<Loan> Loans { get; } = new();

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _db.GetActiveLoansAsync();
        Loans.Clear();
        foreach (var l in items) Loans.Add(l);
    }

    [RelayCommand]
    private async Task ReturnAsync(Loan? loan)
    {
        if (loan is null) return;
        var page = Shell.Current.CurrentPage;

        var confirm = await page.DisplayAlertAsync("Retour", $"Enregistrer le retour de « {loan.BookTitle} » ?", "Oui", "Non");
        if (!confirm) return;

        await _db.ReturnAsync(loan);
        await LoadAsync();
    }
}
