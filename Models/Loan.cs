using SQLite;

namespace BiblioCampus.Models;

public class Loan
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    [Ignore]
    public bool IsActive => ReturnDate is null;

    [Ignore]
    public bool IsOverdue => IsActive && DueDate.Date < DateTime.Today;

    [Ignore]
    public string StatusText => IsOverdue ? "En retard" : "En cours";
}
