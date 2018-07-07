using parse.Models;

namespace parse.Extensions
{
    public static class StringExtensions
    {
        public static Line ParseLine(this string line)
        {
            //TODO: Rework this with string.Split()
            const string commentMarker = "//";
            var result = new Line();
            if (line == null) return result;
            var commentStartIndex = line.IndexOf(commentMarker);
            result.Data = (commentStartIndex == -1)
                ? line
                : line.Substring(0, commentStartIndex)
                ;
            result.Comment = (commentStartIndex == -1)
                ? null
                : line.Substring(commentStartIndex + commentMarker.Length)
                ;

            return result;
        }
    }
}