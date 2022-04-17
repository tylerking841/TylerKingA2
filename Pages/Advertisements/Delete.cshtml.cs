using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Pages.Advertisements
{
    public class DeleteModel : PageModel
    {
        private readonly Lab4.Data.MarketDbContext _context;

        public DeleteModel(Lab4.Data.MarketDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Advertisement Advertisement { get; set; }

        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //runs if id exists
            if (id != null)
            {
                Advertisement = await _context.Advertisements.FirstOrDefaultAsync(m => m.Id == id);
                if (Advertisement == null)
                {
                    return NotFound();
                }
                return Page();
            }
            return NotFound();
        }

        
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //runs if id exists
            if (id != null)
            {
                Advertisement = await _context.Advertisements.FindAsync(id);
                if (Advertisement != null)
                {
                    _context.Advertisements.Remove(Advertisement);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Index");
            }

            return NotFound();
        }
    }
}
