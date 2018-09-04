module GiraffeRazorSample

open System
open System.IO
open System.Threading
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Http.Features
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open FSharp.Control.Tasks.V2.ContextInsensitive
open Giraffe.Razor
open Microsoft.AspNetCore.Mvc.ViewFeatures
open System.Collections.Generic

// ---------------------------------
// Models
// ---------------------------------

[<CLIMutable>]
type Person =
    {
        Name : string
    }

// ---------------------------------
// Web app
// ---------------------------------

let bytesToKbStr (bytes : int64) =
    sprintf "%ikb" (bytes / 1024L)

let displayFileInfos (files : IFormFileCollection) =
    files
    |> Seq.fold (fun acc file ->
        sprintf "%s\n\n%s\n%s" acc file.FileName (bytesToKbStr file.Length)) ""
    |> text

let smallFileUploadHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            return!
                (match ctx.Request.HasFormContentType with
                | false -> text "Bad request" |> RequestErrors.badRequest
                | true  -> ctx.Request.Form.Files |> displayFileInfos) next ctx
        }

let largeFileUploadHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let formFeature = ctx.Features.Get<IFormFeature>()
            let! form = formFeature.ReadFormAsync CancellationToken.None
            return! (form.Files |> displayFileInfos) next ctx
        }

let renderPerson = 
    let modelView = ViewDataAndModel ([("title", box "Mr")], { Name = "Razor" })
    razorHtmlView "Person" modelView
        

let webApp =
    choose [
        GET >=>
            choose [
                route  "/"       >=> text "index"
                route  "/razor"  >=> razorView "text/html" "Hello" (Model ())
                route  "/person" >=> renderPerson
                route  "/upload" >=> razorHtmlView "FileUpload" (Model ())
            ]
        POST >=>
            choose [
                route "/small-upload" >=> smallFileUploadHandler
                route "/large-upload" >=> largeFileUploadHandler
            ]
        text "Not Found" |> RequestErrors.notFound ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> ServerErrors.INTERNAL_ERROR (text ex.Message)

// ---------------------------------
// Main
// ---------------------------------

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffeErrorHandler(errorHandler)
       .UseStaticFiles()
       .UseAuthentication()
       .UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    let sp  = services.BuildServiceProvider()
    let env = sp.GetService<IHostingEnvironment>()
    Path.Combine(env.ContentRootPath, "Views")
    |> services.AddRazorEngine
    |> ignore

let configureLogging (loggerBuilder : ILoggingBuilder) =
    loggerBuilder.AddFilter(fun lvl -> lvl.Equals LogLevel.Error)
                 .AddConsole()
                 .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    WebHost.CreateDefaultBuilder()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0