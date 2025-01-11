using libraryManagment.Core.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryManagment.EF.Context
{
    public class AppDbcontext:IdentityDbContext<ApplicationUser>
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options):base(options) { }


        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           builder.Entity<Book>().HasIndex(x=>x.ISBN).IsUnique();

            builder.Entity<UserBook>()
                .HasKey(ub => new { ub.UserId, ub.BookId });

           
            builder.Entity<UserBook>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBooks)
                .HasForeignKey(ub => ub.UserId);

           
            builder.Entity<UserBook>()
                .HasOne(ub => ub.Book)
                .WithMany(b => b.UserBooks)
                .HasForeignKey(ub => ub.BookId);

            builder.Entity<Book>().HasData(
      new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1111", IsBorrowed = false },
      new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "2222", IsBorrowed = true },
       new Book { Id = 3, Title = "Book 3", Author = "Author 3", ISBN = "3333", IsBorrowed = true },
        new Book { Id = 4, Title = "Book 4", Author = "Author 4", ISBN = "4444", IsBorrowed = true }
  );

            builder.Entity<UserBook>().HasData(
                new UserBook { UserId = "24885ec7-d7a1-45be-85c2-e5bbb5e1774e", BookId = 2, BorrowDate = DateTime.UtcNow }
            );
        }

    }
}
