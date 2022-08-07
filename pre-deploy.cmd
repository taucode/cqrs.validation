dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.Cqrs.Validation.Tests\TauCode.Cqrs.Validation.Tests.csproj
dotnet test -c Release .\test\TauCode.Cqrs.Validation.Tests\TauCode.Cqrs.Validation.Tests.csproj

nuget pack nuget\TauCode.Cqrs.Validation.nuspec