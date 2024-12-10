using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using VinLotteri.Data.Models;

namespace VinLotteri.Data
{
    public class LotteryDbContext : DbContext
    {
        public LotteryDbContext(DbContextOptions<LotteryDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>().HasData(
                Enumerable.Range(1, 100).Select(i => new Ticket
                {
                    Id = i,
                    Number = i,
                    Status = TicketStatus.Available
                })
            );


        }


        public DbSet<Ticket> Tickets { get; set; }

    }
}
