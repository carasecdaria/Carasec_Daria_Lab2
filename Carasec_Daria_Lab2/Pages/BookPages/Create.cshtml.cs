
using Carasec_Daria_Lab2.Data;
using Carasec_Daria_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Carasec_Daria_Lab2.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly Carasec_Daria_Lab2Context _context;

    public CreateModel(Carasec_Daria_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDropdownsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task LoadDropdownsAsync()
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

        ViewData["AuthorID"] = new SelectList(
            authors,
            "ID",
            "FullName",
            Book?.AuthorID
        );

        ViewData["PublisherID"] = new SelectList(
            await _context.Publisher.ToListAsync(),
            "ID",
            "PublisherName",
            Book?.PublisherID
        );
    }
}
