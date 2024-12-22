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
    public class DevelopersModel : PageModel
    {
        private readonly wonderr.AppDbContext _context;

        public DevelopersModel(wonderr.AppDbContext context)
        {
            _context = context;
        }

        public IList<Developer> Developer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Developer = await _context.Developers.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var project = await _context.Developers.FindAsync(id);
                if (project == null)
                {
                    TempData["ErrorMessage"] = "Project not found.";
                    return RedirectToPage("./Developers");
                }

                _context.Developers.Remove(project);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Project deleted successfully.";
                return RedirectToPage("./Developers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting project: {ex.Message}";
                return RedirectToPage("./Developers");
            }
        }
    }
}
