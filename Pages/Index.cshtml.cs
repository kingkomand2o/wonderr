using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wonderr.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(wonderr.AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var developers = _context.Developers.OrderByDescending(x => x.Id).Take(3);
        }
    }
}