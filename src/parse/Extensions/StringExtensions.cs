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

            // Comments trump everything else
            var commentStartIndex = line.IndexOf(commentMarker);
            if (commentStartIndex >= 0)
            {
                result.Comment = line.Substring(commentStartIndex + commentMarker.Length);
                line = line.Substring(0, commentStartIndex);
            }

            var assignmentStartIndex = line.IndexOf(assignmentMarker);
            if (assignmentStartIndex >= 0)
            {
                result.Value = line.Substring(assignmentStartIndex + assignmentMarker.Length);
                line = line.Substring(0, assignmentStartIndex);
            }

            result.Data = line;

            return result;
        }
    }
}