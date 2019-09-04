using System.Collections.Generic;

namespace Samples.AdminUI.UserSettings.Models
{
    public class UserClaimsDto
    {
        public string Subject { get; set; }

        public List<ClaimDto> Claims { get; set; }
    }
}
