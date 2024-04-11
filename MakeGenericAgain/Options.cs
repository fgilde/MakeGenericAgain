using System.Collections.Generic;

namespace MakeGenericAgain
{
    public class Options
    {
        [FromCommandLine("f", nameof(FileName))]
        public string FileName { get; set; }

        [FromCommandLine("i", nameof(TypesToIgnore))]
        public ICollection<string> TypesToIgnore { get; set; } = [];
    }
}