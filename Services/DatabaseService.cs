using BiblioCampus.Models;
using SQLite;

namespace BiblioCampus.Services;

public class DatabaseService
{
    private const int LoanDays = 14;
    private SQLiteAsyncConnection? _db;

    private async Task<SQLiteAsyncConnection> GetDbAsync()
    {
        if (_db is not null) return _db;

        var path = Path.Combine(FileSystem.AppDataDirectory, "bibliocampus.db3");
        var db = new SQLiteAsyncConnection(path);
        await db.CreateTableAsync<Book>();
        await db.CreateTableAsync<Loan>();

        if (await db.Table<Book>().CountAsync() == 0)
            await SeedAsync(db);

        _db = db;
        return _db;
    }

    private static Task SeedAsync(SQLiteAsyncConnection db) => db.InsertAllAsync(new[]
    {
        new Book { Title = "Génie logiciel", Author = "Ian Sommerville", Isbn = "9782744070747", Category = "Informatique", TotalCopies = 3, AvailableCopies = 3 },
        new Book { Title = "Clean Code", Author = "Robert C. Martin", Isbn = "9780132350884", Category = "Informatique", TotalCopies = 2, AvailableCopies = 2 },
        new Book { Title = "Algorithmique", Author = "Thomas H. Cormen", Isbn = "9782100545261", Category = "Informatique", TotalCopies = 2, AvailableCopies = 2 },
        new Book { Title = "Bases de données", Author = "Georges Gardarin", Isbn = "9782212112818", Category = "Informatique", TotalCopies = 1, AvailableCopies = 1 },
        new Book { Title = "Principes d'économie", Author = "N. Gregory Mankiw", Isbn = "9782807308701", Category = "Économie", TotalCopies = 2, AvailableCopies = 2 },
    });

    public async Task<List<Book>> GetBooksAsync(string? search = null)
    {
        var db = await GetDbAsync();
        if (string.IsNullOrWhiteSpace(search))
            return await db.Table<Book>().OrderBy(b => b.Title).ToListAsync();

        var term = $"%{search.Trim()}%";
        return await db.QueryAsync<Book>(
            "SELECT * FROM Book WHERE Title LIKE ? OR Author LIKE ? OR Isbn LIKE ? OR Category LIKE ? ORDER BY Title",
            term, term, term, term);
    }

    public async Task AddBookAsync(Book book)
    {
        var db = await GetDbAsync();
        book.AvailableCopies = book.TotalCopies;
        await db.InsertAsync(book);
    }

    public async Task<(bool Ok, string Message)> BorrowAsync(int bookId, string studentName)
    {
        var db = await GetDbAsync();
        var book = await db.FindAsync<Book>(bookId);
        if (book is null) return (false, "Livre introuvable.");
        if (book.AvailableCopies <= 0) return (false, "Aucun exemplaire disponible.");

        var loan = new Loan
        {
            BookId = book.Id,
            BookTitle = book.Title,
            StudentName = studentName.Trim(),
            LoanDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(LoanDays)
        };

        await db.RunInTransactionAsync(conn =>
        {
            conn.Insert(loan);
            book.AvailableCopies--;
            conn.Update(book);
        });

        return (true, $"Emprunt enregistré. Retour avant le {loan.DueDate:dd/MM/yyyy}.");
    }

    public async Task<List<Loan>> GetActiveLoansAsync()
    {
        var db = await GetDbAsync();
        return await db.Table<Loan>()
            .Where(l => l.ReturnDate == null)
            .OrderBy(l => l.DueDate)
            .ToListAsync();
    }

    public async Task ReturnAsync(Loan loan)
    {
        var db = await GetDbAsync();
        await db.RunInTransactionAsync(conn =>
        {
            loan.ReturnDate = DateTime.Now;
            conn.Update(loan);

            var book = conn.Find<Book>(loan.BookId);
            if (book is not null)
            {
                book.AvailableCopies = Math.Min(book.TotalCopies, book.AvailableCopies + 1);
                conn.Update(book);
            }
        });
    }
}
