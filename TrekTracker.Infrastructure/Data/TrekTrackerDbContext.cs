using Microsoft.EntityFrameworkCore;
using TrekTracker.Domain.Entities;

namespace TrekTracker.Infrastructure.Data;

public class TrekTrackerDbContext : DbContext
{
    public TrekTrackerDbContext(DbContextOptions<TrekTrackerDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<RoutePhoto> RoutePhotos => Set<RoutePhoto>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<RouteTag> RouteTags => Set<RouteTag>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<RouteLike> RouteLikes => Set<RouteLike>();
    public DbSet<RouteRating> RouteRatings => Set<RouteRating>();
    public DbSet<RouteBookmark> RouteBookmarks => Set<RouteBookmark>();
    public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Email).IsRequired().HasMaxLength(255);
            b.Property(e => e.PasswordHash).IsRequired();
            b.Property(e => e.Username).IsRequired().HasMaxLength(100);
            b.HasIndex(e => e.Email).IsUnique();
            b.HasIndex(e => e.Username).IsUnique();
        });

        // Route entity
        modelBuilder.Entity<Route>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Name).IsRequired().HasMaxLength(255);
            b.Property(e => e.Description).IsRequired();
            b.HasOne(e => e.User).WithMany(u => u.Routes).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(e => e.UserId);
            b.HasIndex(e => e.IsActive);
            b.HasIndex(e => new { e.Difficulty, e.Region });
        });

        // RoutePhoto entity
        modelBuilder.Entity<RoutePhoto>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.FilePath).IsRequired();
            b.Property(e => e.ThumbnailPath).IsRequired();
            b.HasOne(e => e.Route).WithMany(r => r.Photos).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(e => e.RouteId);
        });

        // Tag entity
        modelBuilder.Entity<Tag>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Name).IsRequired().HasMaxLength(50);
            b.HasIndex(e => e.Name).IsUnique();
        });

        // RouteTag junction
        modelBuilder.Entity<RouteTag>(b =>
        {
            b.HasKey(e => new { e.RouteId, e.TagId });
            b.HasOne(e => e.Route).WithMany(r => r.RouteTags).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(e => e.Tag).WithMany(t => t.RouteTags).OnDelete(DeleteBehavior.Cascade);
        });

        // Comment entity
        modelBuilder.Entity<Comment>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Text).IsRequired();
            b.HasOne(e => e.Route).WithMany(r => r.Comments).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(e => e.User).WithMany(u => u.Comments).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(e => e.Parent).WithMany(c => c.Replies).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(e => e.RouteId);
            b.HasIndex(e => e.UserId);
            b.HasIndex(e => e.ParentId);
        });

        // RouteLike entity
        modelBuilder.Entity<RouteLike>(b =>
        {
            b.HasKey(e => new { e.UserId, e.RouteId });
            b.HasOne(e => e.User).WithMany(u => u.RouteLikes).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(e => e.Route).WithMany(r => r.Likes).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(e => e.RouteId);
        });

        // RouteRating entity
        modelBuilder.Entity<RouteRating>(b =>
        {
            b.HasKey(e => new { e.UserId, e.RouteId });
            b.HasOne(e => e.User).WithMany(u => u.RouteRatings).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(e => e.Route).WithMany(r => r.Ratings).OnDelete(DeleteBehavior.Cascade);
            b.Property(e => e.Value).HasColumnType("smallint");
            b.HasIndex(e => e.RouteId);
        });

        // RouteBookmark entity
        modelBuilder.Entity<RouteBookmark>(b =>
        {
            b.HasKey(e => new { e.UserId, e.RouteId });
            b.HasOne(e => e.User).WithMany(u => u.RouteBookmarks).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(e => e.Route).WithMany(r => r.Bookmarks).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(e => e.RouteId);
        });

        // CommentLike entity
        modelBuilder.Entity<CommentLike>(b =>
        {
            b.HasKey(e => new { e.UserId, e.CommentId });
            b.HasOne(e => e.User).WithMany(u => u.CommentLikes).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(e => e.Comment).WithMany(c => c.Likes).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(e => e.CommentId);
        });

        // Notification entity
        modelBuilder.Entity<Notification>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Type).IsRequired().HasMaxLength(50);
            b.HasOne(e => e.User).WithMany(u => u.Notifications).OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(e => e.UserId);
            b.HasIndex(e => new { e.UserId, e.IsRead });
        });
    }
}
