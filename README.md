# WireDev ERP 2024
## How to run (debug)
1. Download and install the lates version of *ASP.NET Core Runtime 6* from [the official Mircosoft website](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Clone the project or download the *WireDev.Erp.V1.Api* folder
3. Unpack the archive if not already done
4. Open the folder in your terminal or run `cd your-location/WireDev.Erp.V2024.Api` (replayce `your-location`)
5. Trust the SSL certificate with `dotnet dev-certs https --trust`
6. Run the API with `dotnet run` (ignore warnings)
7. Get the used port from the log
8. Go to `https://your-ip:port/swagger/index.html` to see Swagger UI or just use `https://your-ip:port/api` to send HTTP-Requests (replace `your-ip` und `port`)
9. Log in at `https://.../Authenticate/login` with username: `admin` and password: `Admin#1234` to get your JWT
10. Use your JWT in Swagger UI or in the authorization-header in all your requests
