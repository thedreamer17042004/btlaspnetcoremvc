using Microsoft.EntityFrameworkCore;
using AttributeC = Asp.netApp.Areas.Admin.Models.DataModel.AttributeC;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.DataModels;
namespace Asp.netApp.Areas.Admin.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public DbSet<RoleC> Roles { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<PasswordReset> PasswordResets { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryLanguage> CategoryLanguages { get; set; }

        public DbSet<AttributeC> Attributes { get; set; }

        public DbSet<Brand> Brands { get; set; }
        
        public DbSet<AttributeLanguage> AttributeLanguages { get; set; }

        public DbSet<AttributeOption> AttributeOptions { get; set; }
        public DbSet<AttributeOptionLanguage> AttributeOptionLanguages { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductLanguage> ProductLanguages { get; set; }

        public DbSet<AttributeOptionProduct> AttributeOptionProducts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleC>(entity =>
            {
                entity.ToTable("roles");
            });
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");
            });
            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.ToTable("password_resets");
            });
            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("languages");
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
            });
            modelBuilder.Entity<CategoryLanguage>(entity =>
            {
                entity.ToTable("category_languages");
            });

            modelBuilder.Entity<Category>()
              .HasMany(p => p.CategoryLanguages)
              .WithOne(pl => pl.Category)
              .HasForeignKey(pl => pl.CategoryId)
              .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete



            modelBuilder.Entity<AttributeC>(entity =>
            {
                entity.ToTable("attributes");
            });
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brands");
            });
            modelBuilder.Entity<AttributeLanguage>(entity =>
            {
                entity.ToTable("attribute_languages");
            });


            modelBuilder.Entity<AttributeC>()
              .HasMany(p => p.AttributeLanguages)
              .WithOne(pl => pl.Attribute)
              .HasForeignKey(pl => pl.AttributeId)
              .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete

            modelBuilder.Entity<AttributeOption>(entity =>
            {
                entity.ToTable("attribute_options");
            });
            modelBuilder.Entity<AttributeOptionLanguage>(entity =>
            {
                entity.ToTable("attribute_option_languages");
            });

            modelBuilder.Entity<AttributeOption>()
          .HasMany(p => p.AttributeOptionLanguage)
          .WithOne(pl => pl.AttributeOption)
          .HasForeignKey(pl => pl.AttributeOptionId)
          .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete


            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
            });
            modelBuilder.Entity<ProductLanguage>(entity =>
            {
                entity.ToTable("product_languages");
            });


            modelBuilder.Entity<Product>()
          .HasMany(p => p.ProductLanguages)
          .WithOne(pl => pl.Product)
          .HasForeignKey(pl => pl.ProductId)
          .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete


            modelBuilder.Entity<AttributeOptionProduct>(entity =>
            {
                entity.ToTable("attribute_option_products");
            });
            modelBuilder.Entity<RoleC>().HasData(
                new RoleC {RoleId=1, RoleName = "ADMIN", Permissions = "[\"ALL\"]" },
                new RoleC { RoleId = 2, RoleName = "STAFF", Permissions = "[\"PRODUCT_MANAGEMENT\", \"LANGUAGE_MANAGEMENT\"]" },
                new RoleC { RoleId = 3, RoleName = "USER", Permissions = "[\"BRAND_MANAGEMENT\", \"LANGUAGE_MANAGEMENT\"]" },
                new RoleC { RoleId = 4, RoleName = "MANAGER", Permissions = "[\"PRODUCT_MANAGEMENT\", \"POST_MANAGEMENT\", \"LANGUAGE_MANAGEMENT\"]" }
            );
            DatabaseSeeder.SeedRolesAndAccounts(modelBuilder);
        }



    }
}
