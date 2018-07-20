using FileHelpers;

namespace AntennaBalanceValues
{

/*
 */

    [DelimitedRecord(",")]
    public class ExportRecord
    {
        public string TopFolder;
        public string Folder;
        public string FileName;
        public string Name;
		[FieldQuoted('"', QuoteMode.OptionalForRead)]
		public string Title;
		[FieldQuoted('"', QuoteMode.OptionalForRead)]
        public string Author;
		[FieldQuoted('"', QuoteMode.OptionalForRead)]
        public string Manufacturer;
		public string Category;
		public string EntryCost;
		public string PartCost;
		public string Mass;
		
    }
}