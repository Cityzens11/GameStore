using GameStore.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Context;

public class MainDbContext : IdentityDbContext<User, UserRole, Guid>
{
    public virtual DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public MainDbContext() { }
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
        modelBuilder.Entity<UserRole>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_role_owners");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("user_role_claims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");

        modelBuilder.Entity<Game>().ToTable("games");
        modelBuilder.Entity<Game>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Game>().Property(x => x.Title).HasMaxLength(100);
        modelBuilder.Entity<Game>().Property(x => x.Description).IsRequired();
        modelBuilder.Entity<Game>().Property(x => x.Description).HasMaxLength(3000);
        modelBuilder.Entity<Game>().Property(x => x.Price).IsRequired();
        modelBuilder.Entity<Game>().Property(x => x.Publisher).IsRequired();

        modelBuilder.Entity<Genre>().ToTable("genres");
        modelBuilder.Entity<Genre>().HasMany(x => x.Games).WithMany(x => x.Genres).UsingEntity(t => t.ToTable("games_genres"));

        modelBuilder.Entity<Comment>().ToTable("comments");
        modelBuilder.Entity<Comment>().HasOne(x => x.Game).WithMany(x => x.Comments).HasForeignKey(x => x.GameId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Comment>().Property(x => x.Body).HasMaxLength(600);
    }
}

