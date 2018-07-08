using System.Collections.Generic;
using parse.Models;

namespace parse.Extensions
{
    public static class InputLineExtensions
    {
        ///<summary>Sometimes we get input lines that contain multiple
        ///curly braces.  Those curly braces should always be on their
        ///own line, so we need to split the existing line object into
        ///multiple line objects.</summary>
        public static IEnumerable<InputLine> SplitLineDataOnBraces(this InputLine line)
        {

            var result = new List<InputLine>();
//            var braceIndex = FindNextBrace(line.Data);
//            while (braceIndex >= 0)
//            {
//
//            }
//            if (!string.IsNullOrWhiteSpace(line.Data))
            
            return result;
        }

        private static int FindNextBrace(string input)
        {
            var openIndex = input.IndexOf(Constants.OpeningBrace);
            var closeIndex = input.IndexOf(Constants.ClosingBrace);

            if (openIndex == -1) return closeIndex;
            if (closeIndex == -1) return openIndex;
            return (closeIndex < openIndex) ? closeIndex : openIndex;
        }
    }
}