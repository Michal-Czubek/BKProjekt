using BKProjekt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BKProjekt.Models;
using System.Reflection.Emit;

namespace BKProjekt.Areas.Identity.Data;

public class BKProjektDbContext : IdentityDbContext<BKProjektUser>
{
    public BKProjektDbContext(DbContextOptions<BKProjektDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Borrow>()
            .HasOne(b => b.Book)
            .WithMany(bk => bk.Borrows)
            .HasForeignKey(b => b.BookId);

        
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<BKProjekt.Models.Book> Book { get; set; } = default!;
public DbSet<BKProjekt.Models.Borrow> Borrows { get; set; }
}
