using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Models;

namespace blog_ca_nhan.Data;

public partial class PremiumBlogDbContext : DbContext
{
    public PremiumBlogDbContext()
    {
    }

    public PremiumBlogDbContext(DbContextOptions<PremiumBlogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogThemeOption> BlogThemeOptions { get; set; }

    public virtual DbSet<BlogUser> BlogUsers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<PlatformSetting> PlatformSettings { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostMetum> PostMeta { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PNAM;Database=PremiumBlogDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC073B28872F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Blog).WithMany(p => p.AuditLogs).HasConstraintName("FK__AuditLogs__BlogI__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs).HasConstraintName("FK__AuditLogs__UserI__02FC7413");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blogs__3214EC075B726B94");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(1);

            entity.HasOne(d => d.CurrentTheme).WithMany(p => p.Blogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blogs__CurrentTh__4E88ABD4");

            entity.HasOne(d => d.Owner).WithMany(p => p.Blogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blogs__OwnerId__4D94879B");

            entity.HasOne(d => d.Plan).WithMany(p => p.Blogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blogs__PlanId__4F7CD00D");
        });

        modelBuilder.Entity<BlogThemeOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogThem__3214EC07D773D403");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogThemeOptions).HasConstraintName("FK__BlogTheme__BlogI__7E37BEF6");

            entity.HasOne(d => d.Theme).WithMany(p => p.BlogThemeOptions).HasConstraintName("FK__BlogTheme__Theme__7F2BE32F");
        });

        modelBuilder.Entity<BlogUser>(entity =>
        {
            entity.HasKey(e => new { e.BlogId, e.UserId }).HasName("PK__BlogUser__854F12F447F048EE");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogUsers).HasConstraintName("FK__BlogUsers__BlogI__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.BlogUsers).HasConstraintName("FK__BlogUsers__UserI__5535A963");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A7CDBF7C");

            entity.Property(e => e.OrderIndex).HasDefaultValue(0);

            entity.HasOne(d => d.Blog).WithMany(p => p.Categories).HasConstraintName("FK__Categorie__BlogI__5812160E");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK__Categorie__Paren__59063A47");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC0720DB8592");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments).HasConstraintName("FK__Comments__BlogId__73BA3083");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK__Comments__Parent__75A278F5");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__PostId__74AE54BC");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Media__3214EC0791F5C6EB");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Blog).WithMany(p => p.Media).HasConstraintName("FK__Media__BlogId__6EF57B66");

            entity.HasOne(d => d.User).WithMany(p => p.Media)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Media__UserId__6FE99F9F");
        });

        modelBuilder.Entity<PlatformSetting>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PK__Platform__C41E02882A612884");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Posts__3214EC071FAEAC51");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsFeatured).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue(0);
            entity.Property(e => e.ViewCount).HasDefaultValue(0);

            entity.HasOne(d => d.Author).WithMany(p => p.Posts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Posts__AuthorId__5DCAEF64");

            entity.HasOne(d => d.Blog).WithMany(p => p.Posts).HasConstraintName("FK__Posts__BlogId__5CD6CB2B");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Posts__CategoryI__5EBF139D");

            entity.HasMany(d => d.Tags).WithMany(p => p.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PostTags__TagId__6C190EBB"),
                    l => l.HasOne<Post>().WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK__PostTags__PostId__6B24EA82"),
                    j =>
                    {
                        j.HasKey("PostId", "TagId").HasName("PK__PostTags__7C45AF8257F0EE4C");
                        j.ToTable("PostTags");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.PostsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "PostLike",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__PostLikes__UserI__7B5B524B"),
                    l => l.HasOne<Post>().WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK__PostLikes__PostI__7A672E12"),
                    j =>
                    {
                        j.HasKey("PostId", "UserId").HasName("PK__PostLike__7B6AECDCEEF5984A");
                        j.ToTable("PostLikes");
                    });
        });

        modelBuilder.Entity<PostMetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PostMeta__3214EC07B308B393");

            entity.HasOne(d => d.Post).WithMany(p => p.PostMeta).HasConstraintName("FK__PostMeta__PostId__656C112C");
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrib__3214EC07A18B4E36");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Blog).WithMany(p => p.Subscribers).HasConstraintName("FK__Subscribe__BlogI__06CD04F7");
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC078F30A611");

            entity.Property(e => e.HasAdFree).HasDefaultValue(false);
            entity.Property(e => e.HasCustomDomain).HasDefaultValue(false);
            entity.Property(e => e.MaxPosts).HasDefaultValue(-1);
            entity.Property(e => e.MaxStorageMb).HasDefaultValue(100L);
            entity.Property(e => e.Price).HasDefaultValue(0m);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC074026FE67");

            entity.HasOne(d => d.Blog).WithMany(p => p.Tags).HasConstraintName("FK__Tags__BlogId__68487DD7");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Themes__3214EC0780EA44BC");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsPremium).HasDefaultValue(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07622D3F27");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsPlatformAdmin).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
