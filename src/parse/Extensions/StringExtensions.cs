using parse.Models;

namespace parse.Extensions
{
    public static class StringExtensions
    {
        public static InputLine ParseLine(this string line)
        {
            //TODO: Rework this with string.Split()
            const string commentMarker = "//";
            const string assignmentMarker = "=";

            var result = new InputLine();
            if (line == null) return result;

            var assignmentStartIndex = line.IndexOf(assignmentMarker);
            result.Data = (assignmentStartIndex == -1)
                ? line
                : line.Substring(0, assignmentStartIndex)
                ;

            line = line.Substring(result.Data.Length);

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