using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Carasec_Daria_Lab2.Models;
using Carasec_Daria_Lab2.Data;

namespace Carasec_Daria_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Carasec_Daria_Lab2Context _context;

    public IndexModel(Carasec_Daria_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Category = await _context.Category.ToListAsync();
    }
}
