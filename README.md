# UrlService
 
## Contains
- WEB API to obtain full URL from shortened URL.
- Razor page to enter URL and test returned shortened URL.

## Installation
- Run the urlservice.sql file to create UrlService database.
- Script also create UrlServiceUser (referenced in appsettings.json).

## Usage
- Load project into Visual Studio, build and run.
- Should see the following

<img src="https://github.com/KarlPage/UrlService/blob/master/urlservice.jpg?raw=true">

## Notes
- Microsoft Visual Studio Community 2019 (Version 16.8.6)
- ASP.NET Core (version 5)
- SQL Server 2019
- Can test with Swagger (<base url>/swagger/index.html) to see generated Json

## Refactoring
- Debug shows full call stack with exception details.
  In release mode, one just sees internal server error.
  Useful to include potential error details in the UrlResult.
- Rename UrlResult to UrlServiceResult.
  UrlServiceResult is a POD that requires public constructor for use by the framework when converting to Json, XML etc.
  Whilst one can annotate classes to to ignore, rename, properties, this generates uneccasary dependencies.
  For this reason, I believe it is best to dedicate a single POD for final client output.
  This also ensures that core objects remain immutable.
- Introduce a new UrlServiceStatus class, this should contain an enum, failed, passed, and error details for a failure.
  The UrlServiceResult would contain an instance of the UrlServiceStatus class.
  This would allow clients to query the UrlServiceStatus and act accordingly.
