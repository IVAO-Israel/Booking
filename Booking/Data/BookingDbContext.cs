using Microsoft.EntityFrameworkCore;

namespace Booking.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CreateRelations(modelBuilder);
        }
        private void CreateRelations(ModelBuilder modelBuilder)
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
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<AtcPosition> AtcPositions { get; set; }
        public DbSet<EventAtcPosition> EventAtcPositions { get; set; }
        public DbSet<AtcPositionBooking> BookedAtcPositions { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
    }
}
