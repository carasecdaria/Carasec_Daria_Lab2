
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

    [BindProperty(SupportsGet = true)]
    public int? AuthorID { get; set; }

    public SelectList Authors { get; set; } = default!;

    public async Task OnGetAsync()
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

        Authors = new SelectList(
            authors,
            "ID",
            "FullName",
            AuthorID
        );

        var query = _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .AsQueryable();

        if (AuthorID.HasValue)
        {
            query = query.Where(b => b.AuthorID == AuthorID.Value);
        }

        Book = await query.ToListAsync();
    }
}
