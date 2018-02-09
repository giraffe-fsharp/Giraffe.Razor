# Giraffe.Razor

![Giraffe](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe/master/giraffe.png)

Razor view engine http handlers for the Giraffe web framework.

[![NuGet Info](https://buildstats.info/nuget/Giraffe.Razor?includePreReleases=true)](https://www.nuget.org/packages/Giraffe.Razor/)

| Windows | Linux |
| :------ | :---- |
| [![Windows Build status](https://ci.appveyor.com/api/projects/status/914030ec0lrc0vti/branch/develop?svg=true)](https://ci.appveyor.com/project/dustinmoris/giraffe-razor/branch/develop) | [![Linux Build status](https://travis-ci.org/giraffe-fsharp/Giraffe.Razor.svg?branch=develop)](https://travis-ci.org/giraffe-fsharp/Giraffe.Razor/builds?branch=develop) |
| [![Windows Build history](https://buildstats.info/appveyor/chart/dustinmoris/giraffe-razor?branch=develop&includeBuildsFromPullRequest=false)](https://ci.appveyor.com/project/dustinmoris/giraffe-razor/history?branch=develop) | [![Linux Build history](https://buildstats.info/travisci/chart/giraffe-fsharp/Giraffe.Razor?branch=develop&includeBuildsFromPullRequest=false)](https://travis-ci.org/giraffe-fsharp/Giraffe.Razor/builds?branch=develop) |

## Table of contents

- [Documentation](#documentation)
    - [razorView](#razorview)
    - [razorHtmlView](#razorhtmlview)
    - [validateAntiforgeryToken](#validateantiforgerytoken)
- [Samples](#samples)
- [Nightly builds and NuGet feed](#nightly-builds-and-nuget-feed)
- [More information](#more-information)
- [License](#license)

## Documentation

The `Giraffe.Razor` NuGet package adds additional `HttpHandler` functions to render Razor views in Giraffe.

### razorView

`razorView` uses the official ASP.NET Core MVC Razor view engine to compile a page and set the body of the `HttpResponse`. This http handler triggers a response to the client and other http handlers will not be able to modify the HTTP headers afterwards any more.

The `razorView` handler requires the view name, an object model and the contentType of the response to be passed in. It also requires to be enabled through the `AddRazorEngine` function during start-up.

#### Example:

Add the razor engine service during start-up:

```fsharp
open Giraffe
open Giraffe.Razor

type Startup() =
    member __.ConfigureServices (services : IServiceCollection, env : IHostingEnvironment) =
        let viewsFolderPath = Path.Combine(env.ContentRootPath, "views")
        services.AddRazorEngine(viewsFolderPath) |> ignore
```

Use the razorView function:

```fsharp
open Giraffe
open Giraffe.Razor

[<CLIMutable>]
type TestModel =
    {
        WelcomeText : string
    }

let model = { WelcomeText = "Hello, World" }

let app =
    choose [
        // Assuming there is a view called "Index.cshtml"
        route  "/" >=> razorView "text/html" "Index" model
    ]
```

### razorHtmlView

`razorHtmlView` is the same as `razorView` except that it automatically sets the response as `text/html`.

#### Example:

Add the razor engine service during start-up:

```fsharp
open Giraffe
open Giraffe.Razor

type Startup() =
    member __.ConfigureServices (services : IServiceCollection, env : IHostingEnvironment) =
        let viewsFolderPath = Path.Combine(env.ContentRootPath, "views")
        services.AddRazorEngine(viewsFolderPath) |> ignore
```

Use the razorView function:

```fsharp
open Giraffe
open Giraffe.Razor

[<CLIMutable>]
type TestModel =
    {
        WelcomeText : string
    }

let model = { WelcomeText = "Hello, World" }

let app =
    choose [
        // Assuming there is a view called "Index.cshtml"
        route  "/" >=> razorHtmlView "Index" model
    ]
```

### validateAntiforgeryToken

`validateAntiforgeryToken` allows one to validate an anti forgery token created by the `Microsoft.AspNetCore.Antiforgery` API.

#### Example:

Add the razor engine service during start-up:

```fsharp
open Giraffe
open Giraffe.Razor

type Startup() =
    member __.ConfigureServices (services : IServiceCollection, env : IHostingEnvironment) =
        let viewsFolderPath = Path.Combine(env.ContentRootPath, "views")
        services.AddRazorEngine(viewsFolderPath) |> ignore
```

Inside a razor view add an anti forgery token:

```html
<form action="/submit-sth">
   @Html.AntiforgeryToken
   <input type="submit">
</form>
```

Use the `validateAntiforgeryToken` function to validate the form request:

```fsharp
open Giraffe
open Giraffe.Razor

let invalidCSRFTokenHandler = RequestErrors.badRequest "The CSRF token was invalid"

let app =
    POST
    >=> route "/submit-sth"
    >=> validateAntiforgeryToken invalidCSRFTokenHandler
    >=> Successful.OK "All good"
```

## Samples

Please find a fully functioning sample application under [./samples/GiraffeRazorSample/](https://github.com/giraffe-fsharp/Giraffe.Razor/tree/master/samples/GiraffeRazorSample).

## Nightly builds and NuGet feed

All official Giraffe packages are published to the official and public NuGet feed.

Unofficial builds (such as pre-release builds from the `develop` branch and pull requests) produce unofficial pre-release NuGet packages which can be pulled from the project's public NuGet feed on AppVeyor:

```
https://ci.appveyor.com/nuget/giraffe-razor
```

If you add this source to your NuGet CLI or project settings then you can pull unofficial NuGet packages for quick feature testing or urgent hot fixes.

## More information

For more information about Giraffe, how to set up a development environment, contribution guidelines and more please visit the [main documentation](https://github.com/giraffe-fsharp/Giraffe/blob/master/DOCUMENTATION.md) page.

## License

[Apache 2.0](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe.Razor/master/LICENSE)