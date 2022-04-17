using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;
using Azure;
using Lab4.Data;
using Lab4.Models;
namespace Lab4.Pages.Advertisements
{
    public class CreateModel : PageModel
    {
        private readonly MarketDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string adContainer = "ads";

        public CreateModel(MarketDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Advertisement Advertisement { get; set; }

       
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            BlobContainerClient containerClient;


            try
            {

                containerClient = await _blobServiceClient.CreateBlobContainerAsync(adContainer);
               
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(adContainer);
            }

            try
            {
                string fileName = Path.GetRandomFileName();
               
                var blockBlob = containerClient.GetBlobClient(fileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }



                _context.Advertisements.Add(new Advertisement { FileName = blockBlob.Name, Url = containerClient.GetBlobClient(blockBlob.Name).Uri.AbsoluteUri });

            }
            catch (RequestFailedException)
            {
                return RedirectToPage("Error");
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
