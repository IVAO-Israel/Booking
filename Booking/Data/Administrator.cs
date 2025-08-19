using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Booking.Data
{
    public class Administrator
    {
        [Key]
        public Guid Id { get; set; }
        public int IVAOUserId { get; set; }
        public IList<AdministratorRole>? Roles { get; set; }
        public bool IsAllowedDivision(string divisionId)
        {
            if(Roles is null)
            {
                throw new ArgumentNullException(nameof(Roles));
            }
            if(Roles.Where(r => r.DivisionId == divisionId || (r.DivisionId is null && r.Role == "ADMIN")).Any())
            {
                return true;
            }
            return false;
        }
        public void EnsureIsAllowedDivision(string divisionId, NavigationManager navigationManager)
        {
            if (!IsAllowedDivision(divisionId))
            {
                navigationManager.NavigateTo("accessdenied");
            }
        }
    }
}
