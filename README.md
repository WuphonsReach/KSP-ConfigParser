# KSP-ConfigParser

A .NET Core class library for parsing KSP configuration files and extracting node relationships and attribute definitions.

This is not meant to be used as a replacement for the official KSP configuration file parser or anything that ModuleManager does.  It was a white-room reverse engineering of the configuration file syntax to gather some data that I needed for an add-on project. The parser handles everything that I've thrown at it so far, but there are no guarantees of fitness for purpose. But if you want something simple to search through thousands of configuration nodes and extract information into a CSV format, this may be useful.

Overall, I recommend parsing the ModuleManager.ConfigCache file instead of looking at the original KSP part configuration files.  ModuleManager will handle a lot of the oddities that can happen in `.cfg` files, plus it applies any MM patches in the various files.

# Current analyzers

All of the pre-written analyzers are in the `samples/` folder.  They will parse the configuration file(s) and output a CSV file with the desired information.

- AntennaBalanceValues: Pulls a list of all parts which can act as an antenna (direct, relay, internal).  I used this heavily to work on my KSP Stock Antenna Balance addon.
- ProbeCoreValues: Pulls a list of all parts that have ModuleCommand.  Includes information like min/max crew, antenna values, mass, cost, etc..

# Spreadsheet Links

- [AntennaBalanceValues 1.8.1](https://docs.google.com/spreadsheets/d/1LVQxm1v-wOdYZpmVgXJPef13nOi2Itake04iL8eZbYE/edit?usp=sharing): KSP 1.8.1, mostly vanilla install, with part mods

# Getting Started

You will need to have installed the [.NET Core 2.1 (or later) SDK](https://dot.net/core).  This is available for macOS, Linux and Windows.  For the moment, that is the only prerequisite to be installed prior to forking/cloning the git repository.

Take a look at [runsamples.sh](runsamples.sh) for how to run the sample projects on macOS/Linux.

# Technology

- .NET Core 2.1
- FileHelpers

