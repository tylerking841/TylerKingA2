using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
{
    public class BrokeragesController : Controller
    {
        private readonly MarketDbContext _context;

        public BrokeragesController(MarketDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index(string? Id)
        {
            var viewModel = new BrokeragesViewModel
            {
                Brokerages = await _context.Brokerages
                .Include(i => i.Subscriptions)
                    .ThenInclude(i => i.Client)
                .AsNoTracking()
                .OrderBy(i => i.Title)
                .ToListAsync()
            };

            if (Id != null)
            {
                ViewData["BrokerageId"] = Id;
                viewModel.Subscriptions = viewModel.Brokerages.Where(
                    x => x.Id == Id).Single().Subscriptions;
            }

            return View(viewModel);
            
        }

   
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brokerage = await _context.Brokerages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brokerage == null)
            {
                return NotFound();
            }

            return View(brokerage);
        }

     
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Fee")] Brokerage brokerage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brokerage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brokerage);
        }

  
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brokerage = await _context.Brokerages.FindAsync(id);
            if (brokerage == null)
            {
                return NotFound();
            }
            return View(brokerage);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Fee")] Brokerage brokerage)
        {
            if (id != brokerage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brokerage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrokerageExists(brokerage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brokerage);
        }

    
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brokerage = await _context.Brokerages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brokerage == null)
            {
                return NotFound();
            }

            return View(brokerage);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var brokerage = await _context.Brokerages.FindAsync(id);
            _context.Brokerages.Remove(brokerage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrokerageExists(string id)
        {
            return _context.Brokerages.Any(e => e.Id == id);
        }
    }
}
