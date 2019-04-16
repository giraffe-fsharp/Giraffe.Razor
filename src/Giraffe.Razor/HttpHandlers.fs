namespace Giraffe.Razor
open Microsoft.AspNetCore.Mvc.ViewFeatures

[<AutoOpen>]
module HttpHandlers =

    open System.Text
    open System.Threading.Tasks
    open System.Collections.Generic
    open Microsoft.AspNetCore.Http
    open Microsoft.AspNetCore.Mvc.ModelBinding
    open Microsoft.AspNetCore.Mvc.Razor
    open Microsoft.AspNetCore.Mvc.ViewFeatures
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.AspNetCore.Antiforgery
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Giraffe
    open RazorEngine

    /// Reads a razor view from disk and compiles it with the given model and sets
    /// the compiled output as the HTTP reponse with the given contentType.
    let razorView (contentType : string)
                  (viewName    : string)
                  (model       : 'T option)
                  (viewData    : IDictionary<string, obj> option)
                  (modelState  : ModelStateDictionary option) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let engine = ctx.RequestServices.GetService<IRazorViewEngine>()
                let tempDataDict = ctx.RequestServices.GetService<ITempDataDictionaryFactory>().GetTempData ctx
                let! result = renderView engine tempDataDict ctx viewName model viewData modelState
                match result with
                | Error msg -> return (failwith msg)
                | Ok output ->
                    let bytes = Encoding.UTF8.GetBytes output
                    return! (setHttpHeader "Content-Type" contentType >=> setBody bytes) next ctx
            }

    /// Reads a razor view from disk and compiles it with the given model and sets
    /// the compiled output as the HTTP reponse with a Content-Type of text/html.
    let razorHtmlView (viewName   : string)
                      (model      : 'T option)
                      (viewData   : IDictionary<string, obj> option)
                      (modelState : ModelStateDictionary option) : HttpHandler =
        razorView "text/html; charset=utf-8" viewName model viewData modelState

    /// Validates an anti forgery token.
    /// If the token is valid the handler will procceed as normal,
    /// otherwise it will execute the invalidTokenHandler.
    let validateAntiforgeryToken (invalidTokenHandler : HttpHandler) : HttpHandler =
        fun next ctx ->
            task {
                let antiforgery = ctx.GetService<IAntiforgery>()
                let! isValid    = antiforgery.IsRequestValidAsync ctx
                return!
                    if isValid then next ctx
                    else invalidTokenHandler (Some >> Task.FromResult) ctx
            }