![Giraffe](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe/master/giraffe.png)

# Giraffe.Razor

Razor view engine support for the [Giraffe](https://github.com/giraffe-fsharp/Giraffe) web framework.

[![NuGet Info](https://buildstats.info/nuget/Giraffe.Razor?includePreReleases=true)](https://www.nuget.org/packages/Giraffe.Razor/)

### Linux, macOS and Windows Build Status

![.NET Core](https://github.com/giraffe-fsharp/Giraffe.Razor/workflows/.NET%20Core/badge.svg?branch=develop)

[![Build history](https://buildstats.info/github/chart/giraffe-fsharp/Giraffe.Razor?branch=develop&includeBuildsFromPullRequest=false)](https://github.com/giraffe-fsharp/Giraffe.Razor/actions?query=branch%3Adevelop++)

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

In order to use the Razor functionality in an (ASP.NET Core) Giraffe application you'll need to register additional dependencies through the `AddRazorEngine` extension method during application start-up:

```fsharp
open Giraffe
open Giraffe.Razor

type Startup() =
    member __.ConfigureServices (svc : IServiceCollection,
                                 env : IHostingEnvironment) =
        let viewsFolderPath =
            Path.Combine(env.ContentRootPath, "Views")
        svc.AddRazorEngine viewsFolderPath |> ignore
```

If your all of your Razor views are kept in a Razor class library, then you do not need to specify a views folder path when registering the Razor dependencies. In this case there is an overload of `AddRazorEngine` which takes no arguments:

```fsharp
open Giraffe
open Giraffe.Razor

type Startup() =
    member __.ConfigureServices (svc : IServiceCollection,
                                 env : IHostingEnvironment) =
        svc.AddRazorEngine() |> ignore
```

### razorView

The `razorView` http handler utilises the official ASP.NET Core MVC Razor view engine to compile a view into a HTML page and sets the body of the `HttpResponse` object. It requires the content type, the view name, an optional view model, an optional view data dictionary, and an optional model state dictionary as input parameters.

#### Example:

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
        route  "/" >=> razorView "text/html" "Index" (Some model) None None
    ]
```

### razorHtmlView

The `razorHtmlView` http handler is the same as the `razorView` handler except that it automatically sets the response's content type to `text/html; charset=utf-8`.

#### Example:

Use the razorView function:

```fsharp
open Giraffe
open Giraffe.Razor
open Microsoft.AspNetCore.Mvc.ModelBinding

[<CLIMutable>]
type TestModel =
    {
        WelcomeText : string
    }

let model = { WelcomeText = "Hello, World" }

let viewData =
    dict [
        "Title", "Hello World" :> obj
        "Foo", 89 :> obj
        "Bar", true :> obj
    ]

let modelState = ModelStateDictionary()

let app =
    choose [
        // Assuming there is a view called "Index.cshtml"
        route  "/" >=> razorHtmlView "Index" (Some model) (Some viewData) (Some modelState)
    ]
```

### validateAntiforgeryToken

The `validateAntiforgeryToken` http handler allows one to validate an anti forgery token created by the `Microsoft.AspNetCore.Antiforgery` API.

#### Example:

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

All official release packages are published to the official and public NuGet feed.

Nightly builds (builds from the `develop` branch) produce unofficial pre-release packages which can be pulled from the [project's NuGet feed on GitHub](https://github.com/orgs/giraffe-fsharp/packages).

These packages are being tagged with the Workflow's run number as the package version.

All other builds, such as builds triggered by pull requests produce a NuGet package which can be downloaded as an artifact from the individual GitHub action.

## More information

For more information about Giraffe, how to set up a development environment, contribution guidelines and more please visit the [main documentation](https://github.com/giraffe-fsharp/Giraffe/blob/master/DOCUMENTATION.md) page.

## License

[Apache 2.0](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe.Razor/master/LICENSE)