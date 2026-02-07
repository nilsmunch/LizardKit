using System;
using System.Linq;

namespace LizardKit.Scripts.BaseExtensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1)
            };
    
    
        public static bool ContainsWord(this string source, string word)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(word))
                return false;

            return source
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Any(w => string.Equals(w, word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
