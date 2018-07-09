Searches the ModuleManager.ConfigCache file in my KSP install to gather information about all parts that have antennas.
It then creates an `output.csv` file in the current directory with the information.

`$ dotnet run --project samples/AntennaBalanceValues/`

The path to the configuration file is currently hard-coded, but you can pass it multiple configuration file paths as command-line arguments.
