using Carasec_Daria_Lab2.Data;
using Carasec_Daria_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Carasec_Daria_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Carasec_Daria_Lab2Context _context;

    public IndexModel(Carasec_Daria_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = new List<Book>();
    public BookData BookD { get; set; } = new BookData();
    public int BookID { get; set; }
    public int CategoryID { get; set; }

    // laboratorul 4 - sortare si cautare
    public string TitleSort { get; set; }
    public string AuthorSort { get; set; }
    public string CurrentFilter { get; set; }

    // laboratorul 2 - filtru dupa autor
    [BindProperty(SupportsGet = true)]
    public int? AuthorID { get; set; }
    public SelectList Authors { get; set; } = default!;

    public async Task OnGetAsync(int? id, int? categoryID, string? sortOrder, string? searchString)
    {
        var authors = await _context.Author
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            })
            .ToListAsync();
        Authors = new SelectList(authors, "ID", "FullName", AuthorID);

        // pasul 9 - valorile pentru linkurile de sortare
        TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        AuthorSort = sortOrder == "author" ? "author_desc" : "author";

        // pasul 12 - textul cautat
        CurrentFilter = searchString;

        var query = _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(b => b.Category)
            .AsNoTracking()
            .AsQueryable();

        if (AuthorID.HasValue)
        {
            query = query.Where(b => b.AuthorID == AuthorID.Value);
        }

        if (!String.IsNullOrEmpty(searchString))
        {
            query = query.Where(s => s.Author.FirstName.Contains(searchString)
                                  || s.Author.LastName.Contains(searchString)
                                  || s.Title.Contains(searchString));
        }

        BookD.Books = await query.ToListAsync();

        if (id != null)
        {
            BookID = id.Value;
            Book? book = BookD.Books.FirstOrDefault(i => i.ID == id.Value);
            if (book != null)
            {
                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }
        }

        // pasul 9 - sortarea
        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Title);
                break;
            case "author_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Author?.FullName);
                break;
            case "author":
                BookD.Books = BookD.Books.OrderBy(s => s.Author?.FullName);
                break;
            default:
                BookD.Books = BookD.Books.OrderBy(s => s.Title);
                break;
        }

        Book = BookD.Books.ToList();
    }
}