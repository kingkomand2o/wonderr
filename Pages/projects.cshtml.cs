using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace wonderr.Pages
{
    public class projectsModel : PageModel
    {
        private readonly AppDbContext _context;

        public projectsModel(AppDbContext context)
        {
            _context = context;
        }

        // Properties to hold data for the page
        public List<Project> Projects { get; set; } = new();
        public List<Developer> Developers { get; set; } = new();

        public List<Property> Properties { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Fetch data from the database
            Projects = await _context.Projects
                .ToListAsync();

            Developers = await _context.Developers.ToListAsync();

            Properties = await _context.Properties.ToListAsync();

            return Page();
        }
    }
}
