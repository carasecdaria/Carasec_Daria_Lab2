
using Carasec_Daria_Lab2.Data;
using Carasec_Daria_Lab2.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Carasec_Daria_Lab2.Pages.PublisherPages;

public class IndexModel : PageModel
{
    private readonly Carasec_Daria_Lab2Context _context;

    public IndexModel(Carasec_Daria_Lab2Context context)
    {
        _context = context;
    }

    public IList<Publisher> Publisher { get; set; } = new List<Publisher>();

    public async Task OnGetAsync()
    {
        Publisher = await _context.Publisher
            .Include(p => p.Books)
            .ToListAsync();
    }
}
