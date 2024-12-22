using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace wonderr.Pages.Dashboard
{
    [Authorize]
    public class indexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public indexModel(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IList<Project> Projects { get; set; } = default!;

        public Project Project { get; set; }
        public Developer Developer { get; set; }
        public Property Property { get; set; }
        public IList<Developer> Developers { get; set; } = default!;
        public IList<Property> Properties { get; set; } = default!;
        public int ProjectCount { get; set; }
        public int DeveloperCount { get; set; }
        public int PropertyCount { get; set; }

        public async Task OnGetAsync()
        {
            Projects = await _context.Projects
                .ToListAsync();

            Developers = await _context.Developers.ToListAsync();

            Properties = await _context.Properties
                .Include(p => p.Project)
                .ToListAsync();

            Project = new Project();
            Developer = new Developer();
            Property = new Property();

            ProjectCount = Projects.Count;
            DeveloperCount = Developers.Count;
            PropertyCount = Properties.Count;
        }

        public async Task<IActionResult> OnPostCreateProjectAsync(Project project, IFormFile Image)
        {
            try
            {
                // Image handling
                if (Image != null && Image.Length > 0)
                {
                    var uploadFolder = Path.Combine(_environment.WebRootPath, "product");
                    Directory.CreateDirectory(uploadFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    project.Image = Path.Combine("product", uniqueFileName);
                }
                // Save project
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Project added successfully!";
                return RedirectToPage("/Dashboard/index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error saving project: {ex.Message}";
                return RedirectToPage("/Dashboard/index");
            }
        }


        public async Task<IActionResult> OnPostCreateDeveloperAsync(Developer developer, IFormFile Image)
        {
            try
            {
                // Log data for debugging
                Console.WriteLine("HEYHEYHEYHEYHEY" + developer.NameEn + developer.DescriptionEn);

                // Image handling
                if (Image != null && Image.Length > 0)
                {
                    var uploadFolder = Path.Combine(_environment.WebRootPath, "product");
                    Directory.CreateDirectory(uploadFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    developer.Image = Path.Combine("product", uniqueFileName);
                }

                // Validate that all required fields have valid values
                if (string.IsNullOrWhiteSpace(developer.NameAr))
                {
                    developer.NameAr = developer.NameEn; // Use NameEn as a fallback if NameAr is not provided
                }

                // Save developer
                _context.Developers.Add(developer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Developer added successfully!";
                return RedirectToPage("/Dashboard/index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error saving developer: {ex.Message}";
                return RedirectToPage("/Dashboard/index");
            }
        }




        public async Task<IActionResult> OnPostCreatePropertyAsync(Property property, List<IFormFile> Images)
        {
            try
            {
                // Add the property to the database first to generate its ID
                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                // Handle multiple image uploads
                if (Images != null && Images.Any())
                {
                    var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads", "properties");
                    Directory.CreateDirectory(uploadFolder);

                    foreach (var image in Images)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        var filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        // Save each image with a reference to the property
                        var propertyImage = new PropertyImage
                        {
                            PropertyId = property.Id,
                            ImagePath = Path.Combine("uploads", "properties", uniqueFileName)
                        };

                        _context.PropertyImages.Add(propertyImage);
                    }

                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Property added successfully!";
                return RedirectToPage("/Dashboard/index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToPage("/Dashboard/index");
            }
        }
    }
}
