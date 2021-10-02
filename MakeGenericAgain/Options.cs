namespace MakeGenericAgain
{
    public class Options
    {
        [FromCommandLine("f", nameof(FileName))]
        public string FileName { get; set; }
    }
}