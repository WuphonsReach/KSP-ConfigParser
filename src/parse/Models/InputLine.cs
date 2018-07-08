namespace parse.Models
{
    public class InputLine
    {
        ///<summary>Anything up to the first "//" on a line</summary>
        public string Data { get; set; }

        ///<summary>Anything after the first "//" (comment) on a line</summary>
        public string Comment { get; set; }
    }
}