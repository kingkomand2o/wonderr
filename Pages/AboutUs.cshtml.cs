using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wonderr.Pages
{
    public class AboutUsModel : PageModel
    {
        private readonly ITranslationService _translationService;

        public AboutUsModel(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        // Add these properties to expose translations to the view
        public ITranslationService TranslationService => _translationService;
        public void OnGet()
        {
        }
    }
}
