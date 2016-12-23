Ensure app being run in Development

set environment variable
  set ASPNETCORE_ENVIRONMENT=Development

set user-secret
  Go to directory where project.json is
  dotnet user-secrets set auth0:clientSecret <secretValue>

Run application
  dotnet run

Browse to localhost:5000/

If using VS, still need to set the user-secret, app will then run via IIS Express at http://localhost:60856/ which should still work
