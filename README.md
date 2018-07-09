# KSP-ConfigParser

A .NET Core class library for parsing KSP configuration files and extracting node relationships and attribute definitions.

This is not meant to be used as a replacement for the official KSP configuration file parser or anything that ModuleManager does.  
It was a white-room reverse engineering of the configuration file syntax to gather some data that I needed for an add-on project.
The parser handles everything that I've thrown at it so far, but there are no guarantees of fitness for purpose.
But if you want something simple to search through thousands of configuration nodes and extract information into a CSV format, this may be useful.

# Technology

- .NET Core 2.1
- FileHelpers

