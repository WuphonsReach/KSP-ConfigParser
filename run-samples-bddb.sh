#!/bin/sh

filePath=/home/syb/WR/ksp/1.4.4/Vanilla+BDDB/GameData/ModuleManager.ConfigCache

dotnet run --project samples/AntennaBalanceValues/ "$filePath"
dotnet run --project samples/ProbeCoreValues/ "$filePath"
