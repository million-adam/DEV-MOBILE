using BiblioCampus.Services;
using BiblioCampus.ViewModels;
using BiblioCampus.Views;
using Microsoft.Extensions.Logging;

namespace BiblioCampus;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<DatabaseService>();

        builder.Services.AddTransient<BooksViewModel>();
        builder.Services.AddTransient<LoansViewModel>();
        builder.Services.AddTransient<AddBookViewModel>();

        builder.Services.AddTransient<BooksPage>();
        builder.Services.AddTransient<LoansPage>();
        builder.Services.AddTransient<AddBookPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
