using BiblioCampus.ViewModels;

namespace BiblioCampus.Views;

public partial class LoansPage : ContentPage
{
    private readonly LoansViewModel _vm;

    public LoansPage(LoansViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}
