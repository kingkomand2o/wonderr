using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wonderr;

namespace wonderr.Pages
{
    public class PLModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ITranslationService _translationService;

        // Add these properties to expose translations to the view
        public PLModel(AppDbContext context, IWebHostEnvironment environment, ITranslationService translationService)
        {
            _context = context;
            _environment = environment;
            _translationService = translationService;
        }
        public ITranslationService TranslationService => _translationService;

        public IList<Project> Projects { get; set; } = default!;

        public Project project { get; set; }
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
            project = _context.Projects.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
