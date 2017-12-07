# Giraffe.Razor

![Giraffe](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe/master/giraffe.png)

Razor view engine http handlers for the Giraffe web framework.

[![NuGet Info](https://buildstats.info/nuget/Giraffe.Razor?includePreReleases=true)](https://www.nuget.org/packages/Giraffe.Razor/)

| Windows | Linux |
| :------ | :---- |
| [![Windows Build status](https://ci.appveyor.com/api/projects/status/914030ec0lrc0vti/branch/master?svg=true)](https://ci.appveyor.com/project/dustinmoris/giraffe-razor/branch/master) | [![Linux Build status](https://travis-ci.org/giraffe-fsharp/GiraffeRazor.svg?branch=develop)](https://travis-ci.org/giraffe-fsharp/Giraffe.Razor/builds?branch=develop) |
| [![Windows Build history](https://buildstats.info/appveyor/chart/dustinmoris/giraffe-razor?branch=develop&includeBuildsFromPullRequest=false)](https://ci.appveyor.com/project/dustinmoris/giraffe-razor/history) | [![Linux Build history](https://buildstats.info/travisci/chart/giraffe-fsharp/Giraffe.Razor?branch=develop&includeBuildsFromPullRequest=false)](https://travis-ci.org/giraffe-fsharp/Giraffe.Razor/builds?branch=develop) |

## Table of contents

- [Documentation](#documentation)
    - [razorView](#razorview)
    - [razorHtmlView](#razorhtmlview)
- [Samples](#samples)
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
open Giraffe.Razor

type Startup() =
    member __.ConfigureServices (services : IServiceCollection, env : IHostingEnvironment) =
        let viewsFolderPath = Path.Combine(env.ContentRootPath, "views")
        services.AddRazorEngine(viewsFolderPath) |> ignore
```

Use the razorView function:

```fsharp
open Giraffe.Razor

let model = { WelcomeText = "Hello World" }

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
open Giraffe.Razor

type Startup() =
    member __.ConfigureServices (services : IServiceCollection, env : IHostingEnvironment) =
        let viewsFolderPath = Path.Combine(env.ContentRootPath, "views")
        services.AddRazorEngine(viewsFolderPath) |> ignore
```

Use the razorView function:

```fsharp
open Giraffe.Razor

let model = { WelcomeText = "Hello World" }

let app =
    choose [
        // Assuming there is a view called "Index.cshtml"
        route  "/" >=> razorHtmlView "Index" model
    ]
```

## Samples

Please find a fully functioning sample application under [./samples/GiraffeRazorSample/](https://github.com/giraffe-fsharp/Giraffe.Razor/tree/master/samples/GiraffeRazorSample).

## More information

For more information about Giraffe, how to set up a development environment, contribution guidelines and more please visit the [main documentation](https://github.com/giraffe-fsharp/Giraffe#table-of-contents) page.

## License

[Apache 2.0](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe.Razor/master/LICENSE)