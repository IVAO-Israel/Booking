using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IAtcPositionService
    {
        public Task AddAtcPosition(AtcPosition position);
        public Task<AtcPosition?> GetAtcPosition(Guid id);
        public Task<AtcPosition?> GetAtcPosition(string IVAOCallsign);
        public Task UpdateAtcPosition(AtcPosition position);
        public Task RemoveAtcPosition(AtcPosition position);
        public Task<List<AtcPosition>> GetAllAtcPositions();
    }
}
