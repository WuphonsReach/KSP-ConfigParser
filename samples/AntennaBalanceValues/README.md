Searches the ModuleManager.ConfigCache file in my KSP install to gather information about all parts that have antennas.
It then creates an `output.csv` file in the current directory with the information.
The path to the configuration file is currently hard-coded, but you can pass it multiple configuration file paths as command-line arguments.

# Sample Output

`$ dotnet run --project samples/AntennaBalanceValues/`

    Path: /home/syb/WR/ksp/1.4.4/Play/GameData/ModuleManager.ConfigCache
    Folder:
    FileName: ModuleManager.ConfigCache
    Length: 10753974
    Part nodes: 2055
    Part nodes with antennas: 197
    Finished /home/syb/WR/ksp/1.4.4/Play/GameData/ModuleManager.ConfigCache

    Writing output.csv
    Completed.
