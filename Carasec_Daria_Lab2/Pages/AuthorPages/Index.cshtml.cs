using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Carasec_Daria_Lab2.Models;
using Carasec_Daria_Lab2.Data;

namespace Carasec_Daria_Lab2.Pages.AuthorPages;

public class IndexModel : PageModel
{
    private readonly Carasec_Daria_Lab2Context _context;

    public IndexModel(Carasec_Daria_Lab2Context context)
    {
        _context = context;
    }

    public IList<Author> Author { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Author = await _context.Author.ToListAsync();
    }
}
