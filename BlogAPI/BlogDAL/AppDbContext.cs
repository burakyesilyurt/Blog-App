using BlogDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogDAL
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<ArticleImage> ArticleImages { get; set; }
        public DbSet<ArticleLike> ArticleLikes { get; set; }
        public DbSet<ViewCount> ViewCounts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Article - ArticleImage (One-to-Many)
            //modelBuilder.Entity<Article>()
            //    .HasMany(a => a.ArticleImages)
            //    .WithOne(ai => ai.Article)
            //    .HasForeignKey(ai => ai.ArticleImageId);

            // Article - ArticleView (One-to-Many)
            modelBuilder.Entity<Article>()
                .HasMany(a => a.ViewCounts)
                .WithOne(av => av.Article)
                .HasForeignKey(av => av.ViewCountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Article - ArticleLike (One-to-Many)
            modelBuilder.Entity<Article>()
                .HasMany(a => a.ArticleLikes)
                .WithOne(al => al.Article)
                .HasForeignKey(al => al.ArticleLikeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(a => a.ArticleLikes)
                .WithOne(al => al.User)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Article>()
                .HasMany(a => a.Comment)
                .WithOne(al => al.Article)
                .HasForeignKey(al => al.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(m => m.Comments)
                .WithOne(u => u.User)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Category>()
            //    .HasMany(a => a.Articles)
            //    .WithMany(u => u.Categories);
            modelBuilder.Entity<Article>()
                .HasMany(a => a.Categories)
                .WithMany(a => a.Articles);

            modelBuilder.Entity<Article>()
                .HasMany(t => t.Tags)
                .WithMany(a => a.Articles);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Article)
                .WithOne(a => a.User)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasMany(v => v.ViewCounts)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
