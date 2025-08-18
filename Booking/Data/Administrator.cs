using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Booking.Data
{
    public class Administrator
    {
        [Key]
        public Guid Id { get; set; }
        public int IVAOUserId { get; set; }
        /// <summary>
        /// If it's null, it can edit any division.
        /// </summary>
        public string? DivisionId { get; set; }
        public bool IsAllowedDivision(string divisionId)
        {
            if(DivisionId is null || divisionId == DivisionId)
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
