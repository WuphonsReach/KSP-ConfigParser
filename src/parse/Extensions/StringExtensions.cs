using System.Collections.Generic;
using parse.Models;

namespace parse.Extensions
{
    public static class StringExtensions
    {
        public static InputLine ParseLine(this string line)
        {
            const string commentMarker = "//";

            var result = new InputLine();
            if (line == null) return result;

            // Comments trump everything else
            var commentStartIndex = line.IndexOf(commentMarker);
            if (commentStartIndex >= 0)
            {
                result.Comment = line.Substring(commentStartIndex + commentMarker.Length).Trim();
                line = line.Substring(0, commentStartIndex);
            }

            result.Data = line.Trim();

            return result;
        }
    }
}