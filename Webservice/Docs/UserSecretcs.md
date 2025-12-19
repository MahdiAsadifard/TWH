[<-- Return to webservice](../README.md)

#  local secrets
### Learn More: [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows)


- Right click on `TWHApi` project and select `Manage User Secrets`
- It will generate a new file `secrets.json` in this directory:
    - user_secret_id: `5e7703df-4372-45b2-806a-737fca012379`
    - Windows: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
    - Linux and macOS: `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`
- Update `secrets.json`
```
{
  "Database:MongoCertificatePassword": "TWH$INS@",
  "Database:MongoCertificatePath": "C:\\TWH.pfx"
}
```
