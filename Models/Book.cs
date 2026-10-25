using SQLite;

namespace BiblioCampus.Models;

public class Book
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int TotalCopies { get; set; } = 1;
    public int AvailableCopies { get; set; } = 1;

    [Ignore]
    public bool IsAvailable => AvailableCopies > 0;

    [Ignore]
    public string AvailabilityText => IsAvailable
        ? $"Disponible ({AvailableCopies}/{TotalCopies})"
        : "Indisponible";
}
