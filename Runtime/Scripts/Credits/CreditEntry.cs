namespace TricksForTreats.Credits
{
    public class CreditEntry
    {
        public readonly string Title;
        public readonly string Name;
        public readonly int Count;

        public CreditEntry(string intitle, string inname)
        {
            Title = intitle;
            Name = inname;
            Count = 1;
        }
        public CreditEntry(string intitle, string[] names)
        {
            Title = intitle;
            Name = string.Join("\n", names);
            Count = names.Length;
        }
    }
}