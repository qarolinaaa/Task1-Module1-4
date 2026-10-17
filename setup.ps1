\
    $ErrorActionPreference = "Stop"
    dotnet restore
    dotnet tool list --global | Select-String "dotnet-ef" | Out-Null
    if ($LASTEXITCODE -ne 0) { dotnet tool install --global dotnet-ef }
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    dotnet run
