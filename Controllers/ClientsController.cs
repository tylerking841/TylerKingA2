using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MarketDbContext _context;
        public ClientsController(MarketDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index(string? Id)
        {
            var view = new ClientSubscriptionsViewModel
            {
                Clients = await _context.Clients
                .Include(i => i.Subscriptions)
                .ThenInclude(i => i.Brokerage)
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .ToListAsync()
            };

            if (Id != null)
            {
                ViewData["ClientId"] = Id;
                view.Subscriptions = view.Clients.Where(
                    x => x.Id.ToString() == Id).Single().Subscriptions;
            }
            

            return View(view);
    }

        public async Task<IActionResult> EditSubscriptions(int? Id)
        {

            var viewModelClient = new ClientSubscriptionsViewModel
            {
                        Clients = await _context.Clients
             .Include(i => i.Subscriptions)
             .ThenInclude(i => i.Brokerage)
             .AsNoTracking()
             .OrderBy(i => i.Id)
             .ToListAsync()
            };

            var viewModel = new BrokerageSubscriptionsViewModel
            {
                Brokerages = await _context.Brokerages
                .Include(i => i.Subscriptions)
                .ThenInclude(i => i.Client)
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .ToListAsync()

            };

            if (Id != null)
            {
                ViewData["ClientId"] = Id;
                viewModelClient.Subscriptions = viewModelClient.Clients.Where(
                    x => x.Id == Id).Single().Subscriptions;

                ViewData["FullName"] = viewModelClient.Clients.Where((n) => n.Id == Id).Single().FullName;
                viewModel.Subscriptions = viewModelClient.Subscriptions;

            }

            return View(viewModel);
        }

        public async Task<IActionResult> RemoveSubscription(string ?cId, string brokerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (cId == null || brokerId == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id.ToString() == cId);
            if (client == null)
            {
                return NotFound();
            }
 
            var subscription = await _context.Subscriptions.FindAsync(Int32.Parse(cId), brokerId);
            if (subscription == null)
            {
                return NotFound();
            }
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        public async Task<IActionResult> AddSubscriptions([Bind("ClientId, BrokerageId")] Subscription sub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Subscriptions.Add(sub);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       
        public IActionResult Create()
        {
            return View();
        }


    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Client person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Client person)
        {
           
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(person.Id))
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
            return View(person);
        }

     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(n => n.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(n => n.Id == id);
        }
    }
}
