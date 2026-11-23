download and install the following NuGet packages:
dotnet add package Microsoft.Extensions.Hosting
dotnet add package Microsoft.Extensions.Http


note: 

web api app also included the frame that can use IHttpClientFactory
but for console app, we need to add the Microsoft.Extensions.Hosting package 
to use the HostBuilder to build a host container and register services for dependency injection.
