using FileHelpers;

namespace AntennaBalanceValues
{

/*

UrlConfig
{
	name = SYprobe7m
	type = PART
	parentUrl = SpaceY-Expanded/Parts/Command/SYprobe7m
	PART
	{
		name = SYprobe7m
		module = Part
		author = NecroBones
		scale = 1.0
		rescaleFactor = 1

		MODULE
		{
			name = ModuleCommand
			minimumCrew = 0
			RESOURCE
			{
				name = ElectricCharge
				rate = 0.1
			}
		}
		RESOURCE
		{
			name = ElectricCharge
			amount = 24000
			maxAmount = 24000
		}
		MODULE
		{
			name = ModuleReactionWheel
			PitchTorque = 180
			YawTorque = 180
			RollTorque = 180
			RESOURCE
			{
				name = ElectricCharge
				rate = 4
			}
		}
		MODULE
		{
			name = ModuleSAS
			SASServiceLevel = 3
		}
	}
}
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
		
		// Antenna attributes (Type will usually be DIRECT)
		public string Type;
		public string PacketInterval;
		public string PacketSize;
		public string PacketResourceCost;
		public string RequiredResource;
		public string Power;
		public string OptimumRange;
		public string PacketeFloor;
		public string PacketCeiling;
		public string Combinable;
		public string CombinableExponent;
    }
}