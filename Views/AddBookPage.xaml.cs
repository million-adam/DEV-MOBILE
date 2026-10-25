using BiblioCampus.ViewModels;

namespace BiblioCampus.Views;

public partial class AddBookPage : ContentPage
{
    public AddBookPage(AddBookViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
