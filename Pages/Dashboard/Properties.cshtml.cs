using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wonderr;

namespace wonderr.Pages.Dashboard
{
    [Authorize]
    public class PropertiesModel : PageModel
    {
        private readonly wonderr.AppDbContext _context;

        public PropertiesModel(wonderr.AppDbContext context)
        {
            _context = context;
        }

        public IList<Property> Property { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Property = await _context.Properties
                .Include(p => p.Project).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var project = await _context.Properties.FindAsync(id);
                if (project == null)
                {
                    TempData["ErrorMessage"] = "Project not found.";
                    return RedirectToPage("./Projects");
                }

                _context.Properties.Remove(project);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Project deleted successfully.";
                return RedirectToPage("./Projects");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting project: {ex.Message}";
                return RedirectToPage("./Projects");
            }
        }
    }
}
