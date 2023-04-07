dotnet restore

dotnet build TauCode.Cqrs.Validation.sln -c Debug
dotnet build TauCode.Cqrs.Validation.sln -c Release

dotnet test TauCode.Cqrs.Validation.sln -c Debug
dotnet test TauCode.Cqrs.Validation.sln -c Release

nuget pack nuget\TauCode.Cqrs.Validation.nuspec