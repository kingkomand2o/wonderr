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
    public class PRModel : PageModel
    {
        private readonly wonderr.AppDbContext _context;

        public PRModel(wonderr.AppDbContext context)
        {
            _context = context;
        }

        public IList<Property> Property { get;set; } = default!;
        public IList<PropertyImage> pp { get; set; }

        public async Task OnGetAsync()
        {
            Property = await _context.Properties
                .ToListAsync();
            pp = _context.PropertyImages.ToList();
        }
    }
}
