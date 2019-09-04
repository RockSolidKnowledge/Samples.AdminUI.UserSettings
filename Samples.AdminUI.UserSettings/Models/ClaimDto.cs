using System;

namespace Samples.AdminUI.UserSettings.Models
{
    public class ClaimDto
    {
        public ClaimDto() { }

        public ClaimDto(string type, string value)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value; 
        }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}