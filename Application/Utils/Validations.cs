using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class Validations
    {
        public bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public string FormatLabel(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            // Replace underscores with spaces, ensure Genus is Capitalized and species epithets are lowercase.
            var parts = raw.Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return raw.Replace('_', ' ');

            parts[0] = Capitalize(parts[0]);
            for (int i = 1; i < parts.Length; i++)
                parts[i] = parts[i].ToLowerInvariant();

            return string.Join(' ', parts);
        }

        public string Capitalize(string s)
            => string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1).ToLowerInvariant();
    }
}
