namespace wonderr
{
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; } // Add this

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between Property and PropertyImage
            modelBuilder.Entity<PropertyImage>()
                .HasOne(pi => pi.Property)
                .WithMany(p => p.PropertyImages)
                .HasForeignKey(pi => pi.PropertyId);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Developer
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string Image { get; set; }

        // Navigation property
        public ICollection<Project> Projects { get; set; }
    }

    public class Project
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string LocationEn { get; set; }
        public string LocationAr { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string DeliveryDate { get; set; }
        public string OverviewEn { get; set; }
        public string OverviewAr { get; set; }
    }

    public class PropertyImage
    {
        public int Id { get; set; } // Primary Key
        public string ImagePath { get; set; } // Path to the image file

        // Foreign Key
        public int PropertyId { get; set; }
        public Property Property { get; set; } // Navigation property
    }

    public class Property
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string LotTypeEn { get; set; }
        public string LotTypeAr { get; set; }
        public string LocationEn { get; set; }
        public string LocationAr { get; set; }
        public decimal Price { get; set; }
        public int UnitSize { get; set; }
        public string DeliveryDate { get; set; }

        // Foreign Key
        public int? ProjectId { get; set; } // Nullable for independent properties
        public Project Project { get; set; }

        // Navigation property for images
        public ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
    }
}
