

using BookApi;
using BookApi.Model;
using Microsoft.EntityFrameworkCore;
using Shirley.Book.Model;
using System;

namespace Shirley.Book.DataAccess
{
   public class BookContext : DbContext
   {
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }
        public DbSet<BookStock>  BookStocks { get; set; }
        public DbSet<OrderStock> OrderStocks { get; set; }
        public DbSet<BookOrderDetail> BookOrderDetials { get; set; }

        public BookContext(DbContextOptions<BookContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookOrder>(builder =>
            {
                builder.HasKey(b => b.Id);

                builder.Property(b => b.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<BookStock>(builder =>
            {
                builder.HasKey(b => b.Id);

                builder.Property(b => b.Id)
                    .ValueGeneratedOnAdd();
                builder.Property(b => b.StockCount);
                //    .IsConcurrencyToken();
                //builder.Property(b => b.FreezeStock)
                //    .IsConcurrencyToken();
            });

            modelBuilder.Entity<OrderStock>(builder =>
            {
                builder.HasKey(b => b.Id);

                builder.Property(b => b.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<BookOrderDetail>(builder =>
            {
                builder.HasKey(b => b.Id);

                builder.Property(b => b.Id)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
