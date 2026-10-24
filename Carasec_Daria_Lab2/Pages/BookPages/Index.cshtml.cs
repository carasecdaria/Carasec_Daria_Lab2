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

    [BindProperty(SupportsGet = true)]
    public int? AuthorID { get; set; }

    public SelectList Authors { get; set; } = default!;

    public async Task OnGetAsync(int? id, int? categoryID)
    {
        // lista de autori pentru filtru (de la laboratorul 2)
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

        // cartile, impreuna cu autorul, editura si categoriile
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

        BookD.Books = await query.OrderBy(b => b.Title).ToListAsync();
        Book = BookD.Books.ToList();

        if (id != null)
        {
            BookID = id.Value;
            Book book = BookD.Books.Where(i => i.ID == id.Value).Single();
            BookD.Categories = book.BookCategories.Select(s => s.Category);
        }
    }
}