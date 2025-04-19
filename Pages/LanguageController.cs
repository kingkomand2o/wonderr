using Microsoft.AspNetCore.Mvc;

namespace wonderr.Pages
{
    public class LanguageController : Controller
    {
        private readonly ITranslationService _translationService;

        public LanguageController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeLanguage(string language, string returnUrl)
        {
            if (!string.IsNullOrEmpty(language))
            {
                _translationService.SetLanguage(language);
            }

            // Redirect to the specified return URL or fallback to home
            return LocalRedirect(returnUrl ?? "~/");
        }
    }
}