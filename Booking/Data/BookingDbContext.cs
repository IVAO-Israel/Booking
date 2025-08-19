using Booking.Ivao.DTO;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data
{
    public class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CreateRelations(modelBuilder);
        }
        private static void CreateRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventAtcPosition>().HasOne(p => p.Event).WithMany(p => p.AvailableAtcPositions)
                                                   .HasForeignKey(p => p.EventId)
                                                   .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EventAtcPosition>().HasOne(p => p.AtcPosition).WithMany()
                                                   .HasForeignKey(p => p.AtcPositionId)
                                                   .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AtcPositionBooking>().HasOne(p => p.EventAtcPosition).WithMany(p => p.Bookings)
                                                     .HasForeignKey(p => p.EventAtcPositionId)
                                                     .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AdministratorRole>().HasOne(p => p.Administrator).WithMany(p => p.Roles)
                                                    .HasForeignKey(p => p.AdministratorId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Event>().HasOne<Division>().WithMany().HasForeignKey(p => p.DivisionId);
            modelBuilder.Entity<AtcPosition>().HasOne<Division>().WithMany().HasForeignKey(p => p.DivisionId);
            modelBuilder.Entity<AdministratorRole>().HasOne<Division>().WithMany().HasForeignKey(p => p.DivisionId);
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<AtcPosition> AtcPositions { get; set; }
        public DbSet<EventAtcPosition> EventAtcPositions { get; set; }
        public DbSet<AtcPositionBooking> BookedAtcPositions { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<DbTokenData> IvaoTokenData { get; set; }
        public DbSet<Division> Divisions { get; set; }
    }
}
