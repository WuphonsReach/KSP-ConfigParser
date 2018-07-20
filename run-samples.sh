#!/bin/sh

filePath=/home/syb/WR/ksp/1.4.4/Play/GameData/ModuleManager.ConfigCache

dotnet run --project samples/AntennaBalanceValues/ "$filePath"
dotnet run --project samples/ProbeCoreValues/ "$filePath"
