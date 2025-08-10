using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data
{
    public class BookingDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CreateRelations(modelBuilder);
        }
        private void CreateRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventAtcPosition>().HasOne(p => p.Event).WithMany(p => p.AvailableAtcPositions)
                                                   .HasForeignKey(p => p.EventId);
            modelBuilder.Entity<EventAtcPosition>().HasOne(p => p.AtcPosition).WithMany().HasForeignKey(p => p.AtcPositionId);
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<AtcPosition> AtcPositions { get; set; }
        public DbSet<EventAtcPosition> EventAtcPositions { get; set; }
    }
}
