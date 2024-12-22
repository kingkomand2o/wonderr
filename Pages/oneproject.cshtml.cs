using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace wonderr.Pages
{
    public class oneprojectModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public oneprojectModel(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IList<Project> Projects { get; set; } = default!;

        public Project Project { get; set; }
        public Developer Developer { get; set; }
        public Property property { get; set; }
        public IList<Developer> Developers { get; set; } = default!;
        public IList<Property> Properties { get; set; } = default!;
        public int ProjectCount { get; set; }
        public int DeveloperCount { get; set; }
        public int PropertyCount { get; set; }

        public IList<string> ImagePaths { get; set; } = new List<string>();

        public async Task OnGetAsync(int id)
        {
            property = _context.Properties.Where(x => x.Id == id).FirstOrDefault();

            // Fetch image paths for the given property ID
            ImagePaths = _context.PropertyImages
                .Where(x => x.PropertyId == id)
                .Select(x => x.ImagePath)
                .ToList();
        }

    }
}
