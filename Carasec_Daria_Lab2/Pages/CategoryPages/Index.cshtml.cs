using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Carasec_Daria_Lab2.Models;
using Carasec_Daria_Lab2.ViewModels;

namespace Carasec_Daria_Lab2.Pages.CategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly Carasec_Daria_Lab2.Data.Carasec_Daria_Lab2Context _context;

        public IndexModel(Carasec_Daria_Lab2.Data.Carasec_Daria_Lab2Context context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;
        public CategoryIndexData CategoryData { get; set; } = default!;
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id)
        {
            CategoryData = new CategoryIndexData();

            // incarcam categoriile -> legaturile BookCategory -> cartile -> autorii
            CategoryData.Categories = await _context.Category
                .Include(c => c.BookCategories)
                    .ThenInclude(bc => bc.Book)
                        .ThenInclude(b => b.Author)
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            Category = CategoryData.Categories.ToList();

            if (id != null)
            {
                CategoryID = id.Value;
                Category category = CategoryData.Categories
                    .Where(c => c.ID == id.Value).Single();
                CategoryData.Books = category.BookCategories.Select(bc => bc.Book);
            }
        }
    }
}