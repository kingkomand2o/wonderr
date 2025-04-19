using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wonderr.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITranslationService _translationService;

        public IndexModel(ITranslationService translationService, AppDbContext context)
        {
            _translationService = translationService;
            _context = context;
        }

        // Add these properties to expose translations to the view
        public ITranslationService TranslationService => _translationService;

        private readonly AppDbContext _context;       

        public List<Developer> developers { get; set; }
        public void OnGet()
        {
            developers = _context.Developers.OrderByDescending(x => x.Id).Take(3).ToList();
        }
    }
}