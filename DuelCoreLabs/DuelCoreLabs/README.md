# Di-Core Labs website (VS Code)

1. Install the .NET 8 SDK and the "C# Dev Kit" VS Code extension.
2. Copy your logos into `wwwroot/images/` as `1.png` and `2.png`.
3. In the VS Code terminal, from this folder:
       dotnet dev-certs https --trust
       dotnet watch run
   or press F5 (uses .vscode/launch.json).
4. To enable the contact form email:
       dotnet user-secrets set "Smtp:Host" "smtp.yourprovider.com"
       dotnet user-secrets set "Smtp:Port" "587"
       dotnet user-secrets set "Smtp:User" "hello@dicorelabs.com"
       dotnet user-secrets set "Smtp:Password" "your-app-password"
