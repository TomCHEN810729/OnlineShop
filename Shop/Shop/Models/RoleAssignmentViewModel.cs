using Microsoft.AspNetCore.Identity;

namespace Shop.Models
{
    public class RoleAssignmentViewModel
    {
        public List<IdentityUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public string SelectedUserId { get; set; }
        public string SelectedRoleId { get; set; }
    }
}
