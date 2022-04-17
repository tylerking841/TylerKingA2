using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Pages.Advertisements
{
    public class IndexModel : PageModel
    {
        private readonly Lab4.Data.MarketDbContext _context;

        public IndexModel(Lab4.Data.MarketDbContext context)
        {
            _context = context;
        }


        public IList<Advertisement> Advertisement { get;set; }

        [BindProperty]
        public string broker { get; set; }
        public async Task OnGetAsync(string? Id)
        {
            broker = Id;
            Advertisement = await _context.Advertisements.ToListAsync();
        }
    }
}
