using BiblioCampus.ViewModels;

namespace BiblioCampus.Views;

public partial class BooksPage : ContentPage
{
    private readonly BooksViewModel _vm;

    public BooksPage(BooksViewModel vm)
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
