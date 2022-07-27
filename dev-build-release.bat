dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.Validation.Tests\TauCode.Validation.Tests.csproj
dotnet test -c Release .\tests\TauCode.Validation.Tests\TauCode.Validation.Tests.csproj

nuget pack nuget\TauCode.Validation.nuspec
