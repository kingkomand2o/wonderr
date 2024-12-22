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
    public class ProjectsModel : PageModel
    {
        private readonly wonderr.AppDbContext _context;

        public ProjectsModel(wonderr.AppDbContext context)
        {
            _context = context;
        }

        public IList<Project> Project { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Project = await _context.Projects
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    TempData["ErrorMessage"] = "Project not found.";
                    return RedirectToPage("./Projects");
                }

                _context.Projects.Remove(project);
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
