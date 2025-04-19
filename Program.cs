namespace wonderr
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Globalization;
    using System.Text.Json;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Configure cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Dashboard/Login"; // Redirect to the login page
                    options.AccessDeniedPath = "/Dashboard/AccessDenied"; // Optional: Access denied page
                });

            builder.Services.AddAuthorization();
            // Configure SQLite database connection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ITranslationService, TranslationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // Add HTTP Strict Transport Security
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<LanguageMiddleware>();
            app.UseCookiePolicy();

            // Add authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Razor Pages
            app.MapRazorPages();
            app.MapControllerRoute(
    name: "language",
    pattern: "Language/{action}",
    defaults: new { controller = "Language" }
);


            app.Run();
        }
    }

    // Services/TranslationService.cs
    public interface ITranslationService
    {
        string GetTranslation(string key);
        void SetLanguage(string language);
        string GetCurrentLanguage();
    }
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITranslationService translationService)
        {
            var language = context.Request.Cookies["Language"] ?? "en"; // Default to English
            translationService.SetLanguage(language);
            await _next(context);
        }
    }

    public class TranslationService : ITranslationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;
        private Dictionary<string, object> _translations;  // Changed type here

        public TranslationService(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
            LoadTranslations();
        }

        private void LoadTranslations()
        {
            var currentLanguage = GetCurrentLanguage();
            var path = Path.Combine(_environment.WebRootPath, "Translations", $"{currentLanguage}.json");
            var jsonContent = File.ReadAllText(path);
            _translations = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent);  // Changed type here
        }

        public string GetTranslation(string key)
        {
            var keyParts = key.Split('.');
            var current = _translations;
            object currentValue = current;

            foreach (var part in keyParts)
            {
                if (currentValue is JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == JsonValueKind.Object)
                    {
                        var obj = jsonElement.EnumerateObject();
                        if (obj.Any(p => p.Name == part))
                        {
                            currentValue = obj.First(p => p.Name == part).Value;
                            continue;
                        }
                    }
                }
                else if (currentValue is Dictionary<string, object> dict)
                {
                    if (dict.TryGetValue(part, out var value))
                    {
                        currentValue = value;
                        continue;
                    }
                }

                return key; // Return key if translation not found
            }

            if (currentValue is JsonElement element)
            {
                return element.GetString() ?? key;
            }

            return currentValue?.ToString() ?? key;
        }

        public void SetLanguage(string language)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1),
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("Language", language, options);
            LoadTranslations();
        }

        public string GetCurrentLanguage()
        {
            var cookie = _httpContextAccessor.HttpContext?.Request.Cookies["Language"];
            return cookie ?? "en";
        }
    }
}